using System;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.Game
{
    /// <summary>
    /// ToDo Muss noch Implementiert werden
    /// </summary>
    public interface IMasterService
    {
        Task ExecuteDungeonMasterRequestResponse(string message, Guid avatarRoomId, Guid avatarId,
            int newHpValue, Guid dungeonId);

        Task KickAvatar(Guid avatarRoomId, Guid avatarId, Guid dungeonId);
    }
}