using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Helpers;
using RankOne.Models;
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

            Assert.AreEqual("Test", testDatabaseObjectRepository.GetTableName());
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
