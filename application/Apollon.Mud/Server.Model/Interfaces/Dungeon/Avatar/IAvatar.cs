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
    /// <summary>
    /// An avatar in a dungeon
    /// </summary>
    public interface IAvatar : IInspectable
    {
        #region Properties
        /// <summary>
        /// The race of the avatar
        /// </summary>
        IRace Race { get; set; }

        /// <summary>
        /// The class of the avatar
        /// </summary>
        IClass Class { get; set; }

        /// <summary>
        /// The gender of the avatar
        /// </summary>
        Gender Gender { get; set; }

        /// <summary>
        /// The dungeon the avatar is part of
        /// </summary>
        IDungeon Dungeon { get; set; }

        /// <summary>
        /// The maximum health value of the avatar
        /// </summary>
        int MaxHealth { get; }

        /// <summary>
        /// The actual health value of the avatar
        /// </summary>
        int CurrentHealth { get; set; }

        /// <summary>
        /// The damage value of the avatar
        /// </summary>
        int Damage { get; }

        /// <summary>
        /// The protection value of the avatar
        /// </summary>
        int Protection { get; }

        /// <summary>
        /// The inventory with everything the avatar is carrying
        /// </summary>
        IInventory Inventory { get; }

        /// <summary>
        /// The item the avatar is holding in his hand
        /// </summary>
        ITakeable HoldingItem { get; set; }

        /// <summary>
        /// The armor the avatar is wearing
        /// </summary>
        IWearable Armor { get; set; }

        /// <summary>
        /// The room the avatar is in
        /// </summary>
        IRoom CurrentRoom { get; set; }

        /// <summary>
        /// The user the avatar belongs to
        /// </summary>
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
