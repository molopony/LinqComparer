using System.Collections.Generic;

namespace System.Linq.Compare
{
    public class CompareResult<T>
    {
        private readonly List<T> _unchangedItems;
        private readonly List<T> _addedItems;
        private readonly List<T> _removedItems;
        private readonly List<T> _updatedItems;

        public CompareResult()
        {
            _unchangedItems = new List<T>();
            _addedItems = new List<T>();
            _removedItems = new List<T>();
            _updatedItems = new List<T>();
        }

        public IEnumerable<T> UnchangedItems => _unchangedItems.AsEnumerable();
        public IEnumerable<T> AddedItems => _addedItems.AsEnumerable();
        public IEnumerable<T> RemovedItems => _removedItems.AsEnumerable();
        public IEnumerable<T> UpdatedItems => _updatedItems.AsEnumerable();

        public void AddUnchangedItem(T item)
        {
            _unchangedItems.Add(item);
        }
        public void AddAddedItem(T item)
        {
            _addedItems.Add(item);
        }
        public void AddRemovedItem(T item)
        {
            _removedItems.Add(item);
        }
        public void AddUpdatedItem(T item)
        {
            _updatedItems.Add(item);
        }
    }
}