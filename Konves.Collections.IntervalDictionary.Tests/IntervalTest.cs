using Konves.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Konves.Collections.IntervalDictionary.Tests
{
    
    
    /// <summary>
    ///This is a test class for IntervalTest and is intended
    ///to contain all IntervalTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IntervalTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        #region == ContainsTest_Key ==
        [TestMethod()]
        public void ContainsTest_Key_Mid()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 4;
            bool expected = true;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_TooLow()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 1;
            bool expected = false;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_TooHigh()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 7;
            bool expected = false;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_OnLowerInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 2;
            bool expected = true;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_OnLowerExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Exclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 2;
            bool expected = false;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_OnUpperInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 5;
            bool expected = true;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Key_OnUpperExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Exclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            int key = 5;
            bool expected = false;
            bool actual;
            actual = target.Contains(key);
            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region == ContainsTest_Bound ==
        [TestMethod()]
        public void ContainsTest_Bound_Mid()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(4, BoundType.Inclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_TooLow()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(1, BoundType.Inclusive);
            bool expected = false;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_TooHigh()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(7, BoundType.Inclusive);
            bool expected = false;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_InclusiveOnLowerInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(2, BoundType.Inclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_InclusiveOnLowerExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Exclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(2, BoundType.Inclusive);
            bool expected = false;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_ExclusiveOnLowerInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(2, BoundType.Exclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_ExclusiveOnLowerExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Exclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(2, BoundType.Exclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_InclusiveOnUpperInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(5, BoundType.Inclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_InclusiveOnUpperExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Exclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(5, BoundType.Inclusive);
            bool expected = false;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_ExclusiveOnUpperInclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Inclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(5, BoundType.Exclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ContainsTest_Bound_ExclusiveOnUpperExclusive()
        {
            int lowerBound = 2;
            BoundType lowerBoundType = BoundType.Inclusive;
            int upperBound = 5;
            BoundType upperBoundType = BoundType.Exclusive;
            Interval<int> target = new Interval<int>(lowerBound, lowerBoundType, upperBound, upperBoundType);
            IBound<int> bound = new Bound<int>(5, BoundType.Exclusive);
            bool expected = true;
            bool actual;
            actual = target.Contains(bound);
            Assert.AreEqual(expected, actual);
        } 
        #endregion
    }
}
