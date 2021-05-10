using System;
using System.Diagnostics.CodeAnalysis;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    /// <summary>
    /// Extensions for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Normalizes string to lower and cuts off all extra white spaces
        /// </summary>
        /// <param name="input">String that should get normalized</param>
        /// <returns>Normalized string</returns>
        [ExcludeFromCodeCoverage]
        public static string NormalizeString(this string input)
        {
            return string.Join(" ", input.ToLower().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
