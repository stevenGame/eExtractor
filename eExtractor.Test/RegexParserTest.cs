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
        class Message
        {
            public string Code { get; set; }
            public string TimeStamp { get; set; }
            public string TextInBytes { get; set; }

            // AFH Service Extension
            public string FailedPanel { get; set; }

            // Extensions
            public string NewSerial { get; set; }
            public string OriginalSerial { get; set; }
            public string CardData { get; set; }
            public byte[] CardDataEncrypted { get; set; }
            public string Others { get; set; }
            public string Status { get; set; }
            public string CardReader { get; set; }
            public string Suspicious { get; set; }
            public bool IsValid { get; set; }
            public string Exception { get; set; }
            public string DateEx { get; set; }
            public string LogText { get; set; }
            public int ExceptionId { get; set; }
            public string CardType { get; set; }

        }

        [TestMethod]
        public void ParseMessageTest()
        {
            var text = @"P99912:29:12 10/26/17 P5V1_211";

            var msg = RegexParser.Build(new Dictionary<string, string> {
                { "Code", @"^.{4}\\,\d{3}$" },
                { "TimeStamp", @"\d{2}:\d{2}:\d{2}" },
                { "DateEx", @"\d{2}\/\d{2}\/\d{2}" },
                { "OriginalSerial", @".{4}_\d{3}.?$\\,.{4}_\d{3}" }
            }).Parse<Message>(text);

            Assert.AreEqual("999", msg.Code);
            Assert.AreEqual("12:29:12", msg.TimeStamp);
            Assert.AreEqual("10/26/17", msg.DateEx);
            Assert.AreEqual("P5V1_211", msg.OriginalSerial); // use it as Site ID or Serial number
        }
    }
}
