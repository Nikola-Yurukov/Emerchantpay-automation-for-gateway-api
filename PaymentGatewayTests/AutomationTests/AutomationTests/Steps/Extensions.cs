using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TechTalk.SpecFlow;

namespace AutomationTests.Steps
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the first two columns from the dictionary
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IDictionary<string, string> FirstTwoColumnsToDictionary(this Table table)
        {
            var result = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                result.Add(row[0], row[1]);
            }
            return result;
        }

        /// <summary>
        /// Trims a single occurence of a character from the start and end of the string
        /// The character must be present both at the start and at the end of the string.
        /// </summary>
        /// <param name="value" "The string to trim"></param>
        /// <param name="trimChar" "The character to be trimmed"></param>
        /// <returns></returns>
        public static string TrimSingleSurroundingCharacter(this string value, char trimChar)
        {
            if (value.StartsWith(trimChar.ToString()) && value.EndsWith(trimChar.ToString()))
                return value.Substring(1, value.Length - 2);

            return value;
        }
    }
}
