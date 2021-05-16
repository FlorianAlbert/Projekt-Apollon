using Apollon.Mud.Server.Domain.Interfaces.Game;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.ModelExtensions;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Race;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.Game.Chat;
using Apollon.Mud.Shared.Game.DungeonMaster;
using Apollon.Mud.Shared.HubContract;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Game
{
    /// <inheritdoc cref="IPlayerService"/>
    public class PlayerService : IPlayerService
    {
        private IGameDbService GameDbService { get; }

        private IHubContext<GameHub, IClientGameHubContract> HubContext { get; }

        private IConnectionService ConnectionService { get; }

        /// <summary>
        /// Creates a new Instance of PlayerService
        /// </summary>
        /// <param name="gameDbService">The GameDbService to manipulate the database</param>
        /// <param name="connectionService">The ConnectionService storing all active connections</param>
        /// <param name="hubContext">The HubContext to contact the clients</param>
        public PlayerService(IGameDbService gameDbService, IConnectionService connectionService, IHubContext<GameHub, IClientGameHubContract> hubContext)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            HubContext = hubContext;
        }

        /// <inheritdoc cref="IPlayerService.ValidateCommand"/>
        public async Task ValidateCommand(Guid avatarId, Guid dungeonId, string message)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            var room = avatar?.CurrentRoom;

            if (room is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (avatarConnection is null) return;

            var normalizedMessage = message.NormalizeString();

            string answer;
            switch (normalizedMessage)
            {
                case "hilfe":
                    answer = await GenerateHelp(dungeon.Id, room.Id);

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage(answer);

                    return;
                case "schaue":
                    answer = await GenerateRoomDescription(dungeon.Id, room.Id);

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage(answer);

                    return;
                case "eigenschaften":
                    answer = await GenerateAvatarProperties(avatar.Id);

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage(answer);

                    return;
                case "inventar":
                    answer = await GenerateInventoryList(avatar.Id);

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage(answer);

                    return;
                case "beende":
                    if (await LeaveDungeon(dungeon.Id, avatar.Id))
                    {
                        await HubContext.Clients.Client(avatarConnection.GameConnectionId).NotifyDungeonLeft(true);

                        return;
                    }

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).NotifyDungeonLeft(false);

                    return;
                case "verstaue":
                    await Store(dungeon.Id, avatar.Id);

                    return;
                default:
                    var match = Regex.Match(normalizedMessage, "(untersuche )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await InspectObject(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(gehe )((\\w|\\d)+)");
                    if (match.Success)
                    {
                        await Move(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(nimm )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await Take(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(halte )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await Hold(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(wirf )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await ThrowAway(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(konsumiere )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await Consume(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(ziehe )((\\w|\\d)+(\\w|\\d|\\s)*)( an)");
                    if (match.Success)
                    {
                        await Wear(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    match = Regex.Match(normalizedMessage, "(sprich )((\\w|\\d)+(\\w|\\d|\\s)*)( an)");
                    if (match.Success)
                    {
                        await Talk(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    foreach (var requestable in room.SpecialActions)
                    {
                        if (requestable.Status is Status.Pending) continue;
                        match = Regex.Match(message, requestable.MessageRegex);
                        if (!match.Success) continue;
                        await SpecialAction(match.Groups[0].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage("Diesen Befehl gibt es nicht!");
                    return;
            }
        }

        private async Task Talk(string npcName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var inspectable = avatar.CurrentRoom.Inspectables.FirstOrDefault(x => x.Name.NormalizeString() == npcName.NormalizeString());

            if (inspectable is null || inspectable.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"Hier gibt es keinen NPC mit dem Namen { npcName }!\n");

                return;
            }

            if (inspectable is not Npc npc)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"Mit { inspectable.Name } kannst du nicht sprechen!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"{ npc.Text }\n");
        }

        private async Task SpecialAction(string input, Guid dungeonId, Guid avatarId)
        {
            var avatar = await GameDbService.Get<Avatar>(avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);

            if (dungeonMasterConnection is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Im Moment gibt es wohl keinen Dungeon Master... Benutze wenn möglich die automatisierten Befehle!\n");

                return;
            }

            await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                .ReceiveRequest(new DungeonMasterRequestDto
                {
                    Avatar = new AvatarDto
                    {
                        Id = avatar.Id,
                        Name = avatar.Name,
                        Gender = (int)avatar.ChosenGender,
                        CurrentHealth = avatar.CurrentHealth,
                        CurrentRoom = new RoomDto
                        {
                            Id = avatar.CurrentRoom.Id,
                            Name = avatar.CurrentRoom.Name,
                            Description = avatar.CurrentRoom.Description
                        }
                    },
                    Request = input
                });

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage("Deine Anfrage wird so schnell wie möglich vom Dungeon Master bearbeitet. Du wirst benachrichtigt sobald das Ergebnis bereit steht!\n");
        }

        private async Task Wear(string itemName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var itemToWear = avatar.Inventory.FirstOrDefault(x => x.Name.NormalizeString() == itemName.NormalizeString());
            if (itemToWear is null || itemToWear.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In deinem Inventar existiert kein Item mit diesem Namen!\n");

                return;
            }

            if (itemToWear is not Wearable wearable)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage($"{ itemToWear.Name } kannst du nicht anziehen!\n");

                return;
            }

            if (avatar.Armor is not null)
            {
                var wornWearable = await GameDbService.Get<Wearable>(avatar.Armor.Id);

                avatar.Armor = wearable;

                avatar.Inventory.Remove(wearable);

                avatar.Inventory.Add(wornWearable);

                if (!avatar.Inventory.Contains(wornWearable))
                {
                    var room = await GameDbService.Get<Room>(avatar.CurrentRoom.Id);

                    if (room is null)
                    {
                        await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                            .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                                "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                        return;
                    }

                    room.Inspectables.Add(wornWearable);

                    if (!await GameDbService.NewOrUpdate(room))
                    {
                        await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                            .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                                "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                        return;
                    }

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                        .ReceiveGameMessage($"In deinem Inventar war nicht mehr genügend Platz, deshalb wurde deine zuvor getragene R\u00FCstung { wornWearable.Name } im Raum abgelegt!\n");
                }


            }
            else
            {
                avatar.Armor = wearable;

                avatar.Inventory.Remove(wearable);
            }
            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"Du trägst nun { itemToWear.Name } als Rüstung!\n");
        }

        private async Task Consume(string itemName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var takeable = avatar.Inventory.FirstOrDefault(x => x.Name.NormalizeString() == itemName.NormalizeString());
            if (takeable is null || takeable.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In deinem Inventar existiert kein Item mit diesem Namen!\n");

                return;
            }

            if (takeable is not Consumable consumable)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage($"{ takeable.Name } kann nicht konsumiert werden!\n");

                return;
            }

            avatar.Inventory.Remove(consumable);

            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"{ consumable.EffectDescription }\n");
        }

        private async Task ThrowAway(string itemName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var itemToThrowAway = avatar.Inventory.FirstOrDefault(x => x.Name.NormalizeString() == itemName.NormalizeString());
            if (itemToThrowAway is null || itemToThrowAway.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In deinem Inventar existiert kein Item mit diesem Namen!\n");

                return;
            }

            var room = await GameDbService.Get<Room>(avatar.CurrentRoom.Id);

            if (room is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            room.Inspectables.Add(itemToThrowAway);

            if (!await GameDbService.NewOrUpdate(room))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            avatar.Inventory.Remove(itemToThrowAway);

            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"{ itemToThrowAway.Name } befindet sich jetzt wieder im Raum!\n");
        }

        private async Task Store(Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            if (avatar.HoldingItem is null || avatar.HoldingItem.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Du hältst im Moment gar kein Item in der Hand!\n");

                return;
            }

            var itemToStore = await GameDbService.Get<Takeable>(avatar.HoldingItem.Id);

            if (itemToStore is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            avatar.Inventory.Add(avatar.HoldingItem);

            if (!avatar.Inventory.Contains(avatar.HoldingItem))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In deinem Inventar ist nicht genügend Platz, um dieses Item noch unterzubringen!\n");

                return;
            }

            avatar.HoldingItem = null;

            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"{ itemToStore.Name } befindet sich nun wieder in deinem Inventar!\n");
        }

        private async Task Hold(string itemName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var itemToHold = avatar.Inventory.FirstOrDefault(x => x.Name.NormalizeString() == itemName.NormalizeString());
            if (itemToHold is null || itemToHold.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In deinem Inventar existiert kein Item mit diesem Namen!\n");

                return;
            }

            if (avatar.HoldingItem is not null)
            {
                var heldItem = await GameDbService.Get<Takeable>(avatar.HoldingItem.Id);

                avatar.HoldingItem = itemToHold;

                avatar.Inventory.Remove(itemToHold);

                avatar.Inventory.Add(heldItem);

                if (!avatar.Inventory.Contains(heldItem))
                {
                    var room = await GameDbService.Get<Room>(avatar.CurrentRoom.Id);

                    if (room is null)
                    {
                        await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                            .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                                "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                        return;
                    }

                    room.Inspectables.Add(heldItem);

                    if (!await GameDbService.NewOrUpdate(room))
                    {
                        await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                            .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                                "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                        return;
                    }

                    await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                        .ReceiveGameMessage($"In deinem Inventar war nicht mehr genügend Platz, deshalb wurde dein zuvor gehaltenes Item { heldItem.Name } im Raum abgelegt!\n");
                }
            }
            else
            {
                avatar.HoldingItem = itemToHold;

                avatar.Inventory.Remove(itemToHold);
            }

            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"Du hältst nun { itemToHold.Name } in der Hand!\n");
        }

        private async Task Take(string itemName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var inspectable = avatar.CurrentRoom.Inspectables.FirstOrDefault(x => x.Name.NormalizeString() == itemName.NormalizeString());

            if (inspectable is null || inspectable.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Hier gibt es kein Objekt mit diesem Namen!\n");

                return;
            }

            if (inspectable is not Takeable takeable)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage($"{ inspectable.Name } kannst du leider nicht aufnehmen!\n");

                return;
            }

            var room = await GameDbService.Get<Room>(avatar.CurrentRoom.Id);

            if (room is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Kein Objekt gefunden!\n");

                return;
            }

            avatar.Inventory.Add(takeable);

            if (!await GameDbService.NewOrUpdate(avatar))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

                return;
            }

            if (!avatar.Inventory.Contains(takeable))
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Dein Inventar kann dieses Item nicht mehr aufnehmen! Es ist wohl zu schwer!\n");

                return;
            }

            room.Inspectables.Remove(takeable);

            if (!await GameDbService.NewOrUpdate(room))
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("Fehler - Da lief wohl etwas schief...\n" +
                                        "Benachrichtige bitte einen der Verantwortlichen, die im Impressum aufgeführt sind und erläutere den Fehler!\n");

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage($"Dein Inventar enthält nun { takeable.Name }!\n");

        }

        private async Task<string> GenerateRoomDescription(Guid dungeonId, Guid roomId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null) return "Fehler - Keine Raumbeschreibung gefunden!\n";

            var description = $"\t{ room.Description }\n";

            if (room.Inspectables.Count != 0)
            {
                description += "\n\tGegenstände:\n";
                description = room.Inspectables.Where(x => x.Status is Status.Approved).Aggregate(description,
                    (current, inspectable) => current + $"\t\t{ inspectable.Name }\n");
            }

            if (room.Avatars.Count == 0) return description;

            description += "\n\tAvatare:\n";
            description = room.Avatars.Where(x => x.Status is Status.Approved).Aggregate(description,
                (current, avatar) => current + $"\t\t{ avatar.Name }\n");

            return description;
        }

        private async Task Move(string direction, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            Room newRoom;
            switch (direction.NormalizeString())
            {
                case "n":
                case "nord":
                case "norden":
                    newRoom = avatar.CurrentRoom.NeighborNorth;
                    break;
                case "s":
                case "s\u00FCd":
                case "s\u00FCden":
                    newRoom = avatar.CurrentRoom.NeighborSouth;
                    break;
                case "o":
                case "ost":
                case "osten":
                    newRoom = avatar.CurrentRoom.NeighborEast;
                    break;
                case "w":
                case "west":
                case "westen":
                    newRoom = avatar.CurrentRoom.NeighborWest;
                    break;
                default:
                    await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                        .ReceiveGameMessage("Eine solche Himmelsrichtung ist leider nicht bekannt!\n");
                    return;
            }

            if (newRoom is null || newRoom.Status is Status.Pending)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                    .ReceiveGameMessage("In dieser Himmelsrichtung ist leider kein Raum vorhanden!\n");

                return;
            }

            var oldRoom = avatar.CurrentRoom;

            avatar.CurrentRoom = newRoom;

            if (!await GameDbService.NewOrUpdate(avatar)) return;

            var roomDescription = await GenerateRoomDescription(dungeon.Id, newRoom.Id);

            await HubContext.Clients.Client(avatarConnection.GameConnectionId)
                .ReceiveGameMessage(roomDescription);

            await NotifyAvatarLeftRoom(avatar.Name, dungeon.Id, oldRoom.Id);

            await NotifyAvatarEnteredRoom(avatar.Name, dungeon.Id, avatar.CurrentRoom.Id);
        }

        private async Task InspectObject(string inspectableName, Guid dungeonId, Guid avatarId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            var avatarConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

            if (avatarConnection is null) return;

            var inspectable = avatar.CurrentRoom.Inspectables.FirstOrDefault(x => x.Name.NormalizeString() == inspectableName.NormalizeString());

            if (inspectable?.Status is Status.Approved)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"{ inspectable.Description }\n");

                return;
            }

            var avatarToInspect = avatar.CurrentRoom.Avatars.FirstOrDefault(x => x.Name.NormalizeString() == inspectableName.NormalizeString());

            if (avatarToInspect?.Status is Status.Approved)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"{ avatarToInspect.Description }\n");

                return;
            }

            await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"Hier gibt es kein untersuchbares Objekt mit dem Namen { inspectableName }!\n");
        }

        private async Task<bool> LeaveDungeon(Guid dungeonId, Guid avatarId)
        {
            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (connection is null) return false;

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return false;

            avatar.Status = Status.Pending;

            if (!await GameDbService.NewOrUpdate(avatar)) return false;

            ConnectionService.RemoveConnectionByAvatarId(avatarId);

            await NotifyAvatarLeftDungeon(avatar.Name, dungeon.Id, avatar.CurrentRoom.Id);

            return true;
        }

        private async Task<string> GenerateInventoryList(Guid avatarId)
        {
            var avatar = await GameDbService.Get<Avatar>(avatarId);

            if (avatar is null) return "Fehler - Kein Inventar gefunden!\n";

            return avatar.Inventory.Count == 0
                ? "\tMaximalgewicht: 100\n\tInventar ist leer"
                : avatar.Inventory.Where(x => x.Status is Status.Approved)
                    .Aggregate("\tMaximalgewicht: 100\n", (s, t) => s + $"\t- { t.Name } (Gewicht: { t.Weight })\n");
        }

        private async Task<string> GenerateAvatarProperties(Guid avatarId)
        {
            var avatar = await GameDbService.Get<Avatar>(avatarId);

            if (avatar is null) return "Fehler - Keine Eigenschaften gefunden!\n";

            return $"\tName: { avatar.Name }\n" +
                   $"\tGeschlecht: { avatar.ChosenGender }\n" +
                   $"\tRasse: { avatar.ChosenRace.Name }\n" +
                   $"\tKlasse: { avatar.ChosenClass.Name }\n" +
                   $"\tGesundheit: { avatar.CurrentHealth }\n" +
                   $"\tSchaden: { avatar.Damage }\n" +
                   $"\tSchutz: { avatar.Protection }\n" +
                   $"\tR\u00FCstung: { (avatar.Armor is null ? "Keine" : avatar.Armor.Name) }\n" +
                   $"\tGehaltener Gegenstand: { (avatar.HoldingItem is null ? "Keiner" : avatar.HoldingItem.Name) }\n";
        }

        private async Task<string> GenerateHelp(Guid dungeonId, Guid roomId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null) return "Fehler - Keine Hilfe gefunden!\n";

            return room.SpecialActions.Where(specialAction => specialAction.Status is Status.Approved)
                .Aggregate("\t- Hilfe --- Gibt eine Hilfe zu den m\u00F6glichen Befehlen des Raumes aus\n" +
                           "\t- Schaue --- Gibt Beschreibung des Raumes aus\n" +
                           "\t- Untersuche <Objekt> --- Gibt Beschreibung des zu untersuchenden Objektes aus\n" +
                           "\t- Gehe <Himmelsrichtung> --- Bewegt den Avatar in den Raum in der gew\u00FCnschten Himmelsrichtung\n" +
                           "\t- Nimm <Gegenstand> --- Nimmt den gew\u00FCnschten Gegenstand aus dem Raum in den Inventar auf\n" +
                           "\t- Halte <Gegenstand> --- L\u00E4sst den Avatar den gew\u00FCnschten Gegenstand aus dem Inventar in der Hand halten\n" +
                           "\t- Verstaue --- Bef\u00F6rdert den gehaltenen Gegenstand zur\u00FCck in den Inventar\n" +
                           "\t- Wirf <Gegenstand> --- Bef\u00F6rdert einen Gegenstand aus der Hand zur\u00FCck in den aktuellen Raum\n" +
                           "\t- Konsumiere <Nahrung> --- L\u00E4sst den Avatar den gew\u00FCnschten Gegenstand aus der Hand konsumieren\n" +
                           "\t- Ziehe <Kleidungsst\u00FCck> an --- L\u00E4sst den Avatar das gew\u00FCnschte Kleidungsst\u00FCck anziehen\n" +
                           "\t- Sprich <NPC> an --- L\u00E4sst den Avatar den gew\u00FCnschten NPC ansprechen\n" +
                           "\t- Eigenschaften --- Gibt die Eigenschaften des eigenen Avatars aus\n" +
                           "\t- Inventar --- Gibt den aktuellen Inhalt des Inventars aus\n" +
                           "\t- Beende --- L\u00E4sst den Avatar das Spiel verlassen\n",
                    (current, specialAction) =>
                        current + $"\t- {specialAction.PatternForPlayer} --- Befehl, der vom Dungeon Master bearbeitet wird\n");
        }

        public async Task NotifyAvatarLeftRoom(string avatarName, Guid dungeonId, Guid roomId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null || room.Status is Status.Pending) return;

            var avatarsToNotify = room.Avatars.Where(x => x.Status == Status.Approved).ToList();

            var avatarsToNotifyConnectionIds =
                (from avatarToNotify
                    in avatarsToNotify
                 select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                into avatarToNotifyConnection
                 where avatarToNotifyConnection is not null
                 select avatarToNotifyConnection.GameConnectionId).ToList();

            var roomChatPartnerDtos = avatarsToNotify.Select(x => new ChatPartnerDto
            {
                AvatarId = x.Id,
                AvatarName = x.Name
            }).ToList();

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);

            if (dungeonMasterConnection is not null) roomChatPartnerDtos.Add(new ChatPartnerDto
            {
                AvatarId = null,
                AvatarName = "Dungeon Master"
            });

            await HubContext.Clients.Clients(avatarsToNotifyConnectionIds)
                .ReceiveChatPartnerList(roomChatPartnerDtos);

            await HubContext.Clients.Clients(avatarsToNotifyConnectionIds)
                .ReceiveGameMessage($"{ avatarName } verlässt den Raum!\n");
        }

        public async Task NotifyAvatarEnteredRoom(string avatarName, Guid dungeonId, Guid roomId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null || room.Status is Status.Pending) return;

            var avatarsToNotify = room.Avatars.Where(x => x.Status == Status.Approved).ToList();

            var avatarsToNotifyConnectionIdsChat =
                from avatarToNotify
                    in avatarsToNotify
                select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                into avatarToNotifyConnection
                where avatarToNotifyConnection is not null
                select avatarToNotifyConnection.GameConnectionId;

            var avatarsToNotifyConnectionIdsMessage =
                from avatarToNotify
                    in avatarsToNotify
                where avatarToNotify.Name != avatarName
                select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                into avatarToNotifyConnection
                where avatarToNotifyConnection is not null
                select avatarToNotifyConnection.GameConnectionId;

            var roomChatPartnerDtos = avatarsToNotify.Select(x => new ChatPartnerDto
            {
                AvatarId = x.Id,
                AvatarName = x.Name
            }).ToList();

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);

            if (dungeonMasterConnection is not null) roomChatPartnerDtos.Add(new ChatPartnerDto
            {
                AvatarId = null,
                AvatarName = "Dungeon Master"
            });

            await HubContext.Clients.Clients(avatarsToNotifyConnectionIdsChat)
                .ReceiveChatPartnerList(roomChatPartnerDtos);

            await HubContext.Clients.Clients(avatarsToNotifyConnectionIdsMessage)
                .ReceiveGameMessage($"{ avatarName } betritt den Raum!\n");
        }

        public async Task NotifyDungeonMasterEntering(Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return;

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);

            if (dungeonMasterConnection is not null)
            {
                foreach (var room in dungeon.ConfiguredRooms)
                {
                    if (room.Status is Status.Pending) continue;

                    var roomAvatars = room.Avatars.Where(x => x.Status is Status.Approved).ToList();

                    var roomAvatarsConnectionIds =
                        (from avatarToNotify
                            in roomAvatars
                         select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                        into avatarToNotifyConnection
                         where avatarToNotifyConnection is not null
                         select avatarToNotifyConnection.GameConnectionId).ToList();

                    var roomChatPartnerDtos = roomAvatars.Select(x => new ChatPartnerDto
                    {
                        AvatarId = x.Id,
                        AvatarName = x.Name
                    }).ToList();

                    roomChatPartnerDtos.Add(new ChatPartnerDto
                    {
                        AvatarId = null,
                        AvatarName = "Dungeon Master"
                    });

                    await HubContext.Clients.Clients(roomAvatarsConnectionIds)
                        .ReceiveGameMessage("Ein Dungeon Master hat das Dungeon betreten!\n");

                    await HubContext.Clients.Clients(roomAvatarsConnectionIds)
                        .ReceiveChatPartnerList(roomChatPartnerDtos);
                }

                var dungeonAvatars = dungeon.RegisteredAvatars.Where(x => x.Status is Status.Approved).ToList();

                var dungeonChatPartnerDtos = dungeonAvatars.Select(x => new ChatPartnerDto
                {
                    AvatarId = x.Id,
                    AvatarName = x.Name
                }).ToList();

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveChatPartnerList(dungeonChatPartnerDtos);

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveAvatarList(dungeonAvatars.Select(x =>
                        new AvatarDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Gender = (int)x.ChosenGender,
                            Status = (int)x.Status,
                            Owner = new DungeonUserDto
                            {
                                Id = Guid.Parse(x.Owner.Id),
                                Email = x.Owner.Email,
                                LastActive = x.Owner.LastActive
                            },
                            Class = new ClassDto
                            {
                                Id = x.ChosenClass.Id,
                                Name = x.ChosenClass.Name,
                                Status = (int)x.ChosenClass.Status
                            },
                            Race = new RaceDto
                            {
                                Id = x.ChosenRace.Id,
                                Name = x.ChosenRace.Name,
                                Status = (int)x.ChosenRace.Status
                            },
                            CurrentRoom = new RoomDto
                            {
                                Id = x.CurrentRoom.Id,
                                Name = x.CurrentRoom.Name,
                                Status = (int)x.CurrentRoom.Status
                            }
                        }).ToList());
            }
        }

        public async Task NotifyDungeonMasterLeaving(Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return;

            foreach (var room in dungeon.ConfiguredRooms)
            {
                if (room.Status is Status.Pending) continue;

                var roomAvatars = room.Avatars.Where(x => x.Status is Status.Approved).ToList();

                var roomAvatarsConnectionIds =
                    (from avatarToNotify
                        in roomAvatars
                     select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                    into avatarToNotifyConnection
                     where avatarToNotifyConnection is not null
                     select avatarToNotifyConnection.GameConnectionId).ToList();

                var roomChatPartnerDtos = roomAvatars.Select(x => new ChatPartnerDto
                {
                    AvatarId = x.Id,
                    AvatarName = x.Name
                }).ToList();

                await HubContext.Clients.Clients(roomAvatarsConnectionIds)
                    .ReceiveGameMessage("Der Dungeon Master hat das Dungeon verlassen!\n");

                await HubContext.Clients.Clients(roomAvatarsConnectionIds)
                    .ReceiveChatPartnerList(roomChatPartnerDtos);
            }

        }

        public async Task NotifyAvatarEnteredDungeon(string avatarName, Guid dungeonId, Guid roomId)
        {
            await NotifyAvatarEnteredRoom(avatarName, dungeonId, roomId);

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null || room.Status is Status.Pending) return;

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeon.Id);

            if (dungeonMasterConnection is not null)
            {
                var dungeonChatPartnerDtos = dungeon.RegisteredAvatars
                    .Where(x => x.Status == Status.Approved)
                    .Select(x => new ChatPartnerDto
                    {
                        AvatarId = x.Id,
                        AvatarName = x.Name
                    }).ToList();

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveChatPartnerList(dungeonChatPartnerDtos);

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveAvatarList(dungeon.RegisteredAvatars.Where(x => x.Status is Status.Approved).Select(x =>
                        new AvatarDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Gender = (int)x.ChosenGender,
                            Status = (int)x.Status,
                            Owner = new DungeonUserDto
                            {
                                Id = Guid.Parse(x.Owner.Id),
                                Email = x.Owner.Email,
                                LastActive = x.Owner.LastActive
                            },
                            Class = new ClassDto
                            {
                                Id = x.ChosenClass.Id,
                                Name = x.ChosenClass.Name,
                                Status = (int)x.ChosenClass.Status
                            },
                            Race = new RaceDto
                            {
                                Id = x.ChosenRace.Id,
                                Name = x.ChosenRace.Name,
                                Status = (int)x.ChosenRace.Status
                            },
                            CurrentRoom = new RoomDto
                            {
                                Id = x.CurrentRoom.Id,
                                Name = x.CurrentRoom.Name,
                                Status = (int)x.CurrentRoom.Status
                            }
                        }).ToList());
            }
        }

        public async Task NotifyAvatarLeftDungeon(string avatarName, Guid dungeonId, Guid roomId)
        {
            await NotifyAvatarLeftRoom(avatarName, dungeonId, roomId);

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null || room.Status is Status.Pending) return;

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeon.Id);

            if (dungeonMasterConnection is not null)
            {
                var dungeonChatPartnerDtos = dungeon.RegisteredAvatars
                    .Where(x => x.Status == Status.Approved)
                    .Select(x => new ChatPartnerDto
                    {
                        AvatarId = x.Id,
                        AvatarName = x.Name
                    }).ToList();

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveChatPartnerList(dungeonChatPartnerDtos);

                await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                    .ReceiveAvatarList(dungeon.RegisteredAvatars.Where(x => x.Status is Status.Approved).Select(x =>
                        new AvatarDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Gender = (int)x.ChosenGender,
                            Status = (int)x.Status,
                            Owner = new DungeonUserDto
                            {
                                Id = Guid.Parse(x.Owner.Id),
                                Email = x.Owner.Email,
                                LastActive = x.Owner.LastActive
                            },
                            Class = new ClassDto
                            {
                                Id = x.ChosenClass.Id,
                                Name = x.ChosenClass.Name,
                                Status = (int)x.ChosenClass.Status
                            },
                            Race = new RaceDto
                            {
                                Id = x.ChosenRace.Id,
                                Name = x.ChosenRace.Name,
                                Status = (int)x.ChosenRace.Status
                            },
                            CurrentRoom = new RoomDto
                            {
                                Id = x.CurrentRoom.Id,
                                Name = x.CurrentRoom.Name,
                                Status = (int)x.CurrentRoom.Status
                            }
                        }).ToList());
            }
        }

        public async Task NotifyAvatarMovedToDefaultRoom(Guid avatarId, Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);
            var room = dungeon?.DefaultRoom;
            if (room is null) return;

            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);
            if (connection is null) return;

            var roomDescription = await GenerateRoomDescription(dungeonId, room.Id);

            await HubContext.Clients.Clients(connection.GameConnectionId)
                .ReceiveGameMessage($"Du wurdest in den Standardraum verschoben!\n + {roomDescription}");
        }

        public async Task<bool> EnterDungeon(Guid userId, Guid sessionId, string chatConnectionId, string gameConnectionId, Guid dungeonId,
            Guid avatarId)
        {

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(a => a.Id == avatarId);
            if (avatar is null) return false;

            avatar.Status = Status.Approved;
            if (!await GameDbService.NewOrUpdate(avatar)) return false;

            ConnectionService.AddConnection
            (userId,
                sessionId,
                chatConnectionId,
                gameConnectionId,
                dungeonId,
                avatarId);

            await NotifyAvatarEnteredDungeon(avatar.Name, dungeonId, avatar.CurrentRoom.Id);
            return true;
        }

        async Task<bool> IPlayerService.LeaveDungeon(Guid avatarId, Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(a => a.Id == avatarId);
            if (avatar is null) return false;

            avatar.Status = Status.Pending;
            if (!await GameDbService.NewOrUpdate(avatar)) return false;

            ConnectionService.RemoveConnectionByAvatarId(avatarId);
            await NotifyAvatarLeftDungeon(avatar.Name, dungeonId, avatar.CurrentRoom.Id);
            return true;
        }
    }
}