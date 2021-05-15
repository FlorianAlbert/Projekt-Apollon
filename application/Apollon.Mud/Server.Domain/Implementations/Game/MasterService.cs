using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Client.Services.Implementiations;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Race;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.Game.Chat;
using Apollon.Mud.Shared.HubContract;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Game
{
    /// <inheritdoc cref="IMasterService"/>
    public class MasterService : IMasterService
    {
        private IGameDbService GameDbService { get; }

        private IHubContext<GameHub, IClientGameHubContract> HubContext { get; }

        private IConnectionService ConnectionService { get; }

        private IPlayerService PlayerService { get; }

        /// <summary>
        /// Creates a new Instance of MasterService
        /// </summary>
        /// <param name="gameDbService">The GameDbService to manipulate the database</param>
        /// <param name="connectionService">The ConnectionService storing all active connections</param>
        /// <param name="hubContext">The HubContext to contact the clients</param>
        public MasterService(IGameDbService gameDbService, IConnectionService connectionService, IHubContext<GameHub, 
            IClientGameHubContract> hubContext, IPlayerService playerService)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            HubContext = hubContext;
            PlayerService = playerService;
        }

        /// <inheritdoc cref="IMasterService.ExecuteDungeonMasterRequestResponse"/>
        public async Task ExecuteDungeonMasterRequestResponse(string message, Guid avatarId, int newHpValue, Guid dungeonId)
        {
            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (connection is null) return;

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            avatar.CurrentHealth = newHpValue;

            if (!await GameDbService.NewOrUpdate(avatar)) return;

            await HubContext.Clients.Client(connection.GameConnectionId).ReceiveGameMessage(message);
        }

        /// <inheritdoc cref="IMasterService.KickAvatar"/>
        public async Task KickAvatar(Guid avatarId, Guid dungeonId)
        {
            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (connection is null) return;

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var avatar = dungeon?.RegisteredAvatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            avatar.Status = Status.Pending;

            if (!await GameDbService.NewOrUpdate(avatar)) return;

            ConnectionService.RemoveConnectionByAvatarId(avatarId);

            await HubContext.Clients.Client(connection.GameConnectionId).NotifyKicked();
            
            await PlayerService.NotifyAvatarLeftDungeon(avatar.Name, dungeonId, avatar.CurrentRoom.Id);
        }

        /// <inheritdoc cref="IMasterService.KickAllAvatars"/>
        public async Task KickAllAvatars(Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return;

            var activeAvatars = dungeon.RegisteredAvatars.Where(x => x.Status is Status.Approved);

            foreach (var avatar in activeAvatars)
            {
                avatar.Status = Status.Pending;

                if (!await GameDbService.NewOrUpdate(avatar)) continue;

                var connection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

                if (connection is null) continue;

                ConnectionService.RemoveConnectionByAvatarId(avatar.Id);

                await HubContext.Clients.Client(connection.GameConnectionId).NotifyKicked();
            }

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
                            Status = (int)x.Status,
                            Owner = new DungeonUserDto
                            {
                                Id = Guid.Parse(x.Owner.Id),
                                Email = x.Owner.Email,
                                LastActive = x.Owner.LastActive
                            }
                        }).ToList());
            }
        }

        public async Task<bool> EnterDungeon(DungeonUser user, Guid sessionId, string chatConnectionId, string gameConnectionId, Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return false;

            dungeon.CurrentDungeonMaster = user;
            if (!await GameDbService.NewOrUpdate(dungeon)) return false;

            ConnectionService.AddConnection
            (Guid.Parse(user.Id),
                sessionId,
                chatConnectionId,
                gameConnectionId,
                dungeonId,
                null);

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeon.Id);
            if (dungeonMasterConnection is null) return false;

            await PlayerService.NotifyDungeonMasterEntering(dungeonId);
            return true;
        }

        public async Task<bool> LeaveDungeon(Guid dungeonId, Guid userId, Guid sessionId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return false;

            dungeon.CurrentDungeonMaster = null;
            if (!await GameDbService.NewOrUpdate(dungeon)) return false;

            ConnectionService.RemoveConnection(userId, sessionId);

            await PlayerService.NotifyDungeonMasterLeaving(dungeonId);
            return true;
        }
    }
}