using Apollon.Mud.Server.Domain.Interfaces.Game;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.ModelExtensions;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.Game.Chat;
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

            var answer = string.Empty;
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
                default:
                    var match = Regex.Match(message, "(untersuche )((\\w|\\d)+(\\w|\\d|\\s)*)");
                    if (match.Success)
                    {
                        await InspectObject(match.Groups[2].Value, dungeon.Id, avatarConnection);

                        return;
                    }

                    match = Regex.Match(message, "(gehe )(n(ord(en)?)?|s(\u00FCd(en)?)?|o(st(en)?)?|w(est(en)?)?)");
                    if (match.Success)
                    {
                        await Move(match.Groups[2].Value, dungeon.Id, avatar.Id);

                        return;
                    }

                    return;
            }
        }

        private async Task<string> GenerateRoomDescription(Guid dungeonId, Guid roomId)
        {
            throw new NotImplementedException();
        }

        private async Task Move(string value, Guid id, Guid avatarId)
        {
            throw new NotImplementedException();
        }

        private async Task InspectObject(string inspectableName, Guid dungeonId, Connection avatarConnection)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            if (dungeon is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage("Fehler - Kein untersuchbares Objekt gefunden!\n");

                return;
            }

            var inspectable = dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Name.NormalizeString() == inspectableName);

            if (inspectable is null)
            {
                await HubContext.Clients.Client(avatarConnection.GameConnectionId).ReceiveGameMessage($"Hier gibt es kein untersuchbares Objekt mit dem Namen { inspectableName }!\n");
            }
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

            var avatarsToNotify = dungeon.RegisteredAvatars.Where(x => x.CurrentRoom == avatar.CurrentRoom && x.Status == Status.Approved).ToList();

            var avatarsToNotifyConnectionIds =
                from avatarToNotify
                        in avatarsToNotify
                select ConnectionService.GetConnectionByAvatarId(avatarToNotify.Id)
                into avatarToNotifyConnection
                where avatarToNotifyConnection is not null
                select avatarToNotifyConnection.GameConnectionId;

            var roomChatPartnerDtos = avatarsToNotify.Select(x => new ChatPartnerDto
            {
                AvatarId = x.Id,
                AvatarName = x.Name
            }).ToList();

            var dungeonChatPartnerDtos = dungeon.RegisteredAvatars
                .Where(x => x.Status == Status.Approved)
                .Select(x => new ChatPartnerDto
                {
                    AvatarId = x.Id,
                    AvatarName = x.Name
                }).ToList();

            await HubContext.Clients.Clients(avatarsToNotifyConnectionIds)
                .ReceiveChatPartnerList(roomChatPartnerDtos);

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeon.Id);

            if (dungeonMasterConnection is not null) await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                .ReceiveChatPartnerList(dungeonChatPartnerDtos);

            return true;
        }

        private async Task<string> GenerateInventoryList(Guid avatarId)
        {
            var avatar = await GameDbService.Get<Avatar>(avatarId);

            return avatar is null
                ? "Fehler - Kein Inventar gefunden!\n"
                : avatar.Inventory.Aggregate(string.Empty, (s, t) => s + $"\t- { t.Name }\n");
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

            return room.SpecialActions.Where(specialAction => specialAction.Status is not Status.Pending)
                .Aggregate("\t- Hilfe --- Gibt eine Hilfe zu den m\u00F6glichen Befehlen des Raumes aus\n" +
                           "\t- Schaue --- Gibt Beschreibung des Raumes aus\n" +
                           "\t- Untersuche <Objekt> --- Gibt Beschreibung des zu untersuchenden Objektes aus\n" +
                           "\t- Gehe <Himmelsrichtung> --- Bewegt den Avatar in den Raum in der gew\u00FCnschten Himmelsrichtung\n" +
                           "\t- Nimm <Gegenstand> --- Nimmt den gew\u00FCnschten Gegenstand in den Inventar auf\n" +
                           "\t- Halte <Gegenstand> --- L\u00E4sst den Avatar den gew\u00FCnschten Gegenstand aus dem Inventar in der Hand halten\n" +
                           "\t- Verstaue <Gegenstand> --- Bef\u00F6rdert den gehaltenen Gegenstand zur\u00FCck in den Inventar\n" +
                           "\t- Wirf <Gegenstand> --- Bef\u00F6rdert einen Gegenstand aus der Hand zur\u00FCck in den aktuellen Raum\n" +
                           "\t- Konsumiere <Nahrung> --- L\u00E4sst den Avatar den gew\u00FCnschten Gegenstand aus der Hand konsumieren\n" +
                           "\t- Ziehe <Kleidungsst\u00FCck> an --- L\u00E4sst den Avatar das gew\u00FCnschte Kleidungsst\u00FCck anziehen\n" +
                           "\t- Eigenschaften --- Gibt die Eigenschaften des eigenen Avatars aus\n" +
                           "\t- Inventar --- Gibt den aktuellen Inhalt des Inventars aus\n" +
                           "\t- Beende --- L\u00E4sst den Avatar das Spiel verlassen\n",
                    (current, specialAction) =>
                        current + $"\t- {specialAction.PatternForPlayer} --- Befehl, der von Dungeon Master bearbeitet wird\n");
        }
    }
}