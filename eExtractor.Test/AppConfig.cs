using System.Configuration;
namespace eExtractor.Test
{
    public class AppConfig
    {
        public static string pdf1099k
        {
            get
            {
                return ConfigurationManager.AppSettings["pdf1099k"];
            }
        }
        public static string f1099kRulePath
        {
            get
            {
                return ConfigurationManager.AppSettings["f1099kRulePath"];
            }
        }
    }
}
