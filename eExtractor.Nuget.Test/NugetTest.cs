using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using System.Collections.Generic;
using eExtractor.Logic;

namespace eExtractor.Nuget.Test
{
    [TestClass]
    public class NugetTest
    {
        public class Pdf1099K
        {
            public int ID { get; set; }
            public int TaxYear { get; set; }
            public string FileName { get; set; }
            public string FileLocation { get; set; }
            public string TaxID { get; set; }
            public string FederalID { get; set; }
            public string AccountNum { get; set; }
            public byte[] PDFContent { get; set; }
            public string UpdateBy { get; set; }
            public Nullable<System.DateTime> UpdateAt { get; set; }
            public string CreateBy { get; set; }
            public Nullable<System.DateTime> CreateAt { get; set; }
        }
        [TestMethod]
        public void TestNugetFunction()
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
    }
}
