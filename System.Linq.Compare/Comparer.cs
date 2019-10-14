using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Compare
{
    public class Comparer<T>
    {
        internal CompareResult<T> Compare<T>(IEnumerable<T> items, IEnumerable<T> compareItems)
        {
            var result = new CompareResult<T>();

            foreach ()
            {

            }


            return result;
        }


        private class EqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly PropertyInfo[] _properties;

            public EqualityComparer()
            {
                _properties = typeof(T).GetProperties();
            }


            public bool Equals(T x, T y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

    }
}
