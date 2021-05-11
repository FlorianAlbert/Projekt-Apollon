using System;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.ModelExtensions;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars
{
    /// <summary>
    /// An avatar in a dungeon
    /// </summary>
    public class Avatar : IApprovable
    {
        private Inventory _Inventory;

        private int _HealthDifference;

        public Avatar()
        {
            
        }

        /// <summary>
        /// Creates a new instance of Avatar
        /// </summary>
        /// <param name="name">Name of the new avatar</param>
        /// <param name="chosenRace">Race of the new avatar</param>
        /// <param name="chosenClass">Class of the new avatar</param>
        /// <param name="chosenGender">Gender of the new avatar</param>
        /// <param name="dungeon">Dungeon the new avatar is part of</param>
        /// <param name="owner">Owner of the new avatar</param>
        public Avatar(string name, Race chosenRace, Class chosenClass, Gender chosenGender)
        {
            Id = Guid.NewGuid();
            Status = Status.Pending;

            Name = name;

            ChosenRace = chosenRace;
            ChosenClass = chosenClass;
            ChosenGender = chosenGender;

            MaxHealth = ChosenRace.DefaultHealth + ChosenClass.DefaultHealth;

            // CHECK: überprüfen ob Referenz oder Kopie genommen wird
            Inventory = chosenClass.StartInventory;
        }

        /// <summary>
        /// Name of the Avatar
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string Name { get; set; }

        /// <summary>
        /// Foreign Key for dungeon
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Dungeon Dungeon { get; set; }

        /// <summary>
        /// The race of the avatar
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Race ChosenRace { get; }

        /// <summary>
        /// The class of the avatar
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Class ChosenClass { get; }

        /// <summary>
        /// The gender of the avatar
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Gender ChosenGender { get; set; }

        /// <summary>
        /// The maximum health value of the avatar
        /// </summary>
        [ExcludeFromCodeCoverage]
        public int MaxHealth { get; }

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
                var result = ChosenClass.DefaultDamage + ChosenRace.DefaultDamage;

                if (HoldingItem is Usable usable) result += usable.DamageBoost;

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
                var result = ChosenRace.DefaultProtection + ChosenClass.DefaultProtection;

                if (Armor != null) result += Armor.ProtectionBoost;

                return result;
            }
        }

        /// <summary>
        /// The inventory with everything the avatar is carrying
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Inventory Inventory 
        { 
            get => _Inventory ??= new Inventory();
            init => _Inventory = value;
        }

        /// <summary>
        /// The item the avatar is holding in his hand
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Takeable HoldingItem { get; set; }

        /// <summary>
        /// The armor the avatar is wearing
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Wearable Armor { get; set; }

        /// <summary>
        /// The room the avatar is in
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual Room CurrentRoom { get; set; }

        /// <summary>
        /// The user the avatar belongs to
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual DungeonUser Owner { get; set; }

        /// <inheritdoc cref="Inspectable.Description"/>
        public string Description => $"{ Name } ist von der Rasse { ChosenRace.Name } vom Geschlecht { ChosenGender.GetGermanGender() }.\n" +
                "Diese Rasse zeichnet sich durch folgende Beschreibung aus: \n" +
                $"{ ChosenRace.Description } \n\n" +
                $"Außerdem hat { Name } die Klasse { ChosenClass.Name }, welche sich durch folgende Beschreibung auszeichnet: \n" +
                $"{ ChosenClass.Description }";

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }
    }
}
