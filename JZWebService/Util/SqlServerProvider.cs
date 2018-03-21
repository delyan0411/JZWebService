using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JZWebService.Util
{
    public class SqlServerProvider : IDbProvider
    {
        public string ParameterPrefix
        {
            get
            {
                return "@";
            }
        }

        public object ConvertToLocalDbType(Type t)
        {
            string key;
            switch (key = t.ToString())
            {
                case "System.Boolean":
                    return SqlDbType.Bit;
                case "System.DateTime":
                    return SqlDbType.DateTime;
                case "System.Decimal":
                    return SqlDbType.Decimal;
                case "System.Single":
                    return SqlDbType.Float;
                case "System.Double":
                    return SqlDbType.Float;
                case "System.Byte[]":
                    return SqlDbType.Image;
                case "System.Int64":
                    return SqlDbType.BigInt;
                case "System.Int32":
                    return SqlDbType.Int;
                case "System.String":
                    return SqlDbType.NVarChar;
                case "System.Int16":
                    return SqlDbType.SmallInt;
                case "System.Byte":
                    return SqlDbType.TinyInt;
                case "System.Guid":
                    return SqlDbType.UniqueIdentifier;
                case "System.TimeSpan":
                    return SqlDbType.Time;
                case "System.Object":
                    return SqlDbType.Variant;
            }
            return SqlDbType.Int;
        }

        public string ConvertToLocalDbTypeString(Type netType)
        {
            string key;
            switch (key = netType.ToString())
            {
                case "System.Boolean":
                    return "bit";
                case "System.DateTime":
                    return "datetime";
                case "System.Decimal":
                    return "decimal";
                case "System.Single":
                    return "float";
                case "System.Double":
                    return "float";
                case "System.Int64":
                    return "bigint";
                case "System.Int32":
                    return "int";
                case "System.String":
                    return "nvarchar";
                case "System.Int16":
                    return "smallint";
                case "System.Byte":
                    return "tinyint";
                case "System.Guid":
                    return "uniqueidentifier";
                case "System.TimeSpan":
                    return "time";
                case "System.Byte[]":
                    return "image";
                case "System.Object":
                    return "sql_variant";
            }
            return null;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if (cmd is SqlCommand)
            {
                SqlCommandBuilder.DeriveParameters(cmd as SqlCommand);
            }
        }

        public string GetLastIdSql()
        {
            return "SELECT SCOPE_IDENTITY()";
        }

        public DbProviderFactory Instance()
        {
            return SqlClientFactory.Instance;
        }

        public bool IsBackupDatabase()
        {
            return true;
        }

        public bool IsCompactDatabase()
        {
            return true;
        }

        public bool IsDbOptimize()
        {
            return true;
        }

        public bool IsFullTextSearchEnabled()
        {
            return true;
        }

        public bool IsShrinkData()
        {
            return true;
        }

        public bool IsStoreProc()
        {
            return true;
        }

        public DbParameter MakeParam(string paraName, object paraValue, ParameterDirection direction)
        {
            Type paraType = null;
            if (paraValue != null)
            {
                paraType = paraValue.GetType();
            }
            return this.MakeParam(paraName, paraValue, direction, paraType, null);
        }

        public DbParameter MakeParam(string paraName, object paraValue, ParameterDirection direction, Type paraType, string sourceColumn)
        {
            return this.MakeParam(paraName, paraValue, direction, paraType, sourceColumn, 0);
        }

        public DbParameter MakeParam(string paraName, object paraValue, ParameterDirection direction, Type paraType, string sourceColumn, int size)
        {
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = this.ParameterPrefix + paraName
            };
            if (paraType != null)
            {
                sqlParameter.SqlDbType = (SqlDbType)this.ConvertToLocalDbType(paraType);
            }
            sqlParameter.Value = paraValue;
            if (sqlParameter.Value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            sqlParameter.Direction = direction;
            if (direction != ParameterDirection.Output || paraValue != null)
            {
                sqlParameter.Value = paraValue;
            }
            if (direction == ParameterDirection.Output)
            {
                sqlParameter.Size = size;
            }
            if (sourceColumn != null)
            {
                sqlParameter.SourceColumn = sourceColumn;
            }
            return sqlParameter;
        }
    }
}