using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollon.Mud.Server.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true),
                    ChosenRaceId = table.Column<string>(type: "TEXT", nullable: true),
                    ChosenClassId = table.Column<string>(type: "TEXT", nullable: true),
                    ChosenGender = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    HoldingItemId = table.Column<string>(type: "TEXT", nullable: true),
                    ArmorId = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentRoomId = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatars_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonDungeonUser",
                columns: table => new
                {
                    BlackListDungeonsId = table.Column<string>(type: "TEXT", nullable: false),
                    BlackListId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonDungeonUser", x => new { x.BlackListDungeonsId, x.BlackListId });
                    table.ForeignKey(
                        name: "FK_DungeonDungeonUser_AspNetUsers_BlackListId",
                        column: x => x.BlackListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonDungeonUser1",
                columns: table => new
                {
                    WhiteListDungeonsId = table.Column<string>(type: "TEXT", nullable: false),
                    WhiteListId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonDungeonUser1", x => new { x.WhiteListDungeonsId, x.WhiteListId });
                    table.ForeignKey(
                        name: "FK_DungeonDungeonUser1_AspNetUsers_WhiteListId",
                        column: x => x.WhiteListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonDungeonUser2",
                columns: table => new
                {
                    DungeonMasterDungeonsId = table.Column<string>(type: "TEXT", nullable: false),
                    DungeonMastersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonDungeonUser2", x => new { x.DungeonMasterDungeonsId, x.DungeonMastersId });
                    table.ForeignKey(
                        name: "FK_DungeonDungeonUser2_AspNetUsers_DungeonMastersId",
                        column: x => x.DungeonMastersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonDungeonUser3",
                columns: table => new
                {
                    OpenRequestDungeonsId = table.Column<string>(type: "TEXT", nullable: false),
                    OpenRequestsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonDungeonUser3", x => new { x.OpenRequestDungeonsId, x.OpenRequestsId });
                    table.ForeignKey(
                        name: "FK_DungeonDungeonUser3_AspNetUsers_OpenRequestsId",
                        column: x => x.OpenRequestsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DungeonEpoch = table.Column<string>(type: "TEXT", nullable: true),
                    DungeonDescription = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultRoomId = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentDungeonMasterId = table.Column<string>(type: "TEXT", nullable: true),
                    DungeonOwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonName = table.Column<string>(type: "TEXT", nullable: true),
                    LastActive = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dungeons_AspNetUsers_CurrentDungeonMasterId",
                        column: x => x.CurrentDungeonMasterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Dungeons_AspNetUsers_DungeonOwnerId",
                        column: x => x.DungeonOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultProtection = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultDamage = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspectables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: true),
                    EffectDescription = table.Column<string>(type: "TEXT", nullable: true),
                    DamageBoost = table.Column<int>(type: "INTEGER", nullable: true),
                    ProtectionBoost = table.Column<int>(type: "INTEGER", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspectables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspectables_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultProtection = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultDamage = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NeighborNorthId = table.Column<string>(type: "TEXT", nullable: true),
                    NeighborWestId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_NeighborNorthId",
                        column: x => x.NeighborNorthId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_NeighborWestId",
                        column: x => x.NeighborWestId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SpecialActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageRegex = table.Column<string>(type: "TEXT", nullable: true),
                    PatternForPlayer = table.Column<string>(type: "TEXT", nullable: true),
                    DungeonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialActions_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvatarTakeable",
                columns: table => new
                {
                    InventoryAvatarsId = table.Column<string>(type: "TEXT", nullable: false),
                    InventoryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarTakeable", x => new { x.InventoryAvatarsId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_AvatarTakeable_Avatars_InventoryAvatarsId",
                        column: x => x.InventoryAvatarsId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvatarTakeable_Inspectables_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inspectables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassTakeable",
                columns: table => new
                {
                    ClassesId = table.Column<string>(type: "TEXT", nullable: false),
                    StartInventoryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTakeable", x => new { x.ClassesId, x.StartInventoryId });
                    table.ForeignKey(
                        name: "FK_ClassTakeable_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassTakeable_Inspectables_StartInventoryId",
                        column: x => x.StartInventoryId,
                        principalTable: "Inspectables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectableRoom",
                columns: table => new
                {
                    InspectablesId = table.Column<string>(type: "TEXT", nullable: false),
                    RoomsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectableRoom", x => new { x.InspectablesId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_InspectableRoom_Inspectables_InspectablesId",
                        column: x => x.InspectablesId,
                        principalTable: "Inspectables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectableRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestableRoom",
                columns: table => new
                {
                    RoomsId = table.Column<string>(type: "TEXT", nullable: false),
                    SpecialActionsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestableRoom", x => new { x.RoomsId, x.SpecialActionsId });
                    table.ForeignKey(
                        name: "FK_RequestableRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestableRoom_SpecialActions_SpecialActionsId",
                        column: x => x.SpecialActionsId,
                        principalTable: "SpecialActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_ArmorId",
                table: "Avatars",
                column: "ArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_ChosenClassId",
                table: "Avatars",
                column: "ChosenClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_ChosenRaceId",
                table: "Avatars",
                column: "ChosenRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_CurrentRoomId",
                table: "Avatars",
                column: "CurrentRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_DungeonId",
                table: "Avatars",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_HoldingItemId",
                table: "Avatars",
                column: "HoldingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_OwnerId",
                table: "Avatars",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarTakeable_InventoryId",
                table: "AvatarTakeable",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_DungeonId",
                table: "Classes",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTakeable_StartInventoryId",
                table: "ClassTakeable",
                column: "StartInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonDungeonUser_BlackListId",
                table: "DungeonDungeonUser",
                column: "BlackListId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonDungeonUser1_WhiteListId",
                table: "DungeonDungeonUser1",
                column: "WhiteListId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonDungeonUser2_DungeonMastersId",
                table: "DungeonDungeonUser2",
                column: "DungeonMastersId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonDungeonUser3_OpenRequestsId",
                table: "DungeonDungeonUser3",
                column: "OpenRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_CurrentDungeonMasterId",
                table: "Dungeons",
                column: "CurrentDungeonMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_DefaultRoomId",
                table: "Dungeons",
                column: "DefaultRoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_DungeonOwnerId",
                table: "Dungeons",
                column: "DungeonOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectableRoom_RoomsId",
                table: "InspectableRoom",
                column: "RoomsId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspectables_DungeonId",
                table: "Inspectables",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_DungeonId",
                table: "Races",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestableRoom_SpecialActionsId",
                table: "RequestableRoom",
                column: "SpecialActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DungeonId",
                table: "Rooms",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NeighborNorthId",
                table: "Rooms",
                column: "NeighborNorthId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NeighborWestId",
                table: "Rooms",
                column: "NeighborWestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialActions_DungeonId",
                table: "SpecialActions",
                column: "DungeonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Classes_ChosenClassId",
                table: "Avatars",
                column: "ChosenClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Dungeons_DungeonId",
                table: "Avatars",
                column: "DungeonId",
                principalTable: "Dungeons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Inspectables_ArmorId",
                table: "Avatars",
                column: "ArmorId",
                principalTable: "Inspectables",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Inspectables_HoldingItemId",
                table: "Avatars",
                column: "HoldingItemId",
                principalTable: "Inspectables",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Races_ChosenRaceId",
                table: "Avatars",
                column: "ChosenRaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Rooms_CurrentRoomId",
                table: "Avatars",
                column: "CurrentRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DungeonDungeonUser_Dungeons_BlackListDungeonsId",
                table: "DungeonDungeonUser",
                column: "BlackListDungeonsId",
                principalTable: "Dungeons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DungeonDungeonUser1_Dungeons_WhiteListDungeonsId",
                table: "DungeonDungeonUser1",
                column: "WhiteListDungeonsId",
                principalTable: "Dungeons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DungeonDungeonUser2_Dungeons_DungeonMasterDungeonsId",
                table: "DungeonDungeonUser2",
                column: "DungeonMasterDungeonsId",
                principalTable: "Dungeons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DungeonDungeonUser3_Dungeons_OpenRequestDungeonsId",
                table: "DungeonDungeonUser3",
                column: "OpenRequestDungeonsId",
                principalTable: "Dungeons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dungeons_Rooms_DefaultRoomId",
                table: "Dungeons",
                column: "DefaultRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dungeons_AspNetUsers_CurrentDungeonMasterId",
                table: "Dungeons");

            migrationBuilder.DropForeignKey(
                name: "FK_Dungeons_AspNetUsers_DungeonOwnerId",
                table: "Dungeons");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Dungeons_DungeonId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AvatarTakeable");

            migrationBuilder.DropTable(
                name: "ClassTakeable");

            migrationBuilder.DropTable(
                name: "DungeonDungeonUser");

            migrationBuilder.DropTable(
                name: "DungeonDungeonUser1");

            migrationBuilder.DropTable(
                name: "DungeonDungeonUser2");

            migrationBuilder.DropTable(
                name: "DungeonDungeonUser3");

            migrationBuilder.DropTable(
                name: "InspectableRoom");

            migrationBuilder.DropTable(
                name: "RequestableRoom");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "SpecialActions");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Inspectables");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
