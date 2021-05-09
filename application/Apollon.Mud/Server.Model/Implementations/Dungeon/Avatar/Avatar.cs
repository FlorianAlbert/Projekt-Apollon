using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using Apollon.Mud.Server.Model.ModelExtensions;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar
{
    /// <inheritdoc cref="IAvatar"/>
    public class Avatar : IAvatar
    {
        private IInventory _Inventory;

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
        public Avatar(string name, IRace chosenRace, IClass chosenClass, Gender chosenGender, IDungeon dungeon, DungeonUser owner)
        {
            Id = Guid.NewGuid();
            Name = name;
            Race = chosenRace;
            Class = chosenClass;
            Gender = chosenGender;
            Dungeon = dungeon;
            Owner = owner;

            CurrentRoom = Dungeon.DefaultRoom;
            Status = Status.Pending;

            // CHECK: überprüfen ob Referenz oder Kopie genommen wird
            Inventory = chosenClass.StartInventory;
        }

        /// <inheritdoc cref="IAvatar.Race"/>
        public IRace Race { get; set; }

        /// <inheritdoc cref="IAvatar.Class"/>
        public IClass Class { get; set; }

        /// <inheritdoc cref="IAvatar.Gender"/>
        public Gender Gender { get; set; }

        /// <inheritdoc cref="IAvatar.Dungeon"/>
        public IDungeon Dungeon { get; set; }

        /// <inheritdoc cref="IAvatar.MaxHealth"/>
        public int MaxHealth 
        {
            get => Race.DefaultHealth + Class.DefaultHealth;
        }

        /// <inheritdoc cref="IAvatar.CurrentHealth"/>
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

        /// <inheritdoc cref="IAvatar.Damage"/>
        public int Damage 
        {
            get 
            {
                var result = Class.DefaultDamage + Race.DefaultDamage;

                if (HoldingItem != null && HoldingItem is IUsable weapon) result += weapon.DamageBoost;

                return result;
            }
        }

        /// <inheritdoc cref="IAvatar.Protection"/>
        public int Protection 
        { 
            get
            {
                var result = Race.DefaultProtection + Class.DefaultProtection;

                if (Armor != null) result += Armor.ProtectionBoost;

                return result;
            }
        }

        /// <inheritdoc cref="IAvatar.Inventory"/>
        public IInventory Inventory 
        { 
            get => _Inventory ??= new Inventory();
            init => _Inventory = value;
        }

        /// <inheritdoc cref="IAvatar.HoldingItem"/>
        public ITakeable HoldingItem { get; set; }

        /// <inheritdoc cref="IAvatar.Armor"/>
        public IWearable Armor { get; set; }

        /// <inheritdoc cref="IAvatar.CurrentRoom"/>
        public IRoom CurrentRoom { get; set; }

        /// <inheritdoc cref="IAvatar.Owner"/>
        public DungeonUser Owner { get; set; }

        /// <inheritdoc cref="IInspectable.Description"/>
        public string Description 
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

        /// <inheritdoc cref="IInspectable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

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
