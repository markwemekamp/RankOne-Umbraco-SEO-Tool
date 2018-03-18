using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IOptionHelper
    {
        string GetOptionValue(IEnumerable<IOption> options, string name, string defaultValue);

        int GetOptionValue(IEnumerable<IOption> options, string name, int defaultValue);
    }
}