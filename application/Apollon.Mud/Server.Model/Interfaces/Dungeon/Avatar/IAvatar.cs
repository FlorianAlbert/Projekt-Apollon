using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar
{
    public interface IAvatar : IInspectable
    {
        #region Properties
        IRace Race { get; set; }

        IClass Class { get; set; }

        Gender Gender { get; set; }

        IDungeon Dungeon { get; set; }

        int MaxHealth { get; }

        int CurrentHealth { get; }

        int Damage { get; }

        int Protection { get; }

        ICollection<ITakeable> Inventory { get; }

        ITakeable HoldingItem { get; set; }

        IWearable Armor { get; set; }

        IRoom CurrentRoom { get; set; }

        DungeonUser Owner { get; set; }

        #endregion

        #region Methods
        void SendPrivateMessage(string message);

        string ConsumeItem(string itemName);

        bool AddItemToInventory(ITakeable item);

        ITakeable ThrowAway(string itemName);

        #endregion
    }
}
