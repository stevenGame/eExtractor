using eExtractor.Parser;
using eExtractor.Test.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePdfExtractorUnitTest
{
    [TestClass]
    public class RegexParserTest
    {
        [TestMethod]
        public void ParseObjectText()
        {
            var text = File.ReadAllText(@"Samples\1099k.txt");

            var f1099k = RegexParser.Build(new Dictionary<string, string> {
                { "TaxYear", @"^\d{4}\s+(.*\n){2,6}(Form\s+1099.*-.*K)\\,^\d{4}.*$" },
                {"TaxID", @"PAYEE'S taxpayer identification no(.*\n){2,3}^\d{9}.*$\\,^\d{9}.*$" },
                {
                    "FederalID",
                    @"FILER'S federal identification no(.*\n)+(\d{2}-\d{7}(.*\n))PAYEE'S taxpayer identification no\\,\d{2}-\d{7}(.*$)"
                },
                {"AccountNum", @"Account number.*\n(.*\n){3,5}(\d{16}).*\\,^(\d{16}).*$" },

            }).Parse<Pdf1099K>(text);
            Assert.AreEqual(2012, f1099k.TaxYear);
            Assert.AreEqual("45-4145354", f1099k.FederalID);
            Assert.AreEqual("113019324", f1099k.TaxID);
            Assert.AreEqual("0000270200001005", f1099k.AccountNum);
        }

        [TestMethod]
        public void ParseObjectTextFromJsonRule()
        {
            var text = File.ReadAllText(@"Samples\1099k.txt");
            var f1099k = RegexParser.Build(@"Samples\f1099kRegexRules.json")
                                    .Parse<Pdf1099K>(text);
            Assert.AreEqual(2012, f1099k.TaxYear);
            Assert.AreEqual("45-4145354", f1099k.FederalID);
            Assert.AreEqual("113019324", f1099k.TaxID);
            Assert.AreEqual("0000270200001005", f1099k.AccountNum);
        }
    }
}
