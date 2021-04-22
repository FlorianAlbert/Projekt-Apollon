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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon
{
    public class Dungeon : IDungeon
    {
        public string DungeonEpoch { get ; set ; }
        public string DungeonDescription { get ; set ; }
        public IRoom DefaultRoom { get ; set ; }
        public ICollection<IRace> ConfiguredRaces { get ; set; }
        public ICollection<IClass> ConfiguredClasses { get; set; }
        public ICollection<IRoom> ConfiguredRooms { get; set; }
        public ICollection<IInspectable> ConfiguredInspectables { get; set; }
        public ICollection<IRequestable> ConfiguredRequestables { get; set; }
        public ICollection<IAvatar> RegisteredAvatars { get; set; }
        public ICollection<DungeonUser> DungeonMasters { get; set; }
        public ICollection<DungeonUser> WhiteList { get; set; }
        public ICollection<DungeonUser> BlackList { get; set; }
        public ICollection<DungeonUser> OpenRequests { get; set; }
        public DungeonUser CurrentDungeonMaster { get; set; }
        public DungeonUser DungeonOwner { get; set; }
        public Visibility Visibility { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }

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
        }
    }
}
