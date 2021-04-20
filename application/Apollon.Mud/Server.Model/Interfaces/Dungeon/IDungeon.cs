using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon
{
    public interface IDungeon : IApprovable
    {
        #region Properties
        string DungeonEpoch { get; set; }

        string DungeonDescription { get; set; }

        IRoom DefaultRoom { get; set; }

        ICollection<IRace> ConfiguredRaces { get; set; }

        ICollection<IClass> ConfiguredClasses { get; set; }

        ICollection<IRoom> ConfiguredRooms { get; set; }

        ICollection<IInspectable> ConfiguredInspectables { get; set; }

        ICollection<IRequestable> ConfiguredRequestables { get; set; }

        ICollection<IAvatar> RegisteredAvatars { get; set; }

        ICollection<DungeonUser> DungeonMasters { get; set; }

        ICollection<DungeonUser> WhiteList { get; set; }

        ICollection<DungeonUser> BlackList { get; set; }

        ICollection<DungeonUser> OpenRequests { get; set; }

        DungeonUser CurrentDungeonMaster { get; set; }

        DungeonUser DungeonOwner { get; set; }

        Visibility Visibility { get; set; }

        #endregion

        #region Methods
        bool AddNeighborship(IRoom roomSource, Direction direction, IRoom roomSink);

        bool RemoveNeigborship(IRoom room1, IRoom room2);

        IRoom AddRoom(bool defaultRoom = false);

        bool RemoveRoom(IRoom room);

        IRoom GetRoom(Guid roomId);

        void ChangeRoom(IAvatar avatar, Direction direction);

        void EnterDungeon(IAvatar avatar);

        #endregion
    }
}
