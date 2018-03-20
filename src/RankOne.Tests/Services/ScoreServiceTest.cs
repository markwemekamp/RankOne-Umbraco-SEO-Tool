using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankOne.Models;
using RankOne.Services;
using System;
using System.Collections.Generic;

namespace RankOne.Tests.Services
{
    [TestClass]
    public class ScoreServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetScore_OnExecuteWithNullParameter_ThrowsException()
        {
            var scoreService = new ScoreService();
            scoreService.GetScore(null);
        }

        [TestMethod]
        public void GetScore_OnExecuteWithErrors_SetsOverallScoreTo0()
        {
            var scoreService = new ScoreService();
            var pageAnalysis = new PageAnalysis()
            {
                SummaryResults =
                {
                    new SummaryResult()
                    {
                        Analysis = new Analysis()
                        {
                            Results = new List<AnalyzeResult>()
                            {
                                new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        }
                                    }
                                }, new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new SummaryResult()
                    {
                        Analysis = new Analysis()
                        {
                            Results = new List<AnalyzeResult>()
                            {
                                new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        }
                                    }
                                }, new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Error
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = scoreService.GetScore(pageAnalysis);
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.ErrorCount);
            Assert.AreEqual(0, result.WarningCount);
            Assert.AreEqual(0, result.HintCount);
            Assert.AreEqual(0, result.SuccessCount);
            Assert.AreEqual(0, result.OverallScore);
        }

        [TestMethod]
        public void GetScore_OnExecuteWith1Warning_SetsOverallScoreTo50()
        {
            var scoreService = new ScoreService();
            var pageAnalysis = new PageAnalysis()
            {
                SummaryResults =
                {
                    new SummaryResult()
                    {
                        Analysis = new Analysis()
                        {
                            Results = new List<AnalyzeResult>()
                            {
                                new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Warning
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = scoreService.GetScore(pageAnalysis);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCount);
            Assert.AreEqual(1, result.WarningCount);
            Assert.AreEqual(0, result.HintCount);
            Assert.AreEqual(0, result.SuccessCount);
            Assert.AreEqual(50, result.OverallScore);

        }

        [TestMethod]
        public void GetScore_OnExecuteWith1Hint_SetsOverallScoreTo75()
        {
            var scoreService = new ScoreService();
            var pageAnalysis = new PageAnalysis()
            {
                SummaryResults =
                {
                    new SummaryResult()
                    {
                        Analysis = new Analysis()
                        {
                            Results = new List<AnalyzeResult>()
                            {
                                new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = scoreService.GetScore(pageAnalysis);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCount);
            Assert.AreEqual(0, result.WarningCount);
            Assert.AreEqual(1, result.HintCount);
            Assert.AreEqual(0, result.SuccessCount);
            Assert.AreEqual(75, result.OverallScore);

        }

        [TestMethod]
        public void GetScore_OnExecuteWith5Hints_SetsOverallScoreTo0()
        {
            var scoreService = new ScoreService();
            var pageAnalysis = new PageAnalysis()
            {
                SummaryResults =
                {
                    new SummaryResult()
                    {
                        Analysis = new Analysis()
                        {
                            Results = new List<AnalyzeResult>()
                            {
                                new AnalyzeResult()
                                {
                                    ResultRules = new List<ResultRule>()
                                    {
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        },
                                        new ResultRule()
                                        {
                                            Type = ResultType.Hint
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = scoreService.GetScore(pageAnalysis);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorCount);
            Assert.AreEqual(0, result.WarningCount);
            Assert.AreEqual(5, result.HintCount);
            Assert.AreEqual(0, result.SuccessCount);
            Assert.AreEqual(0, result.OverallScore);

        }
    }
}
