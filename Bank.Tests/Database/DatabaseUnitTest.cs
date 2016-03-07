using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Bank.Database;
using NUnit.Framework;

namespace Bank.Tests.Database
{

    [TestFixture]
    public class DatabaseUnitTest
    {
        public DatabaseUnitTest()
        {
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void TestUserList()
        {
            Assert.AreEqual(2, new DatabaseMockup().GetUsers().Count());
        }

        [Test]
        public void TestAccountList()
        {
            Assert.AreEqual(8, new DatabaseMockup().GetAccounts().Count());
        }

        [Test]
        public void TestTransactionList()
        {
            Assert.AreEqual(8, new DatabaseMockup().GetTransactions().Count());
        }
    }
}
