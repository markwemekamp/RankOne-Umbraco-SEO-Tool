using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Models.Settings;
using RankOne.Summaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Umbraco.Core.IO;

namespace RankOne.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public virtual string ConfigFileName
        {
            get
            {
                return "RankOne.Config";
            }
        }

        public virtual string ConfigFilePath
        {
            get
            {
                return IOHelper.MapPath(Path.Combine(SystemDirectories.Config, ConfigFileName));
            }
        }

        public IEnumerable<ISummary> GetSummaries()
        {
            var settings = ReadSettingsFromFile(ConfigFilePath);

            var summaries = new List<ISummary>();

            return settings.Summaries.Select(CreateSummary);
        }

        protected RankOneSettings ReadSettingsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Could not find file", filePath);

            var serializer = new XmlSerializer(typeof(RankOneSettings));

            var xml = File.ReadAllText(ConfigFilePath);
            using (var reader = new StringReader(xml))
            {
                return (RankOneSettings)serializer.Deserialize(reader);
            }
        }

        private ISummary CreateSummary(SummarySettings settings)
        {
            var summary = CreateSummaryObject(settings);
            if (summary != null)
            {
                summary.Analyzers = GetAnalyzers(settings.Analyzers);
            }
            return summary;
        }

        private ISummary CreateSummaryObject(SummarySettings settings)
        {
            if (string.IsNullOrEmpty(settings.Type))
            {
                return CreateBaseSummary(settings);
            }
            else
            {
                return CreateSummaryByType(settings);
            }
        }

        private ISummary CreateBaseSummary(SummarySettings settings)
        {
            return new BaseSummary()
            {
                Name = settings.Name,
                Alias = settings.Alias
            };
        }

        private ISummary CreateSummaryByType(SummarySettings settings)
        {
            var type = Type.GetType(settings.Type);
            if (type != null)
            {
                var summary = (ISummary)Activator.CreateInstance(type);
                summary.Name = settings.Name;
                summary.Alias = settings.Alias;
                return summary;
            }
            return null;
        }

        protected IEnumerable<IAnalyzer> GetAnalyzers(List<AnalyzerSettings> analyzerSettings)
        {
            return analyzerSettings.Select(CreateAnalyzer);
        }

        private IAnalyzer CreateAnalyzer(AnalyzerSettings settings)
        {
            var type = Type.GetType(settings.Type);
            if (type != null)
            {
                var analyzer = (IAnalyzer)Activator.CreateInstance(type);
                analyzer.Alias = settings.Alias;
                analyzer.Options = settings.Options.Select(x => new Option() { Key = x.Key, Value = x.Value });
                analyzer.Weight = settings.Weight ?? 100;
                return analyzer;
            }
            return null;
        }
    }
}