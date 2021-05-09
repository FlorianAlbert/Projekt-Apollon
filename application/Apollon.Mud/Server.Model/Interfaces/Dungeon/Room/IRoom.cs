using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using System.Collections.Generic;
namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Room
{
    /// <summary>
    /// Describes a room in a dungeon
    /// </summary>
    public interface IRoom : IApprovable
    {
        #region Properties
        /// <summary>
        /// The description of the whole room
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The name of the room
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// All inspectables that are currently in this room
        /// </summary>
        ICollection<IInspectable> Inspectables { get; }

        /// <summary>
        /// The room that is in the north
        /// </summary>
        IRoom NeighborNorth { get; set; }

        /// <summary>
        /// The room that is in the east
        /// </summary>
        IRoom NeighborEast { get; set; }

        /// <summary>
        /// The room that is in the south
        /// </summary>
        IRoom NeighborSouth { get; set; }

        /// <summary>
        /// The room that is in the west
        /// </summary>
        IRoom NeighborWest { get; set; }

        /// <summary>
        /// All special actions an avatar can do in this room
        /// </summary>
        ICollection<IRequestable> SpecialActions { get; }

        #endregion

        /** TODO: Nach PlayerService/KonfigurationsServices verlagern
         * #region Methods
        string GetDescription(string objectName);

        string GetRoomDescription();

        void Leave(IAvatar avatar);

        void TakeItem(IAvatar avatar, string itemName);

        void PlaceItem(ITakeable item);

        void EnterRoom(IAvatar avatar);

        bool SupportsSpecialAction(string action);

        #endregion **/
    }
}
