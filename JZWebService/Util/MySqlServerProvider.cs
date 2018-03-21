using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace JZWebService.Util
{
    public class MySqlServerProvider : IDbProvider
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
                    return MySqlDbType.Bit;
                case "System.DateTime":
                    return MySqlDbType.DateTime;
                case "System.Decimal":
                    return MySqlDbType.Decimal;
                case "System.Single":
                    return MySqlDbType.Float;
                case "System.Double":
                    return MySqlDbType.Float;
                case "System.Byte[]":
                    return MySqlDbType.Binary;
                case "System.Int64":
                    return MySqlDbType.Int64;
                case "System.Int32":
                    return MySqlDbType.Int32;
                case "System.String":
                    return MySqlDbType.VarChar;
                case "System.Byte":
                    return MySqlDbType.Byte;
                case "System.TimeSpan":
                    return MySqlDbType.Time;
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
                    return "varchar";
                case "System.Int16":
                    return "smallint";
                case "System.Byte":
                    return "tinyint";
                case "System.TimeSpan":
                    return "time";
            }
            return null;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if (cmd is MySqlCommand)
            {
                MySqlCommandBuilder.DeriveParameters(cmd as MySqlCommand);
            }
        }

        public string GetLastIdSql()
        {
            return "SELECT LAST_INSERT_ID()";
        }

        public DbProviderFactory Instance()
        {
            return MySqlClientFactory.Instance;
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
            MySqlParameter sqlParameter = new MySqlParameter
            {
                ParameterName = this.ParameterPrefix + paraName
            };
            if (paraType != null)
            {
                sqlParameter.MySqlDbType = (MySqlDbType)this.ConvertToLocalDbType(paraType);
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