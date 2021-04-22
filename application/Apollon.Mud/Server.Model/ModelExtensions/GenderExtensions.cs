using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    public static class GenderExtensions
    {
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
