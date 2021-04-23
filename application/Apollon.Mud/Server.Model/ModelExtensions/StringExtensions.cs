using System;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    //TODO: Evt. verschieben (dahin wos benötigt wird)
    public static class StringExtensions
    {
        public static string NormalizeString(this string input)
        {
            return string.Join(" ", input.ToLower().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
