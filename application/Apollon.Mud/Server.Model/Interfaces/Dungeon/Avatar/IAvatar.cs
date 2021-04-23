using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;

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

        IInventory Inventory { get; }

        ITakeable HoldingItem { get; set; }

        IWearable Armor { get; set; }

        IRoom CurrentRoom { get; set; }

        DungeonUser Owner { get; set; }

        #endregion

        /** TODO: Nach PlayerService/KonfigurationsServices verlagern 
         * #region Methods
        void SendPrivateMessage(string message);

        string ConsumeItem(string itemName);

        bool AddItemToInventory(ITakeable item);

        ITakeable ThrowAway(string itemName);

        #endregion **/
    }
}
