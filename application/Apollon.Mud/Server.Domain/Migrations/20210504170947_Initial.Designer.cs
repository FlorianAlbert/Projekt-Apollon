﻿// <auto-generated />
using System;
using Apollon.Mud.Server.Domain.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Apollon.Mud.Server.Domain.Migrations
{
    [DbContext(typeof(DungeonDbContext))]
    [Migration("20210504170947_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid?>("DungeonId")
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentDungeonMasterId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DefaultRoomId")
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

                    b.HasIndex("DefaultRoomId");

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

                    b.Property<Guid?>("DungeonId1")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId1");

                    b.ToTable("Inspectables");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Inspectable");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid?>("DungeonId")
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageRegex")
                        .HasColumnType("TEXT");

                    b.Property<string>("PatternForPlayer")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DungeonId");

                    b.HasIndex("RoomId");

                    b.ToTable("SpecialActions");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("NeighborNorthId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("NeighborWestId")
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

                    b.Property<Guid?>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId1")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId2")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId3")
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

                    b.HasIndex("DungeonId");

                    b.HasIndex("DungeonId1");

                    b.HasIndex("DungeonId2");

                    b.HasIndex("DungeonId3");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
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

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable");

                    b.Property<string>("ArmorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ChosenClassId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ChosenGender")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ChosenRaceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentHealth")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("CurrentRoomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DungeonId")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoldingItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.HasIndex("ArmorId");

                    b.HasIndex("ChosenClassId");

                    b.HasIndex("ChosenRaceId");

                    b.HasIndex("CurrentRoomId");

                    b.HasIndex("DungeonId");

                    b.HasIndex("HoldingItemId");

                    b.HasIndex("OwnerId");

                    b.HasDiscriminator().HasValue("Avatar");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", b =>
                {
                    b.HasBaseType("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable");

                    b.Property<string>("AvatarId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Weight")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AvatarId");

                    b.HasIndex("ClassId");

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

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("ConfiguredClasses")
                        .HasForeignKey("DungeonId");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "CurrentDungeonMaster")
                        .WithMany()
                        .HasForeignKey("CurrentDungeonMasterId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "DefaultRoom")
                        .WithMany()
                        .HasForeignKey("DefaultRoomId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "DungeonOwner")
                        .WithMany()
                        .HasForeignKey("DungeonOwnerId");

                    b.Navigation("CurrentDungeonMaster");

                    b.Navigation("DefaultRoom");

                    b.Navigation("DungeonOwner");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Inspectable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("ConfiguredInspectables")
                        .HasForeignKey("DungeonId1");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("ConfiguredRaces")
                        .HasForeignKey("DungeonId");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables.Requestable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("ConfiguredRequestables")
                        .HasForeignKey("DungeonId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", null)
                        .WithMany("SpecialActions")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("ConfiguredRooms")
                        .HasForeignKey("DungeonId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborNorth")
                        .WithOne("NeighborSouth")
                        .HasForeignKey("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborNorthId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborWest")
                        .WithOne("NeighborEast")
                        .HasForeignKey("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "NeighborWestId");

                    b.Navigation("NeighborNorth");

                    b.Navigation("NeighborWest");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("BlackList")
                        .HasForeignKey("DungeonId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("DungeonMasters")
                        .HasForeignKey("DungeonId1");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("OpenRequests")
                        .HasForeignKey("DungeonId2");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", null)
                        .WithMany("WhiteList")
                        .HasForeignKey("DungeonId3");
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

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables.Wearable", "Armor")
                        .WithMany()
                        .HasForeignKey("ArmorId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", "ChosenClass")
                        .WithMany()
                        .HasForeignKey("ChosenClassId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Races.Race", "ChosenRace")
                        .WithMany()
                        .HasForeignKey("ChosenRaceId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", "CurrentRoom")
                        .WithMany("Inspectables")
                        .HasForeignKey("CurrentRoomId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", "Dungeon")
                        .WithMany("RegisteredAvatars")
                        .HasForeignKey("DungeonId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", "HoldingItem")
                        .WithMany()
                        .HasForeignKey("HoldingItemId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.User.DungeonUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Armor");

                    b.Navigation("ChosenClass");

                    b.Navigation("ChosenRace");

                    b.Navigation("CurrentRoom");

                    b.Navigation("Dungeon");

                    b.Navigation("HoldingItem");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Takeable", b =>
                {
                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", null)
                        .WithMany("Inventory")
                        .HasForeignKey("AvatarId");

                    b.HasOne("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", null)
                        .WithMany("StartInventory")
                        .HasForeignKey("ClassId");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Classes.Class", b =>
                {
                    b.Navigation("StartInventory");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Dungeon", b =>
                {
                    b.Navigation("BlackList");

                    b.Navigation("ConfiguredClasses");

                    b.Navigation("ConfiguredInspectables");

                    b.Navigation("ConfiguredRaces");

                    b.Navigation("ConfiguredRequestables");

                    b.Navigation("ConfiguredRooms");

                    b.Navigation("DungeonMasters");

                    b.Navigation("OpenRequests");

                    b.Navigation("RegisteredAvatars");

                    b.Navigation("WhiteList");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms.Room", b =>
                {
                    b.Navigation("Inspectables");

                    b.Navigation("NeighborEast");

                    b.Navigation("NeighborSouth");

                    b.Navigation("SpecialActions");
                });

            modelBuilder.Entity("Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars.Avatar", b =>
                {
                    b.Navigation("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}