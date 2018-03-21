using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System;
using System.Linq;

namespace RankOne.Analyzers.Performance
{
    public class AdditionalCallAnalyzer : BaseAnalyzer
    {
        private readonly IOptionHelper _optionHelper;

        private int? _maximumAdditionalCalls;

        private int MaximumAdditionalCalls
        {
            get
            {
                if (!_maximumAdditionalCalls.HasValue)
                {
                    _maximumAdditionalCalls = _optionHelper.GetOptionValue(Options, "MaximumAdditionalCalls", 30);
                }
                return _maximumAdditionalCalls.Value;
            }
        }

        private int? _acceptableAdditionalCalls;

        private int AcceptableAdditionalCalls
        {
            get
            {
                if (!_acceptableAdditionalCalls.HasValue)
                {
                    _acceptableAdditionalCalls = _optionHelper.GetOptionValue(Options, "AcceptableAdditionalCalls", 15);
                }
                return _acceptableAdditionalCalls.Value;
            }
        }

        public AdditionalCallAnalyzer() : this(RankOneContext.Instance)
        { }

        public AdditionalCallAnalyzer(RankOneContext rankOneContext) : this(rankOneContext.OptionHelper.Value)
        { }

        public AdditionalCallAnalyzer(IOptionHelper optionHelper) : base()
        {
            if (optionHelper == null) throw new ArgumentNullException(nameof(optionHelper));

            _optionHelper = optionHelper;
        }

        public override void Analyse(IPageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            var cssFiles = pageData.Document.GetElementsWithAttribute("link", "href").Where(x => x.Attributes.Any(y => y.Name == "rel" && y.Value == "stylesheet"));
            var scriptFiles = pageData.Document.GetElementsWithAttribute("script", "src");
            var images = pageData.Document.GetElementsWithAttribute("img", "src");
            var objects = pageData.Document.GetElementsWithAttribute("object", "data");

            var total = cssFiles.Count() + scriptFiles.Count() + images.Count() + objects.Count();

            var resultRule = new ResultRule();

            if (total > MaximumAdditionalCalls)
            {
                resultRule.Alias = "more_than_maximum_calls";
                resultRule.Type = ResultType.Warning;
            }
            else if (total > AcceptableAdditionalCalls)
            {
                resultRule.Alias = "more_than_acceptable_calls";
                resultRule.Type = ResultType.Hint;
            }
            else
            {
                resultRule.Alias = "less_than_acceptable_calls";
                resultRule.Type = ResultType.Success;
            }

            resultRule.Tokens.Add(total.ToString());                        // 0
            resultRule.Tokens.Add(cssFiles.Count().ToString());             // 1
            resultRule.Tokens.Add(scriptFiles.Count().ToString());          // 2
            resultRule.Tokens.Add(images.Count().ToString());               // 3
            resultRule.Tokens.Add(objects.Count().ToString());              // 4
            resultRule.Tokens.Add(MaximumAdditionalCalls.ToString());       // 5
            resultRule.Tokens.Add(AcceptableAdditionalCalls.ToString());    // 6

            AddResultRule(resultRule);
        }
    }
}