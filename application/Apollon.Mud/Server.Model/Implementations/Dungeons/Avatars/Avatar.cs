using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.ModelExtensions;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars
{
    /// <summary>
    /// An avatar in a dungeon
    /// </summary>
    public class Avatar : Inspectable
    {
        private Inventory _Inventory;

        private int _HealthDifference = 0;

        /// <summary>
        /// Creates a new instance of Avatar
        /// </summary>
        /// <param name="name">Name of the new avatar</param>
        /// <param name="chosenRace">Race of the new avatar</param>
        /// <param name="chosenClass">Class of the new avatar</param>
        /// <param name="chosenGender">Gender of the new avatar</param>
        /// <param name="dungeon">Dungeon the new avatar is part of</param>
        /// <param name="owner">Owner of the new avatar</param>
        public Avatar(string name, Race chosenRace, Class chosenClass, Gender chosenGender, Dungeon dungeon, DungeonUser owner)
            : base("",name)
        {
            Race = chosenRace;
            Class = chosenClass;
            Gender = chosenGender;
            Dungeon = dungeon;
            Owner = owner;

            CurrentRoom = Dungeon.DefaultRoom;

            // CHECK: überprüfen ob Referenz oder Kopie genommen wird
            Inventory = chosenClass.StartInventory;
        }

        /// <summary>
        /// The race of the avatar
        /// </summary>
        Race Race { get; set; }

        /// <summary>
        /// The class of the avatar
        /// </summary>
        Class Class { get; set; }

        /// <summary>
        /// The gender of the avatar
        /// </summary>
        Gender Gender { get; set; }

        /// <summary>
        /// The dungeon the avatar is part of
        /// </summary>
        Dungeon Dungeon { get; set; }

        /// <summary>
        /// The maximum health value of the avatar
        /// </summary>
        public int MaxHealth 
        {
            get => Race.DefaultHealth + Class.DefaultHealth;
        }

        /// <summary>
        /// The actual health value of the avatar
        /// </summary>
        public int CurrentHealth 
        {
            get => MaxHealth - _HealthDifference;
            set
            {
                if (value <= 0) _HealthDifference = MaxHealth;
                else if (value >= MaxHealth) _HealthDifference = 0;
                else _HealthDifference = MaxHealth - value;
            }
        }

        /// <summary>
        /// The damage value of the avatar
        /// </summary>
        public int Damage 
        {
            get 
            {
                var result = Class.DefaultDamage + Race.DefaultDamage;

                if (HoldingItem != null && HoldingItem is Usable weapon) result += weapon.DamageBoost;

                return result;
            }
        }

        /// <summary>
        /// The protection value of the avatar
        /// </summary>
        public int Protection 
        { 
            get
            {
                var result = Race.DefaultProtection + Class.DefaultProtection;

                if (Armor != null) result += Armor.ProtectionBoost;

                return result;
            }
        }

        /// <summary>
        /// The inventory with everything the avatar is carrying
        /// </summary>
        public Inventory Inventory 
        { 
            get => _Inventory ??= new Inventory();
            init => _Inventory = value;
        }

        /// <summary>
        /// The item the avatar is holding in his hand
        /// </summary>
        public Takeable HoldingItem { get; set; }

        /// <summary>
        /// The armor the avatar is wearing
        /// </summary>
        public Wearable Armor { get; set; }

        /// <summary>
        /// The room the avatar is in
        /// </summary>
        public Room CurrentRoom { get; set; }

        /// <summary>
        /// The user the avatar belongs to
        /// </summary>
        public DungeonUser Owner { get; set; }

        /// <inheritdoc cref="Inspectable.Description"/>
        public override string Description 
        {
            get => $"{ Name } ist von der Rasse { Race.Name } vom Geschlecht { Gender.GetGermanGender() }.\n" +
                "Diese Rasse zeichnet sich durch folgende Beschreibung aus: \n" +
                $"{ Race.Description } \n\n" +
                $"Außerdem hat { Name } die Klasse { Class.Name }, welche sich durch folgende Beschreibung auszeichnet: \n" +
                $"{ Class.Description }";
            set 
            { 
            }
        }

        /**
        public bool AddItemToInventory(ITakeable item)
        {
            if (item != null) Inventory.Add(item);
            return Inventory.Contains(item);
        }

        public string ConsumeItem(string itemName)
        {
            var takeableItem = Inventory.FirstOrDefault(takeable => takeable.Name == itemName);

            if (takeableItem == null) return "Dieses Item befindet sich nicht in deinem Inventar.";

            if (takeableItem is not IConsumable item) return "Dieses Item kannst du nicht konsumieren.";

            Inventory.Remove(item);

            return item.EffectDescription;
        }

        // TODO: diskutieren ob überhaupt noch nötig
        public void SendPrivateMessage(string message)
        {
            throw new NotImplementedException();
        }

        public ITakeable ThrowAway(string itemName)
        {
            var item = Inventory.FirstOrDefault(takeable => takeable.Name == itemName);

            if (item != null) Inventory.Remove(item);

            return item;
        } **/
    }
}
