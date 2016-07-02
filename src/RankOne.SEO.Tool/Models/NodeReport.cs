using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RankOne.Models
{
    [TableName("NodeReport")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class NodeReport : BaseDatabaseObject
    {
        [PrimaryKeyColumn(AutoIncrement = false)]
        public int Id { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string FocusKeyword { get; set; }

        public DateTime CreatedOn { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? UpdatedOn { get; set; }
        
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Report { get; set; }
    }
}