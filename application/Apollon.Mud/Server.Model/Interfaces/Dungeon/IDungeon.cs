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
    /// <summary>
    /// Describes a dungeon
    /// </summary>
    public interface IDungeon : IApprovable
    {
        #region Properties
        /// <summary>
        /// Name of the dungeon
        /// </summary>
        string DungeonName { get; set; }

        /// <summary>
        /// Epoch of the dungeon
        /// </summary>
        string DungeonEpoch { get; set; }

        /// <summary>
        /// Description of the dungeon
        /// </summary>
        string DungeonDescription { get; set; }

        /// <summary>
        /// Start room of the dungeon
        /// </summary>
        IRoom DefaultRoom { get; set; }

        /// <summary>
        /// All races an avatar can choose between
        /// </summary>
        ICollection<IRace> ConfiguredRaces { get; }

        /// <summary>
        /// All classes an avatar can choose between
        /// </summary>
        ICollection<IClass> ConfiguredClasses { get; }

        /// <summary>
        /// All rooms of the dungeon
        /// </summary>
        ICollection<IRoom> ConfiguredRooms { get; }

        /// <summary>
        /// All inspectables that can be placed in the rooms
        /// </summary>
        ICollection<IInspectable> ConfiguredInspectables { get; }

        /// <summary>
        /// All requestables that can be attached to a room
        /// </summary>
        ICollection<IRequestable> ConfiguredRequestables { get; }

        /// <summary>
        /// All registered avatar
        /// </summary>
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
