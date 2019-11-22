using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq.Compare
{
    public static class LinqExtensions
    {
        public static CompareResult<T> Compare<T>(this IEnumerable<T> originalItems, IEnumerable<T> newItems) where T : class
        {
            var comparer = new Comparer<T>();
            comparer.MembersToCompare.All();
            var result = comparer.Compare(originalItems, newItems);
            return result;
        }
    }
}
