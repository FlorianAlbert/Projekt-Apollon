using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons
{
    /// <inheritdoc cref="IDungeon"/>
    public class Dungeon : IDungeon
    {
        /// <summary>
        /// Creates a new instance of Dungeon
        /// </summary>
        /// <param name="dungeonEpoch">The epoch of the new dungeon</param>
        /// <param name="dungeonDescription">The description of the new dungeon</param>
        public Dungeon(string dungeonEpoch, string dungeonDescription)
        {
            Id = Guid.NewGuid();

            dungeonEpoch = DungeonEpoch;
            dungeonDescription = DungeonDescription;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IDungeon.DungeonEpoch"/>
        public string DungeonEpoch { get; set; }

        /// <inheritdoc cref="IDungeon.DungeonDescription"/>
        public string DungeonDescription { get; set; }

        /// <inheritdoc cref="IDungeon.DefaultRoom"/>
        public IRoom DefaultRoom { get; set; }

        private ICollection<IRace> _ConfiguredRaces;

        /// <inheritdoc cref="IDungeon.ConfiguredRaces"/>
        public ICollection<IRace> ConfiguredRaces
        {
            get
            {
                return _ConfiguredRaces ??= new List<IRace>();
            }
        }

        private ICollection<IClass> _ConfiguredClasses;

        /// <inheritdoc cref="IDungeon.ConfiguredClasses"/>
        public ICollection<IClass> ConfiguredClasses
        {
            get
            {
                return _ConfiguredClasses ??= new List<IClass>();
            }
        }

        private ICollection<IRoom> _ConfiguredRooms;

        /// <inheritdoc cref="IDungeon.ConfiguredRooms"/>
        public ICollection<IRoom> ConfiguredRooms
        {
            get
            {
                return _ConfiguredRooms ??= new List<IRoom>();
            }
        }

        private ICollection<IInspectable> _ConfiguredInspectables;

        /// <inheritdoc cref="IDungeon.ConfiguredInspectables"/>
        public ICollection<IInspectable> ConfiguredInspectables
        {
            get
            {
                return _ConfiguredInspectables ??= new List<IInspectable>();
            }
        }

        private ICollection<IRequestable> _ConfiguredRequestable;

        /// <inheritdoc cref="IDungeon.ConfiguredRequestables"/>
        public ICollection<IRequestable> ConfiguredRequestables
        {
            get
            {
                return _ConfiguredRequestable ??= new List<IRequestable>();
            }
        }

        private ICollection<IAvatar> _ConfiguredAvatars;

        /// <inheritdoc cref="IDungeon.RegisteredAvatars"/>
        public ICollection<IAvatar> RegisteredAvatars
        {
            get
            {
                return _ConfiguredAvatars ??= new List<IAvatar>();
            }
        }

        private ICollection<DungeonUser> _DungeonMasters;

        /// <inheritdoc cref="IDungeon.DungeonMasters"/>
        public ICollection<DungeonUser> DungeonMasters
        {
            get
            {
                return _DungeonMasters ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _WhiteList;

        /// <inheritdoc cref="IDungeon.WhiteList"/>
        public ICollection<DungeonUser> WhiteList
        {
            get
            {
                return _WhiteList ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _BlackList;

        /// <inheritdoc cref="IDungeon.BlackList"/>
        public ICollection<DungeonUser> BlackList
        {
            get
            {
                return _BlackList ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _OpenRequests;

        /// <inheritdoc cref="IDungeon.OpenRequests"/>
        public ICollection<DungeonUser> OpenRequests
        {
            get
            {
                return _OpenRequests ??= new List<DungeonUser>();
            }
        }

        /// <inheritdoc cref="IDungeon.CurrentDungeonMaster"/>
        public DungeonUser CurrentDungeonMaster { get; set; }

        /// <inheritdoc cref="IDungeon.DungeonOwner"/>
        public DungeonUser DungeonOwner { get; set; }

        /// <inheritdoc cref="IDungeon.Visibility"/>
        public Visibility Visibility { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /// <inheritdoc cref="IDungeon.DungeonName"/>
        public string DungeonName { get; set; }

        /** TODO: In PlayerService zu verlagern
        public bool AddNeighborship(IRoom roomSource, Direction direction, IRoom roomSink)
        {
            throw new NotImplementedException();
        }

        public IRoom AddRoom(bool defaultRoom = false)
        {
            throw new NotImplementedException();
        }

        public void ChangeRoom(IAvatar avatar, Direction direction)
        {
            throw new NotImplementedException();
        }

        public void EnterDungeon(IAvatar avatar)
        {
            throw new NotImplementedException();
        }

        public IRoom GetRoom(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveNeigborship(IRoom room1, IRoom room2)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoom(IRoom room)
        {
            throw new NotImplementedException();
        } **/
    }
}
