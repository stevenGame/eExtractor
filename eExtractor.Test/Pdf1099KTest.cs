﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using eExtractor.Logic;
using eExtractor.Test.Entity;

namespace eExtractor.Test
{
    [TestClass]
    public class Pdf1099KTest
    {
        private string testFolder;
        [TestInitialize()]
        public void SetUp()
        {
            testFolder = @"\\us001-pc-fs01\departments\MIS\DEV\Projects\1099K";
        }
        /// <summary>
        /// Network drive contains error file now allow exception
        /// It will throw null reference exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestExtract()
        {
            Pdf1099KExtractor extractor = new Pdf1099KExtractor(testFolder,
                                                                "2017");
            var f1099k = extractor.Extract().FirstOrDefault();
            Assert.IsNotNull(f1099k);
            Assert.AreNotEqual(0, f1099k.ID);
        }

        [TestMethod]
        public void TestExtractSamples()
        {
            Pdf1099KExtractor extractor = new Pdf1099KExtractor(@"Samples\Form1099K",
                                                                "2017");
            var f1099k = extractor.Extract().FirstOrDefault();
            Assert.IsNotNull(f1099k);
            Assert.AreNotEqual(0, f1099k.ID);
        }
        [TestMethod]
        public void TestParse()
        {
            string content = FileUtil.GetPdfContent($"{testFolder}\\PROD12_1099K_Forms_Part1953.pdf");
            var f1099k = File1099KParser.Parse(content);
            Assert.AreEqual(2012, f1099k.TaxYear);
            Assert.AreEqual("45-4145354", f1099k.FederalID);
            Assert.AreEqual("113019324", f1099k.TaxID);
            Assert.AreEqual("0000270200001005", f1099k.AccountNum);

        }

        [TestMethod]
        public void TextReadContent()
        {
            string content = FileUtil.GetPdfContent($"{testFolder}\\PROD12_1099K_Forms_Part1953.pdf");

            Assert.IsNotNull(content);
            Assert.IsTrue(content.Contains("2012"));        // year
            Assert.IsTrue(content.Contains("45-4145354"));  // federal identification no.
            Assert.IsTrue(content.Contains("113019324"));   // tax ID
            Assert.IsTrue(content.Contains("0000270200001005")); // account number

        }
        [TestMethod]
        public void TestForms()
        {

            var f1099k = File1099KParser.ParseForms($"{testFolder}\\PROD12_1099K_Forms_Part1953.pdf");
            //Assert.AreEqual(2012, f1099k.TaxYear);
            //Assert.AreEqual("45-4145354", f1099k.FederalID);
            //Assert.AreEqual("113019324", f1099k.TaxID);
            //Assert.AreEqual("0000270200001005", f1099k.AccountNum);

        }
        [TestMethod]
        public void TestParseTable()
        {
            var f1099k = File1099KParser.ParseTable($"{testFolder}\\PROD12_1099K_Forms_Part1953.pdf");
            //Assert.AreEqual(2012, f1099k.TaxYear);
            //Assert.AreEqual("45-4145354", f1099k.FederalID);
            //Assert.AreEqual("113019324", f1099k.TaxID);
            //Assert.AreEqual("0000270200001005", f1099k.AccountNum);
        }

        [TestMethod]
        public void SaveMutipleTime()
        {
            using (var ctx = new Extract1099KEntities())
            {
                var testfile = Pdf1099K.GetByTaxYearAndID(2012, "113019324").FirstOrDefault();
                var federalID = testfile.FederalID;
                testfile.FederalID = "xxxxxxx";
                testfile.Save(ctx);
                ctx.SaveChanges();
                var savedFile = Pdf1099K.GetByTaxYearAndID(2012, "113019324").FirstOrDefault();
                Assert.AreEqual("xxxxxxx", testfile.FederalID);
                testfile.FederalID = federalID;
                testfile.Save(ctx);
                ctx.SaveChanges();
                savedFile = Pdf1099K.GetByTaxYearAndID(2012, "113019324").FirstOrDefault();
                Assert.AreEqual(federalID, testfile.FederalID);
            }
        }

    }
}