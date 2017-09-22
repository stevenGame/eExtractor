using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace eExtractor.Parser
{
    /// <summary>
    /// Generic parse text content or file to object
    ///  with JSON file description rules or dictionary of rules
    ///  reflection map it to object 
    /// </summary>
    public class RegexParser
    {

        private Dictionary<string, string> _rules;

        /// <summary>
        /// avoid new keyword for parser
        /// </summary>
        private RegexParser(Dictionary<string, string> rules)
        {
            _rules = rules ?? throw new ArgumentNullException("rules");
        }

        /// <summary>
        /// Rules contains dictionary with key is object property name
        /// value is the regular expression extract text and value split 
        /// with double slash and comma
        /// 
        /// just like below
        /// {
        ///  prop1:"REGEX string match area\\,v value extract REGEX string"
        ///  prop2:"REGEX string match area\\,v value extract REGEX string"
        ///  }
        ///  value extract string is optional if not set use REGEX string match area 
        ///  it will automatically map text content to object
        /// </summary>
        public static RegexParser Build(Dictionary<string, string> rules)
        {
            return new RegexParser(rules);
        }
        /// <summary>
        /// Build REGEX parser from rule in JSON file
        /// just like below
        /// {
        ///  prop1:"REGEX string match area\\,v value extract REGEX string"
        ///  prop2:"REGEX string match area\\,v value extract REGEX string"
        ///  }
        /// </summary>
        /// <param name="ruleJsonPath">JOSN file location store REGEX parse rules from file</param>
        /// <returns></returns>
        public static RegexParser Build(string ruleJsonPath)
        {
            if (!File.Exists(ruleJsonPath))
            {
                throw new ArgumentException("ruleJsonPath must be a exist!");
            }

            string ruleText = File.ReadAllText(ruleJsonPath);
            var jsonConvert = new JavaScriptSerializer();
            var jsonRules = jsonConvert.Deserialize<Dictionary<string, string>>(ruleText);
            return Build(jsonRules);
        }

        /// <summary>
        /// parse text to object depend rules 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public T Parse<T>(string text)
            where T : class, new()
        {

            T someObj = new T();
            return _rules.Select(kv =>
                    {
                        var proeprty = someObj.GetType().GetProperty(kv.Key);
                        string areaRegex = kv.Value.Split(@"\\,")[0];
                        // value extract string is optional if not set use REGEX string match area 
                        string valueRegex = kv.Value.Split(@"\\,")[1] ?? areaRegex;

                        string areaMatch = text.Match(areaRegex);
                        string valueMatch = areaMatch.Match(valueRegex) ?? areaMatch;
                        valueMatch = valueMatch.Trim();
                        // type covert from string
                        PropertyInfo p = someObj.GetType().GetProperty(kv.Key);
                        Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                        object safeValue = (valueMatch == null) ? null : Convert.ChangeType(valueMatch, type);
                        return new
                        {
                            prop = kv.Key,
                            val = safeValue
                        };

                    }).ToDictionary(v => v.prop, v => v.val)
                      .ToObject<T>();
        }

    }
}
