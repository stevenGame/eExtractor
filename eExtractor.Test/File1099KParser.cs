using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using iTextSharp.text.pdf;
using eExtractor.Test.Entity;

namespace eExtractor.Logic
{
    /// TODO should not have file prefix but 1099 is a number
    /// <summary>
    /// This class not use any more
    /// </summary>
    [Obsolete("this class not use any more check RegexParser for more detail will delete for ext version")]
    public class File1099KParser
    {
        private static File1099KParser _parser;
        public string content;
        /// <summary>
        /// store parsed result
        /// </summary>
        private Pdf1099K _result;

        /// <summary>
        /// parse text content to 1099 object
        /// </summary>
        /// <param name="content">content 1099k form format</param>
        /// <returns></returns>
        public static Pdf1099K Parse(string content)
        {
            File1099KParser _parser = Instance();
            // for safety use new result every time
            _parser._result = new Pdf1099K();
            _parser.content = content;
            _parser.ParseYear()
                   .ParseTaxID()
                   .ParseFederalID()
                   .ParseAccountNum();

            return _parser._result;
        }
        /// <summary>
        /// try to use read PDF form fields
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public static Pdf1099K ParseForms(string pdfPath)
        {
            PdfReader pdfReader = new PdfReader(pdfPath);
            var fields = pdfReader.AcroFields.Fields;

            return null;
        }
       
        /// <summary>
        /// use singleton avoid new key word and save memory resource in loop 
        /// not thread save yet but can easily implement if need process file in multiple thread 
        /// </summary>
        /// <returns></returns>
        private static File1099KParser Instance()
        {
            if (_parser == null)
            {
                _parser = new File1099KParser();
            }
            return _parser;
        }
        // TODO can write a generic function read regular express
        private File1099KParser ParseYear()
        {
            string yearCells = Match(@"^\d{4}\s+(.*\n){2,6}(Form\s+1099.*-.*K)"); // year cell
            string yearStr = RegexExtension.Match(yearCells, @"^\d{4}.*$").Trim();
            _result.TaxYear = int.Parse(yearStr);
            return this;
        }

        private File1099KParser ParseTaxID()
        {
            string taxCell = Match(@"PAYEE'S taxpayer identification no(.*\n){2,3}^\d{9}.*$");
            string taxIDStr = RegexExtension.Match(taxCell, @"^\d{9}.*$").Trim();
            _result.TaxID = taxIDStr;
            return this;
        }
        private File1099KParser ParseFederalID()
        {
            string federalIDCell = Match(@"FILER'S federal identification no(.*\n)+(\d{2}-\d{7}(.*\n))PAYEE'S taxpayer identification no");
            string federalID = RegexExtension.Match(federalIDCell, @"\d{2}-\d{7}(.*$)");
            _result.FederalID = federalID.Trim();
            return this;
        }
        private File1099KParser ParseAccountNum()
        {
            string accountCell = Match(@"Account number.*\n(.*\n){3,5}(\d{16}).*");
            string accountIdStr = RegexExtension.Match(accountCell, @"^(\d{16}).*$").Trim();
            _result.AccountNum = accountIdStr;
            return this;
        }
        /// <summary>
        /// get match pattern string in content
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private string Match(string pattern)
        {
            return RegexExtension.Match(content, pattern);
        }

        #region Testing 


        public static Pdf1099K ParseTable(string pdfPath)
        {
            var content = Read(pdfPath);
            return null;
        }

        public static string Read(string _filePath)
        {
            var pdfReader = new PdfReader(_filePath);
            var pages = new List<String>();

            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {
                string textFromPage = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default,
                                                              Encoding.UTF8,
                                                              pdfReader.GetPageContent(i + 1)));

                pages.Add(GetDataConvertedData(textFromPage));
            }

            return string.Join("--------------------------\n", pages);
        }

        static string GetDataConvertedData(string textFromPage)
        {
            var texts = textFromPage.Split(new[] { "\n" }, StringSplitOptions.None)
                                    .Where(text => text.Contains("Tj")).ToList();

            return texts.Aggregate(string.Empty, (current, t) => current +
                       t.TrimStart('(')
                        .TrimEnd('j')
                        .TrimEnd('T')
                        .TrimEnd(')'));
        }
        #endregion
    }
}
