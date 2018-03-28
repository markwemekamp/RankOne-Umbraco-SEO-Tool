using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;

namespace RankOne.Tests.Models
{
    [TestClass]
    public class RankOneContextTest
    {
        [TestMethod]
        public void Instance_OnGet_ReturnsAnInstance()
        {
            var result = RankOneContext.Instance;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RankOneContext));
        }
    }
}