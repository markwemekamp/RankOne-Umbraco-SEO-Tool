using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class ResultRuleTest
    {
        [TestMethod]
        public void Constructor_OnExecute_InitializesCollections()
        {
            var resultRule = new ResultRule();

            Assert.IsNotNull(resultRule.Tokens);
        }

        [TestMethod]
        public void TokensProperty_OnSet_SetsTheValue()
        {
            var resultRule = new ResultRule();
            resultRule.Tokens = new List<string> { "token" };
            Assert.IsNotNull(resultRule.Tokens);
            Assert.AreEqual(1, resultRule.Tokens.Count());
        }

        [TestMethod]
        public void TokensProperty_OnSet_SetsTheValueToNull()
        {
            var resultRule = new ResultRule();
            resultRule.Tokens = null;
            Assert.IsNull(resultRule.Tokens);
        }

        [TestMethod]
        public void TypeProperty_OnSet_SetsTheValue()
        {
            var resultRule = new ResultRule();
            resultRule.Type = "type";
            Assert.AreEqual("type", resultRule.Type);
        }

        [TestMethod]
        public void TypeProperty_OnSet_SetsTheValueToNull()
        {
            var resultRule = new ResultRule();
            resultRule.Type = null;
            Assert.IsNull(resultRule.Type);
        }

        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValue()
        {
            var resultRule = new ResultRule();
            resultRule.Alias = "alias";
            Assert.AreEqual("alias", resultRule.Alias);
        }

        [TestMethod]
        public void AliasProperty_OnSet_SetsTheValueToNull()
        {
            var resultRule = new ResultRule();
            resultRule.Alias = null;
            Assert.IsNull(resultRule.Alias);
        }
    }
}
