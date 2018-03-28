using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Controllers;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Summaries;
using RankOne.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace RankOne.Tests.Controllers
{
    [TestClass]
    public class AnalyzerStructureApiControllerTest : BaseUmbracoControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_OnExecuteWithNull_ThrowsException()
        {
            new AnalyzerStructureApiController((IEnumerable<ISummary>)null);
        }

        [TestMethod]
        public void GetStructure_OnExecuteWithEmptyList_ReturnsOk()
        {
            var controller = new AnalyzerStructureApiController(new List<ISummary>());
            var result = controller.GetStructure();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<AnalyzerStructure>>));
            Assert.IsTrue(!((OkNegotiatedContentResult<List<AnalyzerStructure>>)result).Content.Any());
        }


        [TestMethod]
        public void GetStructure_OnExecuteWithFilledList_ReturnsOk()
        {
            var summaries = new List<ISummary>()
            {
                new BaseSummary()
                {
                    Alias = "test summary 1",
                    Analyzers = new List<IAnalyzer>()
                    {
                        new AnalyzerMock(){Alias = "test analyzer 1"},
                        new AnalyzerMock(){Alias = "test analyzer 2"},
                        new AnalyzerMock(){Alias = "test analyzer 3"}
                    }
                },
                new BaseSummary()
                {
                    Alias = "test summary 2",
                    Analyzers = new List<IAnalyzer>()
                    {
                        new AnalyzerMock(){Alias = "test analyzer 4"},
                        new AnalyzerMock(){Alias = "test analyzer 5"},
                        new AnalyzerMock(){Alias = "test analyzer 6"}
                    }
                },
            };

            var controller = new AnalyzerStructureApiController(summaries);
            var result = controller.GetStructure();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<AnalyzerStructure>>));
            Assert.AreEqual(2, ((OkNegotiatedContentResult<List<AnalyzerStructure>>)result).Content.Count());
        }
    }
}
