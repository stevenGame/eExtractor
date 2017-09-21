using eExtractor.Test.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Test.Entity
{
    public partial class Pdf1099K
    {
        public static List<Pdf1099K> GetByTaxYearAndID(int taxYear, string TaxID)
        {
            using (var ctx = new Extract1099KEntities())
            {
                return ctx.Pdf1099K
                          .Where(f => f.TaxYear == taxYear && TaxID == f.TaxID)
                          .ToList();
            }
           
        }
        /// <summary>
        /// For this application use context support batch update 
        /// and insert, better as Access of this class not need 
        /// pass parameter of database context
        /// </summary>
        /// <param name="ctx"></param>
        public void Save(Extract1099KEntities ctx, string updateOrCreateBy = null)
        {
            if (Exist(ctx))
            {
                Pdf1099K existObj = ctx.Pdf1099K.Where(f => f.FileName == FileName).FirstOrDefault();
                ID = existObj.ID;
                if (!string.IsNullOrEmpty(updateOrCreateBy))
                {
                    this.UpdateBy = updateOrCreateBy;
                }
                this.UpdateAt = DateTime.Now;
                // assign this object to database
                Assign(this, existObj);
                ctx.Entry(existObj).State = EntityState.Modified;
            }
            else
            {
                ctx.Pdf1099K.Add(this);
                if (!string.IsNullOrEmpty(updateOrCreateBy))
                {
                    CreateBy = updateOrCreateBy;
                }
                CreateAt = DateTime.Now;
            }
            UpdateAt = DateTime.Now;

        }

        /// <summary>
        /// TODO change it to generic function if need support other type
        /// support other custom exist and save function
        /// Assign this object to database object using reflection
        /// Except ID filed or custom ID property 
        /// </summary>
        /// <returns>database object</returns>
        private Pdf1099K Assign(Pdf1099K obj, Pdf1099K databaseObj)
        {
            // ignore ID property
            // TODO change it to function or interface make it more generic
            List<PropertyInfo> needUpdate = obj.GetType()
                                            .GetProperties()
                                            .Where(p => p.Name != "ID")
                                            .Where(p => p.GetValue(obj) != null) // ignore null value from source
                                            .ToList();

            needUpdate.ForEach(p =>
            {
                Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                object value = p.GetValue(obj);
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                p.SetValue(databaseObj, safeValue, null);
            });
            return databaseObj;
        }

        /// <summary>
        /// TODO !
        /// use file name as primary key now unless we know the primary key
        /// is TaxID and TaxYear and account number 
        /// As I know people can file multiple 1099k per year -- by Steven Wu
        /// unless account number is who pay the person who file 1099
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool Exist(Extract1099KEntities ctx)
        {
            return ctx.Pdf1099K.Any(f => f.FileName == FileName);
        }
    }
}
