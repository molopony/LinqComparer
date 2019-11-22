using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace System.Linq.Compare.UnitTest
{
    [TestClass]
    public class ComparerTest
    {
        [TestMethod]
        public void TestCompareWithAddedItem()
        {
            var originalList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
            };

            var comparisonList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
                new TestClass1
                {
                    MyInt = 2,
                    MyString = "String B",
                },
            };

            var result = originalList.Compare(comparisonList);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.UnchangedItems.Count());
            Assert.AreEqual(1, result.AddedItems.Count());
            Assert.AreEqual(0, result.RemovedItems.Count());
            Assert.AreEqual(0, result.UpdatedItems.Count());
        }

        [TestMethod]
        public void TestCompareWithRemovedItem()
        {
            var originalList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
                new TestClass1
                {
                    MyInt = 2,
                    MyString = "String B",
                },
            };

            var comparisonList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
            };


            var result = originalList.Compare(comparisonList);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.UnchangedItems.Count());
            Assert.AreEqual(0, result.AddedItems.Count());
            Assert.AreEqual(1, result.RemovedItems.Count());
            Assert.AreEqual(0, result.UpdatedItems.Count());
        }

        [TestMethod]
        public void TestCompareWithIdentifyingMember()
        {
            var originalList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
                new TestClass1
                {
                    MyInt = 2,
                    MyString = "String B",
                },
                new TestClass1
                {
                    MyInt = 3,
                    MyString = "String C",
                },
            };

            var comparisonList = new List<TestClass1>
            {
                new TestClass1
                {
                    MyInt = 1,
                    MyString = "String A",
                },
                new TestClass1
                {
                    MyInt = 3,
                    MyString = "String C2",
                },
                new TestClass1
                {
                    MyInt = 4,
                    MyString = "String D",
                },
            };

            var comparer = new Comparer<TestClass1>();
            comparer.MembersToCompare.Add(c => c.MyString);
            comparer.IdentifyingMembers.Add(c => c.MyInt);
            var result = comparer.Compare(originalList, comparisonList);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.UnchangedItems.Count());
            Assert.AreEqual(1, result.AddedItems.Count());
            Assert.AreEqual(1, result.RemovedItems.Count());
            Assert.AreEqual(1, result.UpdatedItems.Count());
        }

        public class TestClass1
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }
    }
}
