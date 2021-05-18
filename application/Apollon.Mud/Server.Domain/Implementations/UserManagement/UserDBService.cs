using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <inheritdoc cref="IUserDbService"/>
    public class UserDbService: IUserDbService
    {
        #region member
        /// <summary>
        /// Manager to access and modify the content of the database.
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;

        /// <summary>
        /// Service to access and save dungeon content.
        /// </summary>
        private readonly IGameDbService _gameDbService;
        #endregion


        public UserDbService(UserManager<DungeonUser> userManager, IGameDbService gameDbService)
        {
            _userManager = userManager;
            _gameDbService = gameDbService;
        }

        #region methods
        /// <inheritdoc cref="IUserDbService.CreateUser"/>
        public async Task<bool> CreateUser(DungeonUser user, string password, bool asAdmin = false)
        {
            var creationSucceeded = await _userManager.CreateAsync(user, password);

            if (!creationSucceeded.Succeeded)
            {
                return false;
            }
            var addedToRoles = await _userManager.AddToRoleAsync(user, Roles.Player.ToString());

            if (!addedToRoles.Succeeded)
            {
                await RollbackUserCreation(user);
                return false;
            }

            if (asAdmin)
            {
                addedToRoles = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                if (!addedToRoles.Succeeded)
                {
                    await RollbackUserCreation(user);
                    return false;
                }
                CreateTutorialDungeon(user);
            }

            return true;
        }

        /// <inheritdoc cref="IUserDbService.GetUser"/>
        [ExcludeFromCodeCoverage]
        public async Task<DungeonUser> GetUser(Guid userId)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        /// <inheritdoc cref="IUserDbService.GetUsers"/>
        [ExcludeFromCodeCoverage]
        public async Task<ICollection<DungeonUser>> GetUsers()
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.Users.ToListAsync();
        }

        /// <inheritdoc cref="IUserDbService.UpdateUser"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.DeleteUser"/>
        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetUserByEmail"/>
        [ExcludeFromCodeCoverage]
        public async Task<DungeonUser> GetUserByEmail(string userEmail)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.FindByEmailAsync(userEmail);
        }

        /// <inheritdoc cref="IUserDbService.ResetPassword"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> ResetPassword(DungeonUser user, string token, string password)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetResetToken"/>
        [ExcludeFromCodeCoverage]
        public async Task<string> GetResetToken(DungeonUser user)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.ConfirmEmail"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> ConfirmEmail(DungeonUser user, string token)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetEmailConfirmationToken"/>
        [ExcludeFromCodeCoverage]
        public async Task<string> GetEmailConfirmationToken(DungeonUser user)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.IsAdminRegistered"/>
        public async Task<bool> IsAdminRegistered()
        {
            var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin.ToString());
            return admins.Count > 0;
        }

        /// <summary>
        /// Undo the user creation.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal async Task RollbackUserCreation(DungeonUser user)
        {
            IdentityResult result;
            do
            {
                result = await _userManager.DeleteAsync(user);
            } while (!result.Succeeded);
        }

        /// <inheritdoc cref="IUserDbService.UpdateUserTimestamp"/>
        public async Task UpdateUserTimestamp(DungeonUser user)
        {
            user.LastActive = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.IsUserInRole"/>
        public async Task<bool> IsUserInRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <inheritdoc cref="IUserDbService.GetUsersInRole"/>
        public async Task<ICollection<DungeonUser>> GetUsersInRole(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        /// <inheritdoc cref="IUserDbService.AddUserToRole"/>
        public async Task<bool> AddUserToRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.RemoveUserFromRole"/>
        public async Task<bool> RemoveUserFromRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            return result.Succeeded;
        }

        /// <summary>
        /// Creates a tutorial Dungeon.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateTutorialDungeon(DungeonUser user)
        {

            var dungeon = new Dungeon("Griechische Mythologie",
                "Ein Dungeon der Neuankömmlinge eine Starthilfe in ApollonDungeons gibt.",
                "Wiki Dungeon")
            {
                Status = Status.Approved,
                Visibility = Visibility.Public,
                DungeonOwner = user,
                LastActive = DateTime.UtcNow,
                WhiteList = { user },
                DungeonMasters = { user }
            };
            _gameDbService.NewOrUpdate(dungeon);


            var wandschild =
                new Inspectable(
                    "Du ließt das Schild an der Wand. Darauf steht: \"Eindringlinge werden von den Göttern bestraft\".",
                    "Wandschild")
                {
                    Dungeon = dungeon,
                    Status = Status.Approved
                };

            _gameDbService.NewOrUpdate(wandschild);

            var weinflasche = new Consumable("Weinflasche",
                "Eine kostabare Rarität hier in der Gegend. Der Geschmack des Weines ist atemberaubend.", 2, "Du bist angeheitert von dem Alkohol im Wein.")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(weinflasche);

            var flasche = new Consumable("Flasche",
                "Eine normale Flasche mit einer durchsichtigen Flüssigkeit. Es könnte wohl Wasser sein", 2,
                "Die Götter bestragen dich dafür, dass du in das Haus eingedrungen bist!")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(flasche);

            var schild = new Wearable("Schild",
                "Ein Schild der getragen werden kann um die Verteidigung des Avatars zu steigern.", 10, 10)
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(schild);

            var schwert = new Usable("Schwert", "Eine spitze Klinge die dem Gegenüber Schaden zufügen kann.", 10, 10)
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(schwert);

            var soldat = new Npc("Der Soldat sagt zu dir: \"Du darfst derzeit die Stadt nicht betreten!\"",
                "Ein großer ausgerüsteter Soldat, mit finsterem Blick.", "Soldat")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(soldat);

            var mannMitUmhang = new Npc(
                "Als du den Mann anschaust, sagt er zu dir: \"Hey du, hey, haben dich die Soldaten nicht reingelassen hmm.Die lassen keinen rein.Aber, aber ich, ja ich kenne einen Weg rein.Gehe nach Norden, nahe der Stadt. Du wirst ein Loch in der Mauer der Stadt finden, dich dich in die Stadt reinbringt.\" Du bedankst dich bei dem Mann und schaust dir die Umgebung an. Es führt ein Weg in Richtung Norden, wie der Mann gerade beschrieben hat.",
                "Ein kleiner dreckiger Mann mit einem Umgang.", "Mann mit Umhang")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(mannMitUmhang);

            var sonnentanz = new Requestable("(S|s)onnentanz", "Sonnentanz")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(sonnentanz);

            var mensch = new Race("Mensch", "Der Mensch ist eine normale nicht magische Rasse. Er hat ausgewogene Eigenschaften.", 50, 25, 25)
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(mensch);

            var halbgott = new Race("Halbgott", "Halb Mensch halb Gott. Verbannt vom Olymp auf die Erde.", 40, 30, 30)
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(halbgott);

            var krieger = new Class("Krieger", "Ein helfender Krieger wessen Fertigkeiten in der Unterstützung liegen.", 80, 50,20)
            {
                Dungeon = dungeon,
                Status = Status.Approved,
                StartInventory = { schild, weinflasche }
            };
            _gameDbService.NewOrUpdate(krieger);

            var laeufer = new Class("Läufer", "Ein flinker unauffälliger Kamerad, welcher im Schatten agiert.", 50, 20, 80)
            {
                Dungeon = dungeon,
                Status = Status.Approved,
                StartInventory = { schwert, weinflasche }
            };
            _gameDbService.NewOrUpdate(laeufer);

            var großeWiese = new Room("Zu sehen ist eine große Wiese. Wie bin ich hier hergekommen? Die Wiese ist umgeben von Klippen. Wer wohl Zeus wütend gemacht hat, dass er uns mit derartigem Regen bestraft. Ich sehe einen Wanderweg Richtung Osten, der zu einem kleinen Haus führt. Nutze den Befehl \"hilfe\", um dir die Befehlsmöglichkeiten anzeigen zu lassen. ", "Große Wiese")
            {
                Dungeon = dungeon,
                Status = Status.Approved,
                SpecialActions = { sonnentanz }
            };
            _gameDbService.NewOrUpdate(großeWiese);

            var vorDemKleinenHaus = new Room("Der Regen lässt nicht nach, der Wanderweg ist zu Schlamm geworden. In der ferne Richtung Osten sieht man eine Stadt. Ist das wohl Athen... An der Wand des Hauses befindet sich ein Wandschild. Ich kann nach Norden in das Haus rein gehen. Ich sehe eine Stadt in Richtung Osten (Du kannst Gegenstände und Objekte näher betrachten, versuche es mit \"untersuche wandschild\").", "Vor dem kleinem Haus")
            {
                Dungeon = dungeon,
                Status = Status.Approved,
                SpecialActions = {sonnentanz},
                Inspectables = { wandschild }
            };
            _gameDbService.NewOrUpdate(vorDemKleinenHaus);

            var imKleinenHaus = new Room("Das kleine Haus ist frei von Menschen. Sieht nach einem normalen Haus aus. Der einzige Weg führt wieder durch Tür raus nach Süden (Wenn du Probleme hast, suche mit \"hilfe\" nach dem passenden Befehl).", "Im kleinen Haus")
            {
                Dungeon = dungeon,
                Status = Status.Approved,
                Inspectables = { flasche }
            };
            _gameDbService.NewOrUpdate(imKleinenHaus);

            var vorDerStadt = new Room(
                "Du bist am Ortseingang der Stadt. Vor die steht ein großer Soldat und versperrt dir den Weg in die Stadt. Der Regen ist noch schlimmer als zuvor. Du schaust dich um und siehst einen Mann im Umhang der sich südlich der Stadt befindet (Du kannst den Soldaten mit \"sprich soldat an\" ansprechen, versuche es mal).",
                "Vor der Stadt")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(vorDerStadt);

            var suedlicheDerStadt = new Room(
                "Der Mann mit Umhang ist hier. Er steht da und regt sich kaum (Du hast im letzten Raum gelernt, wie du mit der Welt kommunizieren kannst. Falls du dich nicht mehr erinnerst, siehe mit der \"hilfe\" nochmals nach).",
                "Südlich von der Stadt")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(suedlicheDerStadt);

            var naeheStadtmauer = new Room("Du glaubst nicht was du siehst. Der kleiner dreckige Mann hatte Recht. Da ist ein kleines Loch in der Stadtmauer. Groß genug sich durchzudrücken. Du schaust dich um, keiner ist in deiner Nähe, selbst der Mann mit dem Umhang ist nicht mehr zu sehen. Glückwunsch du hast das Tutorial erfolgreich beendet! In der Welt der Dungeons kannst du selbst Dungeons erstellen oder in bereits bestehende Dungeons eintauchen.", 
                "Südlich der Stadt, Nähe der Stadtmauer")
            {
                Dungeon = dungeon,
                Status = Status.Approved
            };
            _gameDbService.NewOrUpdate(naeheStadtmauer);

            vorDemKleinenHaus.NeighborWest = großeWiese;
            vorDemKleinenHaus.NeighborNorth = imKleinenHaus;
            vorDemKleinenHaus.NeighborEast = vorDerStadt;

            _gameDbService.NewOrUpdate(vorDemKleinenHaus);

            suedlicheDerStadt.NeighborWest = vorDerStadt;
            suedlicheDerStadt.NeighborNorth = naeheStadtmauer;

            _gameDbService.NewOrUpdate(suedlicheDerStadt);

            dungeon.DefaultRoom = großeWiese;
            _gameDbService.NewOrUpdate(dungeon);
        }
        #endregion
    }
}