using Konves.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Konves.Collections.IntervalDictionary.Tests
{
    
    
    /// <summary>
    ///This is a test class for BoundTest and is intended
    ///to contain all BoundTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BoundTest
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

        
        [TestMethod()]
        public void MaxTest_DifferentValues()
        {
            IBound<int> a = new Bound<int>(2, BoundType.Inclusive);
            IBound<int> b = new Bound<int>(5, BoundType.Inclusive);
            IBound<int> expected = b;
            IBound<int> actual;
            actual = Bound<int>.Max(a, b);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxTest_SameValues()
        {
            IBound<int> a = new Bound<int>(5, BoundType.Exclusive);
            IBound<int> b = new Bound<int>(5, BoundType.Inclusive);
            IBound<int> expected = b;
            IBound<int> actual;
            actual = Bound<int>.Max(a, b);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MinTest_DifferentValues()
        {
            IBound<int> a = new Bound<int>(2, BoundType.Inclusive);
            IBound<int> b = new Bound<int>(5, BoundType.Inclusive);
            IBound<int> expected = a;
            IBound<int> actual;
            actual = Bound<int>.Min(a, b);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MinxTest_SameValues()
        {
            IBound<int> a = new Bound<int>(5, BoundType.Exclusive);
            IBound<int> b = new Bound<int>(5, BoundType.Inclusive);
            IBound<int> expected = b;
            IBound<int> actual;
            actual = Bound<int>.Min(a, b);
            Assert.AreEqual(expected, actual);
        }
    }
}
