using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    public static class StringExtensions
    {
        public static string NormalizeString(this string input)
        {
            return string.Join(" ", input.ToLower().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
