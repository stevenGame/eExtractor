using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor
{
    public static class StringExtension
    {
        /// <summary>
        /// easier version to split string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            var test = str.Split(new string[] { separator }, StringSplitOptions.None);
            return str.Split(new string[] { separator }, StringSplitOptions.None);
        }

        public static string Match(this string str, string regex)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return RegexExtension.Match(str, regex);
        }
    }
}
