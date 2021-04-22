using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using Apollon.Mud.Server.Model.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Room
{
    public class Room : IRoom
    {
        public Room(string description, string name)
        {
            Id = Guid.NewGuid();

            description = Description;
            name = Name;

            Status = Status.Pending;
        }
        public string Description { get; set; }
        public string Name { get; set; }
        private ICollection<IInspectable> _Inspectables;
        public ICollection<IInspectable> Inspectables
        {
            get
            {
                return _Inspectables ??= new List<IInspectable>();
            }
        }
        public IRoom NeighborNorth { get; set; }
        public IRoom NeighborEast { get; set; }
        public IRoom NeighborSouth { get; set; }
        public IRoom NeighborWest { get; set; }
        private ICollection<IRequestable> _SpecialActions;
        public ICollection<IRequestable> SpecialActions
        {
            get
            {
                return _SpecialActions ??= new List<IRequestable>();
            }
        }

        public Guid Id { get; }

        public Status Status { get; set; }

        public bool SupportsSpecialAction(string action)
        {
            return SpecialActions.Any(x => x.Message.NormalizeString() == action.NormalizeString());
        }

        public void EnterRoom(IAvatar avatar)
        {
            avatar.CurrentRoom = this;
            Inspectables.Add(avatar);
        }

        public string GetDescription(string objectName)
        {
            var inspectable = Inspectables.Where(x => x.Name.NormalizeString() == objectName.NormalizeString()).FirstOrDefault();
            return inspectable is not null ? inspectable.Description : $"Es befindet sich kein untersuchbares Objekt mit Namen {objectName} in diesem Raum.";
        }

        public void InspectRoom(IAvatar avatar)
        {
            ;
        }

        public void Leave(IAvatar avatar)
        {
            ;
        }

        public void PlaceItem(ITakeable item)
        {
            ;
        }

        public void TakeItem(IAvatar avatar, string itemName)
        {
            ;
        }
    }
}
