using eExtractor.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Util
{
    public class FileUtil
    {
        /// <summary>
        /// search files under folder return dictionary 
        ///     key is file name
        ///     value is file location
        /// </summary>
        /// <param name="folder">folder need to search</param>
        /// <param name="pattern">regular expression pattern to search</param>
        /// <returns>List of dictionary contain file name and path</returns>
        public static Dictionary<string, string> Search(string folder, string pattern)
        {
            return ListFiles(folder, pattern).ToDictionary(f => Path.GetFileName(f), f => f);
        }
        /// <summary>
        /// list all files under folder with specify pattern
        /// </summary>
        /// <param name="folder">folder name</param>
        /// <param name="pattern">regular expression pattern to search</param>
        /// <returns>return list of files location</returns>
        public static List<string> ListFiles(string folder, string pattern)
        {
            FileAttributes attr = File.GetAttributes(folder);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("Folder must exist and be a directory");
            }
            List<string> files = Directory.GetFiles(folder)
                                          .Where(f => RegexExtension.IsMatch(f, pattern))
                                          .ToList();
            return files;
        }
        /// <summary>
        /// Get specify file PDF content
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetPdfContent(string filePath)
        {
            PDFParser pdfParser = new PDFParser();

            return pdfParser.ExtractText(filePath); ;
        }

    }
}
