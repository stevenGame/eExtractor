using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Entity
{
    /// <summary>
    /// Interface for savable have to be implement 
    /// exists function in database
    /// the save function will automatically update the update,and create fields 
    /// </summary>
    public interface ISavable<TSource>
    {
        string UpdateBy { get; set; }
        Nullable<System.DateTime> UpdateAt { get; set; }
        string CreateBy { get; set; }
        Nullable<System.DateTime> CreateAt { get; set; }
        Expression<Func<TSource, bool>> GetExistPredicate();
        TSource GetThis();
      
    }
}
