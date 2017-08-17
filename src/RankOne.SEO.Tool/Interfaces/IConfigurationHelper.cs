using System.Collections.Generic;

namespace RankOne.Interfaces
{
    public interface IConfigurationHelper
    {
        IEnumerable<ISummary> GetSummaries();
    }
}
