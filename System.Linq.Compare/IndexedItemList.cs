using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System.Linq.Compare
{
    internal class IndexedItemList<T>
    {
        private MemberSelector<T> _identifyingMembers;
        private Dictionary<string, List<T>> _items = new Dictionary<string, List<T>>();

        public IndexedItemList(MemberSelector<T> identifyingMembers)
            : this(Enumerable.Empty<T>(), identifyingMembers)
        {
        }

        public IndexedItemList(IEnumerable<T> items, MemberSelector<T> identifyingMembers)
        {
            _identifyingMembers = identifyingMembers;

            foreach (var item in items)
            {
                Add(item);
            }
        }

        public IEnumerable<T> GetItemsWithIdenticalIdentity(T item)
        {
            var index = GetIndex(item);

            if (_items.TryGetValue(index, out var itemList))
            {
                return itemList
                    .Where(i => _identifyingMembers.Equal(i, item));
            }

            return Enumerable.Empty<T>();
        }

        public void Add(T item)
        {
            var index = GetIndex(item);

            if (!_items.TryGetValue(index, out var itemList))
            {
                itemList = new List<T>();
                _items.Add(index, itemList);
            }

            itemList.Add(item);
        }

        private string GetIndex(T item)
        {
            var result = new StringBuilder();

            foreach (var member in _identifyingMembers.Members)
            {
                object value = null;
                if (member is PropertyInfo propertyInfo)
                {
                    value = propertyInfo.GetValue(item);
                }
                else if (member is FieldInfo fieldInfo)
                {
                    value = fieldInfo.GetValue(item);
                }

                result.Append(value?.GetHashCode().ToString());
            }

            return result.ToString();
        }
    }
}