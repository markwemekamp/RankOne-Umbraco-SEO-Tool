using RankOne.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class OptionHelper : IOptionHelper
    {
        public string GetOptionValue(IEnumerable<IOption> options, string name, string defaultValue)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            var option = options.FirstOrDefault(x => x.Key == name);
            var optionValue = option.Value;
            if (string.IsNullOrEmpty(optionValue))
            {
                optionValue = defaultValue;
            }
            return optionValue;
        }

        public int GetOptionValue(IEnumerable<IOption> options, string name, int defaultValue = 0)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (defaultValue < 0) throw new ArgumentException(nameof(defaultValue));

            var option = options.FirstOrDefault(x => x.Key == name);
            var optionValue = 0;
            if (option != null)
            {
                int.TryParse(option.Value, out optionValue);
            }
            return optionValue > 0 ? optionValue : defaultValue;
        }
    }
}