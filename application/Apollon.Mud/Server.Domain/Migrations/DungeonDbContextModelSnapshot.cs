﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Domain.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Apollon.Mud.Server.Domain.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(DungeonDbContext))]
    partial class DungeonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ArmorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChosenClassId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ChosenGender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChosenRaceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentHealth")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CurrentRoomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoldingItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ArmorId");

                    b.HasIndex("ChosenClassId");

                    b.HasIndex("ChosenRaceId");

                    b.HasIndex("CurrentRoomId");

                    b.HasIndex("DungeonId");

                    b.HasIndex("HoldingItemId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("DefaultDamage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultHealth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultProtection")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentDungeonMasterId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultRoomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonEpoch")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonOwnerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Visibility")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CurrentDungeonMasterId");

                    b.HasIndex("DefaultRoomId")
                        .IsUnique();

                    b.HasIndex("DungeonOwnerId");

                    b.ToTable("Dungeons");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.ToTable("Inspectables");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Inspectable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("DefaultDamage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultHealth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultProtection")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables.Requestable", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageRegex")
                        .HasColumnType("TEXT");

                    b.Property<string>("PatternForPlayer")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.ToTable("SpecialActions");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NeighborNorthId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NeighborWestId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.HasIndex("NeighborNorthId")
                        .IsUnique();

                    b.HasIndex("NeighborWestId")
                        .IsUnique();

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AvatarTakeable", b =>
                {
                    b.Property<string>("InventoryAvatarsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("InventoryId")
                        .HasColumnType("TEXT");

                    b.HasKey("InventoryAvatarsId", "InventoryId");

                    b.HasIndex("InventoryId");

                    b.ToTable("AvatarTakeable");
                });

            modelBuilder.Entity("ClassTakeable", b =>
                {
                    b.Property<string>("ClassesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartInventoryId")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassesId", "StartInventoryId");

                    b.HasIndex("StartInventoryId");

                    b.ToTable("ClassTakeable");
                });

            modelBuilder.Entity("DungeonDungeonUser", b =>
                {
                    b.Property<string>("BlackListDungeonsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("BlackListId")
                        .HasColumnType("TEXT");

                    b.HasKey("BlackListDungeonsId", "BlackListId");

                    b.HasIndex("BlackListId");

                    b.ToTable("DungeonDungeonUser");
                });

            modelBuilder.Entity("DungeonDungeonUser1", b =>
                {
                    b.Property<string>("WhiteListDungeonsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("WhiteListId")
                        .HasColumnType("TEXT");

                    b.HasKey("WhiteListDungeonsId", "WhiteListId");

                    b.HasIndex("WhiteListId");

                    b.ToTable("DungeonDungeonUser1");
                });

            modelBuilder.Entity("DungeonDungeonUser2", b =>
                {
                    b.Property<string>("DungeonMasterDungeonsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DungeonMastersId")
                        .HasColumnType("TEXT");

                    b.HasKey("DungeonMasterDungeonsId", "DungeonMastersId");

                    b.HasIndex("DungeonMastersId");

                    b.ToTable("DungeonDungeonUser2");
                });

            modelBuilder.Entity("DungeonDungeonUser3", b =>
                {
                    b.Property<string>("OpenRequestDungeonsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OpenRequestsId")
                        .HasColumnType("TEXT");

                    b.HasKey("OpenRequestDungeonsId", "OpenRequestsId");

                    b.HasIndex("OpenRequestsId");

                    b.ToTable("DungeonDungeonUser3");
                });

            modelBuilder.Entity("InspectableRoom", b =>
                {
                    b.Property<string>("InspectablesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomsId")
                        .HasColumnType("TEXT");

                    b.HasKey("InspectablesId", "RoomsId");

                    b.HasIndex("RoomsId");

                    b.ToTable("InspectableRoom");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RequestableRoom", b =>
                {
                    b.Property<string>("RoomsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SpecialActionsId")
                        .HasColumnType("TEXT");

                    b.HasKey("RoomsId", "SpecialActionsId");

                    b.HasIndex("SpecialActionsId");

                    b.ToTable("RequestableRoom");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable");

                    b.Property<int>("Weight")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Takeable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs.Npc", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Npc");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables.Consumable", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable");

                    b.Property<string>("EffectDescription")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Consumable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables.Usable", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable");

                    b.Property<int>("DamageBoost")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Usable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables.Wearable", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable");

                    b.Property<int>("ProtectionBoost")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Wearable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables.Wearable", "Armor")
                        .WithMany("ArmorAvatars")
                        .HasForeignKey("ArmorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", "ChosenClass")
                        .WithMany("Avatars")
                        .HasForeignKey("ChosenClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", "ChosenRace")
                        .WithMany("Avatars")
                        .HasForeignKey("ChosenRaceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "CurrentRoom")
                        .WithMany("Avatars")
                        .HasForeignKey("CurrentRoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("RegisteredAvatars")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", "HoldingItem")
                        .WithMany("HoldingItemAvatars")
                        .HasForeignKey("HoldingItemId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "Owner")
                        .WithMany("Avatars")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Armor");

                    b.Navigation("ChosenClass");

                    b.Navigation("ChosenRace");

                    b.Navigation("CurrentRoom");

                    b.Navigation("Dungeon");

                    b.Navigation("HoldingItem");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("ConfiguredClasses")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dungeon");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "CurrentDungeonMaster")
                        .WithMany("CurrentDungeonMasterDungeons")
                        .HasForeignKey("CurrentDungeonMasterId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "DefaultRoom")
                        .WithOne()
                        .HasForeignKey("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "DefaultRoomId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "DungeonOwner")
                        .WithMany("DungeonOwnerDungeons")
                        .HasForeignKey("DungeonOwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CurrentDungeonMaster");

                    b.Navigation("DefaultRoom");

                    b.Navigation("DungeonOwner");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("ConfiguredInspectables")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dungeon");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("ConfiguredRaces")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dungeon");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables.Requestable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("ConfiguredRequestables")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dungeon");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("ConfiguredRooms")
                        .HasForeignKey("DungeonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborNorth")
                        .WithOne("NeighborSouth")
                        .HasForeignKey("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborNorthId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborWest")
                        .WithOne("NeighborEast")
                        .HasForeignKey("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborWestId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Dungeon");

                    b.Navigation("NeighborNorth");

                    b.Navigation("NeighborWest");
                });

            modelBuilder.Entity("AvatarTakeable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", null)
                        .WithMany()
                        .HasForeignKey("InventoryAvatarsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", null)
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClassTakeable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", null)
                        .WithMany()
                        .HasForeignKey("ClassesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", null)
                        .WithMany()
                        .HasForeignKey("StartInventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DungeonDungeonUser", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany()
                        .HasForeignKey("BlackListDungeonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("BlackListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DungeonDungeonUser1", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany()
                        .HasForeignKey("WhiteListDungeonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("WhiteListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DungeonDungeonUser2", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany()
                        .HasForeignKey("DungeonMasterDungeonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("DungeonMastersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DungeonDungeonUser3", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany()
                        .HasForeignKey("OpenRequestDungeonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("OpenRequestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InspectableRoom", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable", null)
                        .WithMany()
                        .HasForeignKey("InspectablesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RequestableRoom", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables.Requestable", null)
                        .WithMany()
                        .HasForeignKey("SpecialActionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.Navigation("Avatars");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", b =>
                {
                    b.Navigation("ConfiguredClasses");

                    b.Navigation("ConfiguredInspectables");

                    b.Navigation("ConfiguredRaces");

                    b.Navigation("ConfiguredRequestables");

                    b.Navigation("ConfiguredRooms");

                    b.Navigation("RegisteredAvatars");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", b =>
                {
                    b.Navigation("Avatars");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.Navigation("Avatars");

                    b.Navigation("NeighborEast");

                    b.Navigation("NeighborSouth");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", b =>
                {
                    b.Navigation("Avatars");

                    b.Navigation("CurrentDungeonMasterDungeons");

                    b.Navigation("DungeonOwnerDungeons");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", b =>
                {
                    b.Navigation("HoldingItemAvatars");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables.Wearable", b =>
                {
                    b.Navigation("ArmorAvatars");
                });
#pragma warning restore 612, 618
        }
    }
}
