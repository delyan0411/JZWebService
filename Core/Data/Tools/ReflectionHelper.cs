using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Core.Data
{
    public class ReflectionHelper
    {
        public static Type GetType(string typeAndAssName)
        {
            string[] array = typeAndAssName.Split(new char[]
			{
				','
			});
            if (array.Length < 2)
            {
                return Type.GetType(typeAndAssName);
            }
            return ReflectionHelper.GetType(array[0].Trim(), array[1].Trim());
        }

        public static Type GetType(string typeFullName, string assemblyName)
        {
            if (assemblyName == null)
            {
                return Type.GetType(typeFullName);
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (assembly.FullName.Split(new char[]
				{
					','
				})[0].Trim() == assemblyName.Trim())
                {
                    return assembly.GetType(typeFullName);
                }
            }
            Assembly assembly2 = Assembly.Load(assemblyName);
            if (assembly2 != null)
            {
                return assembly2.GetType(typeFullName);
            }
            return null;
        }

        public static object GetPropertyValue(object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }
    }
}