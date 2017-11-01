using eExtractor.Test.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Test
{
    [TestClass]
    public class SavableEntityTest
    {
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
      

        [TestMethod]
        public void NewInMemoryObjctUpdateTest()
        {
            var x = Pdf1099K.GetByTaxYearAndID(2012, "113019324").FirstOrDefault();
            var newObj = new Pdf1099K
            {
                TaxYear = x.TaxYear,
                FileName = x.FileName,
                TaxID = x.TaxID,
                AccountNum = x.AccountNum,
                FederalID = x.FederalID,
                FileLocation = x.FileLocation,
                PDFContent = x.PDFContent
            };
            newObj.Save();
            Assert.AreEqual(x.ID, newObj.ID);
            Assert.IsTrue(DateTime.Now.Subtract(newObj.UpdateAt.Value) < TimeSpan.FromSeconds(3));
        }
        [TestMethod]
        public void TestInContext()
        {
            using (var ctx = new Extract1099KEntities())
            {
                var testfile = ctx.Pdf1099K.FirstOrDefault();
                testfile.Save(ctx);
                ctx.SaveChanges();
                Assert.IsTrue(DateTime.Now.Subtract(testfile.UpdateAt.Value) < TimeSpan.FromSeconds(3));
            }
        }
    }
}
