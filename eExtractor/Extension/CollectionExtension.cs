using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Extension
{
    public static class CollectionExtension
    {
        public static T TryGet<T>(this T[] arr, int index)
        {
            if (index >= arr.Length)
            {
                return default(T);
            }
            return arr[index];
        }
    }
}
