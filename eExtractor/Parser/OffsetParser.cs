using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eExtractor.Extension;
namespace eExtractor.Parser
{
    /// <summary>
    /// Rules parameter for offset parser 
    /// </summary>
    public class OffsetRule
    {
        /// <summary>
        /// Field name in Object
        /// </summary>
        public string Field { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }

    /// <summary>
    /// Parse data from text content use offset rules
    /// </summary>
    public class OffsetParser<T>
    where T : class, new()
    {

        private List<OffsetRule> rules;

        public OffsetParser(List<OffsetRule> rules)
        {
            this.rules = rules;
            VerifyRule();
        }

        /// <summary>
        /// Can Verify rules in Contractor to save calculate time
        /// </summary>
        /// <returns></returns>
        private bool VerifyRule()
        {
            List<OffsetRule> commonRules = GetCommonRules();

            if (commonRules.Count != rules.Count)
            {
                string[] commonFieldsName = commonRules.Select(r => r.Field).ToArray();
                string[] notFoundFelids = rules.Where(r => !commonFieldsName.Contains(r.Field))
                                               .Select(r => r.Field).ToArray();
                throw new Exception($"Fields {string.Join(",", notFoundFelids)} not found please check parameters!");
            }

            return rules.Count == commonRules.Count;
        }
        private List<OffsetRule> GetCommonRules()
        {
            string[] propNames = typeof(T).GetProperties().Select(p => p.Name).ToArray();
            return rules.Where(r => propNames.Contains(r.Field)).ToList();
        }
        /// <summary>
        /// Parse text content to Object user offset rules
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public T Parse(string text)
        {
            return GetCommonRules().Select(r => new
            {
                prop = r.Field,
                val = text.Substring(r.Offset, r.Length)
            })
            .ToDictionary(dc => dc.prop, dc => dc.val)
            .ToObject<T>();
        }


    }

    /// <summary>
    /// static method port to offset parser
    /// </summary>
    public class OffsetExtractor
    {
        public static ParseT Parse<ParseT>(string text, List<OffsetRule> rules)
            where ParseT : class, new()
        {
            return Build<ParseT>(rules).Parse(text);
        }
        /// <summary>
        /// Use Builder patten to save memory for same parser batch of files
        /// </summary>
        /// <typeparam name="ParseT"></typeparam>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static OffsetParser<ParseT> Build<ParseT>(List<OffsetRule> rules)
             where ParseT : class, new()
        {
            var parser = new OffsetParser<ParseT>(rules);
            return parser;
        }
    }
}
