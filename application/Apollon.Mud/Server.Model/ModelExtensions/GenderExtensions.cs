using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    /// <summary>
    /// Extension Methods for <see cref="Gender"/>
    /// </summary>
    public static class GenderExtensions
    {
        /// <summary>
        /// Gives german expression for the given gender
        /// </summary>
        /// <param name="gender">The <see cref="Gender"/> value</param>
        /// <returns>The german value string</returns>
        public static string GetGermanGender(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Männlich",
                Gender.Female => "Weiblich",
                Gender.Diverse => "Divers",
                Gender.Neutral => "Neutral",
                _ => "Unbekannt",
            };
        }
    }
}
