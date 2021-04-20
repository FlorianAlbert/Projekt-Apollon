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
            switch (gender)
            {
                case Gender.Male:
                    return "Männlich";
                case Gender.Female:
                    return "Weiblich";
                case Gender.Diverse:
                    return "Divers";
                case Gender.Neutral:
                    return "Neutral";
                default:
                    return "Unbekannt";
            }
        }
    }
}
