using RankOne.Interfaces;
using RankOne.Models.Settings;
using RankOne.Summaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Umbraco.Core.IO;

namespace RankOne.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private RankOneSettings ReadSettings()
        {
            var configFile = IOHelper.MapPath(Path.Combine(SystemDirectories.Config, "RankOne.Config"));

            if (File.Exists(configFile))
            {
                var serializer = new XmlSerializer(typeof(RankOneSettings));

                string xml = File.ReadAllText(configFile);
                using (var reader = new StringReader(xml))
                {
                    return (RankOneSettings)serializer.Deserialize(reader);
                }
            }
            return null;
        }

        public IEnumerable<ISummary> GetSummaries()
        {
            var settings = ReadSettings();

            var summaries = new List<ISummary>();

            foreach (var summarySetting in settings.Summaries)
            {
                ISummary summary = null;
                if (string.IsNullOrEmpty(summarySetting.Type))
                {
                    summary = new BaseSummary()
                    {
                        Name = summarySetting.Name,
                        Alias = summarySetting.Alias
                    };
                }
                else
                {
                    var type = Type.GetType(summarySetting.Type);
                    if (type != null)
                    {
                        summary = (ISummary)Activator.CreateInstance(type);
                        summary.Name = summarySetting.Name;
                        summary.Alias = summarySetting.Alias;
                    }
                }

                if (summary != null)
                {
                    summary.Analyzers = GetAnalyzers(summarySetting.Analyzers);
                    summaries.Add(summary);
                }
            }

            return summaries;
        }

        private IEnumerable<IAnalyzer> GetAnalyzers(List<AnalyzerSettings> analyzerSettings)
        {
            var analyzers = new List<IAnalyzer>();
            foreach (var analyzerSetting in analyzerSettings)
            {
                var type = Type.GetType(analyzerSetting.Type);
                if (type != null)
                {
                    var analyzer = (IAnalyzer)Activator.CreateInstance(type);
                    analyzer.Alias = analyzerSetting.Alias;

                    analyzers.Add(analyzer);
                }
            }

            return analyzers;
        }
    }
}