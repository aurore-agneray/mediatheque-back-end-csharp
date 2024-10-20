using System.Text.RegularExpressions;

namespace ApplicationCore.Extensions
{
    /// <summary>
    /// Extension methods dedicated to "string" objects
    /// </summary>
    public static partial class StringExtensions
    {
        /// <remarks>
        /// The attribute improves the performances of the regex while
        /// preparing it within compile time. That avoids the use of
        /// reflection at runtime. It also gives extra information
        /// about the regex conditions when the developer hovers above
        /// the name of the source method.
        /// </remarks>
        [GeneratedRegex(@"^\D+")]
        private static partial Regex ExtractPrefixRegex();

        [GeneratedRegex(@"\d+")]
        private static partial Regex ExtractNumberRegex();

        /// <summary>
        /// Extracts a prefix without numbers of the given string
        /// </summary>
        /// <param name="input">A string value with numbers and other characters</param>
        /// <returns>Returns a string value without numbers or string.Empty
        /// if the string begins with a number</returns>
        public static string ExtractPrefixWithoutNumbers(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            var match = ExtractPrefixRegex().Match(input);
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

            var match = ExtractNumberRegex().Match(input);
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}