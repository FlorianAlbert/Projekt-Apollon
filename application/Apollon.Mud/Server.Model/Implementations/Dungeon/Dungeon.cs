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

namespace Apollon.Mud.Server.Model.Implementations.Dungeon
{
    public class Dungeon : IDungeon
    {
        public Dungeon(string dungeonEpoch, string dungeonDescription)
        {
            Id = Guid.NewGuid();

            dungeonEpoch = DungeonEpoch;
            dungeonDescription = DungeonDescription;

            Status = Status.Pending;
        }
        public string DungeonEpoch { get; set; }
        public string DungeonDescription { get; set; }
        public IRoom DefaultRoom { get; set; }
        private ICollection<IRace> _ConfiguredRaces;
        public ICollection<IRace> ConfiguredRaces
        {
            get
            {
                return _ConfiguredRaces ??= new List<IRace>();
            }
        }
        private ICollection<IClass> _ConfiguredClasses;
        public ICollection<IClass> ConfiguredClasses
        {
            get
            {
                return _ConfiguredClasses ??= new List<IClass>();
            }
        }
        private ICollection<IRoom> _ConfiguredRooms;
        public ICollection<IRoom> ConfiguredRooms
        {
            get
            {
                return _ConfiguredRooms ??= new List<IRoom>();
            }
        }
        private ICollection<IInspectable> _ConfiguredInspectables;
        public ICollection<IInspectable> ConfiguredInspectables
        {
            get
            {
                return _ConfiguredInspectables ??= new List<IInspectable>();
            }
        }
        private ICollection<IRequestable> _ConfiguredRequestable;
        public ICollection<IRequestable> ConfiguredRequestables
        {
            get
            {
                return _ConfiguredRequestable ??= new List<IRequestable>();
            }
        }
        private ICollection<IAvatar> _ConfiguredAvatars;
        public ICollection<IAvatar> RegisteredAvatars
        {
            get
            {
                return _ConfiguredAvatars ??= new List<IAvatar>();
            }
        }
        private ICollection<DungeonUser> _DungeonMasters;
        public ICollection<DungeonUser> DungeonMasters
        {
            get
            {
                return _DungeonMasters ??= new List<DungeonUser>();
            }
        }
        private ICollection<DungeonUser> _WhiteList;
        public ICollection<DungeonUser> WhiteList
        {
            get
            {
                return _WhiteList ??= new List<DungeonUser>();
            }
        }
        private ICollection<DungeonUser> _BlackList;
        public ICollection<DungeonUser> BlackList
        {
            get
            {
                return _BlackList ??= new List<DungeonUser>();
            }
        }
        private ICollection<DungeonUser> _OpenRequests;
        public ICollection<DungeonUser> OpenRequests
        {
            get
            {
                return _OpenRequests ??= new List<DungeonUser>();
            }
        }
        public DungeonUser CurrentDungeonMaster { get; set; }
        public DungeonUser DungeonOwner { get; set; }
        public Visibility Visibility { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
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
