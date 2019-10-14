using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq.Compare
{
    public static class LinqExtensions
    {
        public static CompareResult<T> Compare<T>(this IEnumerable<T> items, IEnumerable<T> compareItems)
        {
            var comparer = new Comparer<T>();
            var result = comparer.Compare(items, compareItems);
            return result;
        }
    }
}
