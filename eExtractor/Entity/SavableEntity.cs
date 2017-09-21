using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Entity
{
    /// <summary>
    /// Support automatically Save and update function for entity object
    /// Which implement ISavable interface
    /// </summary>
    /// <typeparam name="EntityT">Entity database context type</typeparam>
    /// <typeparam name="SourceT">Entity object type</typeparam>
    public class SavableEntity<EntityT, SourceT>
        where EntityT : DbContext, new()
        where SourceT : class, ISavable<SourceT>
    {

        public void Save(EntityT ctx, string updateOrCreateBy = null)
        {
            ISavable<SourceT> _this = (ISavable<SourceT>)this;

            if (Exist(ctx))
            {
                /// update the updates fields 
                if (!string.IsNullOrEmpty(updateOrCreateBy))
                {
                    _this.UpdateBy = updateOrCreateBy;
                }
                _this.UpdateAt = DateTime.Now;

                // for performance only need switch and assign this object to exist object 
                // when the entity detached 
                if (ctx.Entry(_this.GetThis()).State == EntityState.Detached)
                {
                    var existObj = ctx.Set<SourceT>()
                                      .Where(_this.GetExistPredicate())
                                      .FirstOrDefault();
                    AssignTo(existObj);
                    OnUpdate(existObj);
                    ctx.Entry(existObj).State = EntityState.Modified;
                }
                else
                {
                    ctx.Entry(_this.GetThis()).State = EntityState.Modified;
                }

            }
            else
            {
                ctx.Set<SourceT>().Add(_this.GetThis());
                if (!string.IsNullOrEmpty(updateOrCreateBy))
                {
                    _this.CreateBy = updateOrCreateBy;
                }
                _this.CreateAt = DateTime.Now;
            }
            _this.UpdateAt = DateTime.Now;

        }

        /// <summary>
        /// Save single entity  
        /// </summary>
        /// <param name="updateOrCreateBy"></param>
        public void Save(string updateOrCreateBy = null)
        {
            using (var ctx = new EntityT())
            {
                Save(ctx, updateOrCreateBy);
                ctx.SaveChanges();
            }
        }

        public SourceT Delete(EntityT ctx)
        {
            if (Exist(ctx))
            {
                ISavable<SourceT> _this = (ISavable<SourceT>)this;
                SourceT existObj = ctx.Set<SourceT>().Where(_this.GetExistPredicate()).FirstOrDefault();
                ctx.Set<SourceT>().Remove(existObj);
                ctx.SaveChanges();
                return existObj;
            }
            return null;
        }

        public SourceT Delete()
        {
            using (var ctx = new EntityT())
            {
                return Delete(ctx);
            }
        }

        /// <summary>
        /// Check object Exist in database or not
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool Exist(EntityT ctx)
        {
            var savable = (ISavable<SourceT>)this;
            return ctx.Set<SourceT>().Any(savable.GetExistPredicate());
        }

        /// <summary>
        /// Check object Exist in database or not
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public bool Exist()
        {
            using (var ctx = new EntityT())
            {
                return Exist(ctx);
            }
        }

        /// <summary>
        /// support other custom exist and save function
        /// Assign this object to database object using reflection
        /// Except ID filed or custom ID property 
        /// </summary>
        /// <remarks>
        /// TODO implement Get IDs ignore all the ID fields in Savable interface
        ///      if there are any table primary key not Name ID, or have multiple ID
        /// 
        /// </remarks>
        /// <returns>database object</returns>
        private SourceT AssignTo(SourceT databaseObj)
        {
            SourceT obj = this.Cast<ISavable<SourceT>>().GetThis();
            // ignore ID property
            // TODO change it to function or interface make it more generic
            List<PropertyInfo> needUpdate = obj.GetType()
                                            .GetProperties()
                                            .Where(p => p.Name != "ID") // ignore ID
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
        /// Update ID If create ID in data base but not set in database yet 
        /// </summary>
        ///<remarks>
        /// TODO implement Get IDs ignore all the ID fields in Savable interface
        ///      if there are any table primary key not Name ID, or have multiple ID
        /// 
        /// </remarks>
        /// <param name="dbObject"></param>
        private void OnUpdate(SourceT dbObject)
        {
            // ID = existObj.ID;
            PropertyInfo idProp = GetType().GetProperty("ID");
            object dbId = idProp.GetValue(dbObject);
            idProp.SetValue(this, dbId);
        }

    }
}
