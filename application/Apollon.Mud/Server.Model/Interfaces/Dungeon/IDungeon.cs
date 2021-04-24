using Apollon.Mud.Server.Model.Implementations.Dungeon;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon
{
    public interface IDungeon : IApprovable
    {
        #region Properties
        string DungeonName { get; set; }

        string DungeonEpoch { get; set; }

        string DungeonDescription { get; set; }

        IRoom DefaultRoom { get; set; }

        ICollection<IRace> ConfiguredRaces { get; }

        ICollection<IClass> ConfiguredClasses { get; }

        ICollection<IRoom> ConfiguredRooms { get; }

        ICollection<IInspectable> ConfiguredInspectables { get; }

        ICollection<IRequestable> ConfiguredRequestables { get; }

        ICollection<IAvatar> RegisteredAvatars { get; }

        ICollection<DungeonUser> DungeonMasters { get; }

        ICollection<DungeonUser> WhiteList { get; }

        ICollection<DungeonUser> BlackList { get; }

        ICollection<DungeonUser> OpenRequests { get; }

        DungeonUser CurrentDungeonMaster { get; set; }

        DungeonUser DungeonOwner { get; set; }

        Visibility Visibility { get; set; }

        #endregion

        /** TODO: Nach PlayerService/KonfigurationsService verlagern 
         * #region Methods
        bool AddNeighborship(IRoom roomSource, Direction direction, IRoom roomSink);

        bool RemoveNeigborship(IRoom room1, IRoom room2);

        IRoom AddRoom(bool defaultRoom = false);

        bool RemoveRoom(IRoom room);

        IRoom GetRoom(Guid roomId);

        void ChangeRoom(IAvatar avatar, Direction direction);

        void EnterDungeon(IAvatar avatar);

        #endregion **/
    }
}
