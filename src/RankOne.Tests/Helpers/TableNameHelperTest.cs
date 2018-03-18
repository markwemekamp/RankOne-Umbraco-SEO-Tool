using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using RankOne.Models;
using System;
using Umbraco.Core.Persistence;

namespace RankOne.Tests.Helpers
{
    [TestClass]
    public class TableNameHelperTest
    {
        [TestMethod]
        public void GetTableName_OnExecute_IsCorreclyResolvedWhenTableNameAttributeIsSet()
        {
            var testDatabaseObjectRepository = new TableNameHelper<TestDatabaseObject>();
            var result = testDatabaseObjectRepository.GetTableName();

            Assert.AreEqual("Test", result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetTableName_OnExecute_ThrowsExceptionWhenNotSet()
        {
            var testDatabaseObjectRepository = new TableNameHelper<TestDatabaseObjectWithoutTableName>();

            testDatabaseObjectRepository.GetTableName();
        }

        [TableName("Test")]
        public class TestDatabaseObject : BaseDatabaseObject
        { }

        public class TestDatabaseObjectWithoutTableName : BaseDatabaseObject
        { }
    }
}