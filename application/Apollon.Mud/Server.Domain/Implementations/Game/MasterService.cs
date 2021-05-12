using System;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.HubContract;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Game
{
    /// <summary>
    /// ToDo Muss noch implementiert werden
    /// </summary>
    public class MasterService: IMasterService
    {
        private IGameDbService GameDbService { get; }

        private IHubContext<GameHub, IClientGameHubContract> HubContext { get; }

        private IConnectionService ConnectionService { get; }

        public MasterService(IGameDbService gameDbService, IConnectionService connectionService, IHubContext<GameHub, IClientGameHubContract> hubContext)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            HubContext = hubContext;
        }

        public async Task ExecuteDungeonMasterRequestResponse(string message, Guid avatarRoomId, Guid avatarId, int newHpValue, Guid dungeonId)
        {
            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (connection is null) return;

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == avatarRoomId);

            var avatar = room?.Avatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            avatar.CurrentHealth = newHpValue;

            if (!await GameDbService.NewOrUpdate(avatar)) return;

            await HubContext.Clients.Client(connection.GameConnectionId).ReceiveGameMessage(message);
        }

        public async Task KickAvatar(Guid avatarRoomId, Guid avatarId, Guid dungeonId)
        {
            var connection = ConnectionService.GetConnectionByAvatarId(avatarId);

            if (connection is null) return;

            var dungeon = await GameDbService.Get<Dungeon>(dungeonId);

            var room = dungeon?.ConfiguredRooms.FirstOrDefault(x => x.Id == avatarRoomId);

            var avatar = room?.Avatars.FirstOrDefault(x => x.Id == avatarId);

            if (avatar is null) return;

            avatar.Status = Status.Pending;

            if (!await GameDbService.NewOrUpdate(avatar)) return;

            ConnectionService.RemoveConnectionByAvatarId(avatarId);

            await HubContext.Clients.Client(connection.GameConnectionId).NotifyKicked();
        }

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
        }
    }
}