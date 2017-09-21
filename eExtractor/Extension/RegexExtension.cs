using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eExtractor
{
    public class RegexExtension
    {
        /// <summary>
        /// easier version to use for check Regular Expressions match
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(string str, string pattern)
        {
            return Regex.Match(str, pattern, RegexOptions.Multiline).Success;
        }

        /// <summary>
        /// easier version regular expressions get first match value
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(string str, string pattern)
        {
            Regex r = new Regex(pattern, RegexOptions.Multiline);
            Match m = r.Match(str);
            while (m.Success)
            {
                return m.Value;
            }
            return null;
        }

    }
}
