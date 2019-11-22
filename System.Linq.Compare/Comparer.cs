using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Compare
{
    public class Comparer<T> where T : class
    {
        public MemberSelector<T> IdentifyingMembers { get; } = new MemberSelector<T>();

        public MemberSelector<T> MembersToCompare { get; } = new MemberSelector<T>();

        public CompareResult<T> Compare(IEnumerable<T> originalItems, IEnumerable<T> newItems)
        {
            var result = new CompareResult<T>();

            ValidateComparison(originalItems, newItems);

            var useIdentifyingMembers = IdentifyingMembers.Members.Any();
            var identifyingMembers = useIdentifyingMembers ? IdentifyingMembers : MembersToCompare;

            var handledDuplicates = new IndexedItemList<T>(identifyingMembers);
            var indexedOriginalItems = new IndexedItemList<T>(originalItems, identifyingMembers);
            var indexedNewItems = new IndexedItemList<T>(newItems, identifyingMembers);

            foreach (var originalItem in originalItems)
            {
                // is duplicate of an already handled item?
                if (handledDuplicates.GetItemsWithIdenticalIdentity(originalItem).Any())
                {
                    continue;
                }

                // get all duplicates
                var sourceItems = indexedOriginalItems.GetItemsWithIdenticalIdentity(originalItem).ToList();
                if (sourceItems.Count > 1)
                {
                    handledDuplicates.Add(originalItem);
                }

                var targetItems = indexedNewItems.GetItemsWithIdenticalIdentity(originalItem).ToList();

                for (int i = 0; i < Math.Max(sourceItems.Count, targetItems.Count); i++)
                {
                    var sourceItemCurrent = i < sourceItems.Count ? sourceItems[i] : null;
                    var targetItemCurrent = i < targetItems.Count ? targetItems[i] : null;

                    if (sourceItemCurrent == null)
                    {
                        result.AddAddedItem(targetItemCurrent);
                    }
                    else if (targetItemCurrent == null)
                    {
                        result.AddRemovedItem(sourceItemCurrent);
                    }
                    else
                    {
                        // Is item unchanged? (always true if not using identifying members, because target is already found using MembersToCompare)
                        if (!useIdentifyingMembers || MembersToCompare.Equal(sourceItemCurrent, targetItemCurrent))
                        {
                            result.AddUnchangedItem(sourceItemCurrent);
                        }
                        else
                        {
                            result.AddUpdatedItem(targetItemCurrent);
                        }
                    }
                }
            }

            // iterate source items
            foreach (var newItem in newItems)
            {
                var sourceItemExists = indexedOriginalItems.GetItemsWithIdenticalIdentity(newItem).Any();
                if (!sourceItemExists)
                {
                    result.AddAddedItem(newItem);
                }
            }

            return result;
        }


        private class EqualityComparer : IEqualityComparer<T>
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

        private void ValidateComparison(IEnumerable<T> originalItems, IEnumerable<T> newItems)
        {
            if (originalItems == null)
            {
                throw new ArgumentNullException(nameof(originalItems));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            if (!MembersToCompare.Members.Any())
            {
                throw new NotImplementedException("MembersToCompare should contain at least 1 member");
            }
        }

    }
}
