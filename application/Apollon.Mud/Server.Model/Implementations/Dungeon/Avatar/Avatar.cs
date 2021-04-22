using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Room;
using Apollon.Mud.Server.Model.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar
{
    public class Avatar : IAvatar
    {
        private ICollection<ITakeable> _Inventory;

        private int _HealthDifference = 0;

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

        public IRace Race { get; set; }
        public IClass Class { get; set; }
        public Gender Gender { get; set; }
        public IDungeon Dungeon { get; set; }
        public int MaxHealth 
        {
            get => Race.DefaultHealth + Class.DefaultHealth;
        }
        public int CurrentHealth 
        {
            get => MaxHealth - _HealthDifference;
        }
        public int Damage 
        {
            get 
            {
                var result = Class.DefaultDamage + Race.DefaultDamage;

                if (HoldingItem != null && HoldingItem is IUsable weapon) result += weapon.DamageBoost;

                return result;
            }
        }
        public int Protection 
        { 
            get
            {
                var result = Race.DefaultProtection + Class.DefaultProtection;

                if (Armor != null) result += Armor.ProtectionBoost;

                return result;
            }
        }
        public ICollection<ITakeable> Inventory 
        { 
            get => _Inventory ??= new Inventory(); init;
        }
        public ITakeable HoldingItem { get; set; }
        public IWearable Armor { get; set; }
        public IRoom CurrentRoom { get; set; }
        public DungeonUser Owner { get; set; }
        public string Description 
        {
            get => $"{ Name } ist von der Rasse { Race.Name } vom Geschlecht { Gender.GetGermanGender() }.\n" +
                $"Diese Rasse zeichnet sich durch folgende Beschreibung aus: \n" +
                $"{ Race.Description } \n\n" +
                $"Außerdem hat { Name } die Klasse { Class.Name }, welche sich durch folgende Beschreibung auszeichnet: \n" +
                $"{ Class.Description }";
            set 
            { 
            }
        }
        public string Name { get; set; }
        public Guid Id { get; }
        public Status Status { get; set; }

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
        }
    }
}
