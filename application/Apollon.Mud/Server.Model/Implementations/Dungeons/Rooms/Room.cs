using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Room
{
    /// <inheritdoc cref="IRoom"/>
    public class Room : IRoom
    {
        /// <summary>
        /// Creates a new instance Room
        /// </summary>
        /// <param name="description">Description of the new room</param>
        /// <param name="name">Name of the new room</param>
        public Room(string description, string name)
        {
            Id = Guid.NewGuid();

            Description = description;
            Name = name;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IRoom.Description"/>
        public string Description { get; set; }

        /// <inheritdoc cref="IRoom.Name"/>
        public string Name { get; set; }

        private ICollection<IInspectable> _Inspectables;

        /// <inheritdoc cref="IRoom.Inspectables"/>
        public ICollection<IInspectable> Inspectables
        {
            get
            {
                return _Inspectables ??= new List<IInspectable>();
            }
        }

        /// <inheritdoc cref="IRoom.NeighborNorth"/>
        public IRoom NeighborNorth { get; set; }

        /// <inheritdoc cref="IRoom.NeighborEast"/>
        public IRoom NeighborEast { get; set; }

        /// <inheritdoc cref="IRoom.NeighborSouth"/>
        public IRoom NeighborSouth { get; set; }

        /// <inheritdoc cref="IRoom.NeighborWest"/>
        public IRoom NeighborWest { get; set; }

        private ICollection<IRequestable> _SpecialActions;

        /// <inheritdoc cref="IRoom.SpecialActions"/>
        public ICollection<IRequestable> SpecialActions
        {
            get
            {
                return _SpecialActions ??= new List<IRequestable>();
            }
        }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /**
        public bool SupportsSpecialAction(string action)
        {
            return SpecialActions.Any(x => x.Message.NormalizeString() == action.NormalizeString());
        }

        public void EnterRoom(IAvatar avatar)
        {
            avatar.CurrentRoom = this;
            if (!Inspectables.Contains(avatar)) Inspectables.Add(avatar);
        }

        public string GetDescription(string objectName)
        {
            var inspectable = Inspectables.Where(x => x.Name.NormalizeString() == objectName.NormalizeString()).FirstOrDefault();
            return inspectable is not null ? inspectable.Description : $"Es befindet sich kein untersuchbares Objekt mit Namen {objectName} in diesem Raum.";
        }

        public string GetRoomDescription()
        {
            return Description;
        }

        public void Leave(IAvatar avatar)
        {
            Inspectables.Remove(avatar);
        }

        public void PlaceItem(ITakeable item)
        {
            Inspectables.Add(item);
        }

         TODO: Logik in PlayerService auslagern, damit Unterscheidung möglich zwischen existiert nicht versus kein takeable
        public ITakeable TakeItem(string itemName)
        {
            var inspectable = Inspectables.Where(x => x.Name.NormalizeString() == itemName.NormalizeString()).FirstOrDefault();
            
            if (inspectable is not null && inspectable is ITakeable takeable)
            {
                Inspectables.Remove(takeable);
                return takeable;
            }
            return null;
        }
        **/
    }
}
