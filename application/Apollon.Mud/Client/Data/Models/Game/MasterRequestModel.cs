using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Game
{
    /// <summary>
    /// The Model for the form the Dungeon Master has to fill out when answering a player request
    /// </summary>
    public class MasterRequestModel
    {
        /// <summary>
        /// The textual answer to the players request
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Spieler eine textliche Rückmeldung geben")]
        public string RequestAnswer { get; set; }

        /// <summary>
        /// The new HP of the player 
        /// </summary>
        [Required(ErrorMessage = "Du musst die neuen Avatar HP angeben")]
        public int HealthChange { get; set; }
    }
}
