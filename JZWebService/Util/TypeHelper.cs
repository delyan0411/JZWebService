using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Util
{
    public class TypeHelper
    {
        public static object ChangeType(Type targetType, object val)
        {
            if (val == null || (targetType.IsGenericType && val.ToString() == ""))
            {
                return null;
            }
            if (targetType == val.GetType() || targetType.IsGenericType)
            {
                return val;
            }
            if (targetType == typeof(bool))
            {
                if (val.ToString() == "0")
                {
                    return false;
                }
                if (val.ToString() == "1")
                {
                    return true;
                }
            }
            if (targetType.IsEnum)
            {
                int num = 0;
                if (!int.TryParse(val.ToString(), out num))
                {
                    return Enum.Parse(targetType, val.ToString());
                }
                return val;
            }
            else
            {
                if (targetType == typeof(Type))
                {
                    return ReflectionHelper.GetType(val.ToString());
                }
                return Convert.ChangeType(val, targetType);
            }
        }

        public static string GetClassSimpleName(Type t)
        {
            string[] expr_16 = t.ToString().Split(new char[]
			{
				'.'
			});
            return expr_16[expr_16.Length - 1].ToString();
        }

        public static string GetDefaultValue(Type destType)
        {
            if (TypeHelper.IsNumbericType(destType))
            {
                return "0";
            }
            if (destType == typeof(string))
            {
                return "\"\"";
            }
            if (destType == typeof(bool))
            {
                return "false";
            }
            if (destType == typeof(DateTime))
            {
                return "DateTime.Now";
            }
            if (destType == typeof(Guid))
            {
                return "System.Guid.NewGuid()";
            }
            if (destType == typeof(TimeSpan))
            {
                return "System.TimeSpan.Zero";
            }
            return "null";
        }

        public static Type GetTypeByRegularName(string regularName)
        {
            return ReflectionHelper.GetType(regularName);
        }

        public static string GetTypeRegularName(Type destType)
        {
            string arg = destType.Assembly.FullName.Split(new char[]
			{
				','
			})[0];
            return string.Format("{0},{1}", destType.ToString(), arg);
        }

        public static string GetTypeRegularNameOf(object obj)
        {
            return TypeHelper.GetTypeRegularName(obj.GetType());
        }

        public static bool IsFixLength(Type destDataType)
        {
            return TypeHelper.IsNumbericType(destDataType) || destDataType == typeof(byte[]) || destDataType == typeof(DateTime) || destDataType == typeof(bool);
        }

        public static bool IsNumbericType(Type destDataType)
        {
            return destDataType == typeof(int) || destDataType == typeof(uint) || destDataType == typeof(double) || destDataType == typeof(short) || destDataType == typeof(ushort) || destDataType == typeof(decimal) || destDataType == typeof(long) || destDataType == typeof(ulong) || destDataType == typeof(float) || destDataType == typeof(byte) || destDataType == typeof(sbyte);
        }

        public static bool IsSimpleType(Type t)
        {
            return TypeHelper.IsNumbericType(t) || t == typeof(char) || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime) || t == typeof(Type) || t.IsEnum;
        }

        public static object SqlNull(object obj)
        {
            if (obj == null || "".Equals(obj))
            {
                return DBNull.Value;
            }

            return obj;
        }
    }
}