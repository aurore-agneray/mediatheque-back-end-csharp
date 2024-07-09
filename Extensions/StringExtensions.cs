using System.Text.RegularExpressions;

namespace mediatheque_back_csharp.Extensions
{
    /// <summary>
    /// Extension methods dedicated to "string" objects
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extracts the ALPHABETICAL prefix of the given string
        /// </summary>
        /// <param name="input">A string value with letters and numbers</param>
        /// <returns>Returns a string alphabetical value or string.Empty
        /// if the string begins with a number</returns>
        public static string ExtractPrefix(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            var match = Regex.Match(input, @"^\D+");
            return match.Success ? match.Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Extracts the first number found into the given string
        /// </summary>
        /// <param name="input">A string value with letters and numbers</param>
        /// <returns>Returns a numerical value or 0 if the string 
        /// has no numbers</returns>
        public static int ExtractNumber(this string input)
        {
            if (input == null)
            {
                return 0;
            }

            var match = Regex.Match(input, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}