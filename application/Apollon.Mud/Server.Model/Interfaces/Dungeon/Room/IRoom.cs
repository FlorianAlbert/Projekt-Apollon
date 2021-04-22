using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using System.Collections.Generic;
namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Room
{
    public interface IRoom : IApprovable
    {
        #region Properties
        string Description { get; set; }

        string Name { get; set; }

        ICollection<IInspectable> Inspectables { get; }

        IRoom NeighborNorth { get; set; }

        IRoom NeighborEast { get; set; }

        IRoom NeighborSouth { get; set; }

        IRoom NeighborWest { get; set; }

        ICollection<IRequestable> SpecialActions { get; }

        #endregion

        #region Methods
        string GetDescription(string objectName);

        void InspectRoom(IAvatar avatar);

        void Leave(IAvatar avatar);

        void TakeItem(IAvatar avatar, string itemName);

        void PlaceItem(ITakeable item);

        void EnterRoom(IAvatar avatar);

        bool SupportsSpecialAction(string action);

        #endregion
    }
}
