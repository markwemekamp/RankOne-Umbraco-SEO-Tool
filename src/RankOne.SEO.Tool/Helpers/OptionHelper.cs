using RankOne.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class OptionHelper : IOptionHelper
    {
        public string GetOptionValue(IEnumerable<IOption> options, string name, string defaultValue)
        {
            var option = options.FirstOrDefault(x => x.Key == name);
            var optionValue = option.Value;
            if (string.IsNullOrEmpty(optionValue))
            {
                optionValue = defaultValue;
            }
            return optionValue;
        }

        public int GetOptionValue(IEnumerable<IOption> options, string name, int defaultValue)
        {
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