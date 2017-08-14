using RankOne.Interfaces;
using RankOne.Models;
using System;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace RankOne.Helpers
{
    public class TableNameHelper<T> : ITableNameHelper<T> where T : BaseDatabaseObject
    {
        public string GetTableName()
        {
            var type = typeof(T);
            var tableNameAttribute = type.FirstAttribute<TableNameAttribute>();
            if (tableNameAttribute == null)
                throw new Exception(
                    string.Format(
                        "The Type '{0}' does not contain a TableNameAttribute, which is used to find the name of the table to drop. The operation could not be completed.",
                        type.Name));
            return tableNameAttribute.Value;
        }
    }
}