using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.Dungeon.Avatar;
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

        /// <summary>
        /// Creates a new Instance of MasterService
        /// </summary>
        /// <param name="gameDbService">The GameDbService to manipulate the database</param>
        /// <param name="connectionService">The ConnectionService storing all active connections</param>
        /// <param name="hubContext">The HubContext to contact the clients</param>
        public MasterService(IGameDbService gameDbService, IConnectionService connectionService, IHubContext<GameHub, IClientGameHubContract> hubContext)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            HubContext = hubContext;
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
        }

        /// <inheritdoc cref="IMasterService.KickAllAvatars"/>
        public async Task KickAllAvatars(Guid dungeonId)
        {
            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return;

            var activeAvatars = dungeon.RegisteredAvatars.Where(x => x.Status is Status.Approved);

            foreach (var avatar in activeAvatars)
            {
                var connection = ConnectionService.GetConnectionByAvatarId(avatar.Id);

                if (connection is null) continue;

                avatar.Status = Status.Pending;

                if (!await GameDbService.NewOrUpdate(avatar)) continue;

                ConnectionService.RemoveConnectionByAvatarId(avatar.Id);

                await HubContext.Clients.Client(connection.GameConnectionId).NotifyKicked();
            }

            var dungeonChatPartnerDtos = dungeon.RegisteredAvatars
                .Where(x => x.Status == Status.Approved)
                .Select(x => new ChatPartnerDto
                {
                    AvatarId = x.Id,
                    AvatarName = x.Name
                }).ToList();

            var dungeonMasterConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeon.Id);

            if (dungeonMasterConnection is not null) await HubContext.Clients.Client(dungeonMasterConnection.GameConnectionId)
                .ReceiveChatPartnerList(dungeonChatPartnerDtos);
        }
    }
}