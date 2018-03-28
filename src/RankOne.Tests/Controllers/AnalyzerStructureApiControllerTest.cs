using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Controllers;
using RankOne.Interfaces;
using System;
using System.Collections.Generic;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class AnalyzerStructureApiControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNull_ThrowsException()
        {
            new AnalyzerStructureApiController((IEnumerable<ISummary>)null);
        }
    }
}
