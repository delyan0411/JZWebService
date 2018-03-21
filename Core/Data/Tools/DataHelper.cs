using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Core.Data
{
    public class DataHelper
    {
        public class PropertyNotFoundException : Exception
        {
            private string targetPropertyName;

            public string TargetPropertyName
            {
                get
                {
                    return this.targetPropertyName;
                }
                set
                {
                    this.targetPropertyName = value;
                }
            }

            public PropertyNotFoundException()
            {
            }

            public PropertyNotFoundException(string propertyName)
                : base(string.Format("The property named '{0}' not found in Entity definition.", propertyName))
            {
                this.targetPropertyName = propertyName;
            }
        }

        public static IList<TEntity> ConvertDataTableToObjects<TEntity>(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            IList<TEntity> list = new List<TEntity>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(DataHelper.ConvertRowToObject<TEntity>(row));
            }
            return list;
        }

        public static TEntity ConvertRowToObject<TEntity>(DataRow row)
        {
            if (row == null)
            {
                return default(TEntity);
            }
            return (TEntity)((object)DataHelper.ConvertRowToObject(typeof(TEntity), row));
        }

        public static object ConvertRowToObject(Type objType, DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            DataTable dt = row.Table;
            object obj = Activator.CreateInstance(objType);
            foreach (DataColumn dataColumn in dt.Columns)
            {
                PropertyInfo property = objType.GetProperty(dataColumn.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (property != null)
                {
                    Type propertyType = property.PropertyType;
                    object obj2 = null;
                    bool flag = true;
                    try
                    {
                        obj2 = TypeHelper.ChangeType(propertyType, row[dataColumn.ColumnName]);
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        object[] args = new object[]
						{
							obj2
						};
                        objType.InvokeMember(dataColumn.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, null, obj, args);
                    }
                }
            }
            return obj;
        }

        public static IList<string> DistillCommandParameter(string sqlStatement, string paraPrefix)
        {
            sqlStatement += " ";
            IList<string> list = new List<string>();
            DataHelper.DoDistill(sqlStatement, list, paraPrefix);
            if (list.Count > 0)
            {
                string text = list[list.Count - 1].Trim();
                if (text.EndsWith("\""))
                {
                    text.TrimEnd(new char[]
					{
						'"'
					});
                    list.RemoveAt(list.Count - 1);
                    list.Add(text);
                }
            }
            return list;
        }

        private static void DoDistill(string sqlStatement, IList<string> paraList, string paraPrefix)
        {
            sqlStatement.TrimStart(new char[]
			{
				' '
			});
            int num = sqlStatement.IndexOf(paraPrefix);
            if (num >= 0)
            {
                int num2 = sqlStatement.IndexOf(" ", num);
                int length = num2 - num;
                string text = sqlStatement.Substring(num, length);
                paraList.Add(text.Replace(paraPrefix, ""));
                DataHelper.DoDistill(sqlStatement.Substring(num2), paraList, paraPrefix);
            }
        }

        public static void FillCommandParameterValue(IDbCommand command, object entityOrRow)
        {
            foreach (IDbDataParameter dbDataParameter in command.Parameters)
            {
                dbDataParameter.Value = DataHelper.GetColumnValue(entityOrRow, dbDataParameter.SourceColumn);
                if (dbDataParameter.Value == null)
                {
                    dbDataParameter.Value = DBNull.Value;
                }
            }
        }

        public static object GetColumnValue(object entityOrRow, string columnName)
        {
            DataRow dataRow = entityOrRow as DataRow;
            if (dataRow != null)
            {
                return dataRow[columnName];
            }
            return entityOrRow.GetType().InvokeMember(columnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, entityOrRow, null);
        }

        public static object GetSafeDbValue(object val)
        {
            if (val != null)
            {
                return val;
            }
            return DBNull.Value;
        }

        public static void RefreshEntityFields(object entity, DataRow row)
        {
            DataTable arg_0C_0 = row.Table;
            IList<string> list = new List<string>();
            foreach (DataColumn dataColumn in arg_0C_0.Columns)
            {
                list.Add(dataColumn.ColumnName);
            }
            DataHelper.RefreshEntityFields(entity, row, list);
        }

        public static void RefreshEntityFields(object entity, DataRow row, IList<string> refreshFields)
        {
            Type type = entity.GetType();
            foreach (string current in refreshFields)
            {
                PropertyInfo expr_20 = type.GetProperty(current, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (expr_20 == null)
                {
                    throw new DataHelper.PropertyNotFoundException(current);
                }
                Type propertyType = expr_20.PropertyType;
                object obj = null;
                bool flag = true;
                try
                {
                    obj = TypeHelper.ChangeType(propertyType, row[current]);
                }
                catch
                {
                    flag = false;
                }
                if (flag)
                {
                    object[] args = new object[]
					{
						obj
					};
                    type.InvokeMember(current, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, null, entity, args);
                }
            }
        }
    }
}