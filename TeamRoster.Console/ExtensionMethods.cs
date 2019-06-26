using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TeamRoster.App
{
    public static class ExtensionMethods
    {
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
        }

        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof(int))
                return "a number";
            else if (type == typeof(short))
                return "a number less than 32,767";
            else if (type == typeof(byte))
                return "a byte";
            else if (type == typeof(bool))
                return "either true or false";
            else if (type == typeof(long))
                return "a number";
            else if (type == typeof(float))
                return "a decimal";
            else if (type == typeof(double))
                return "a decimal";
            else if (type == typeof(decimal))
                return "a decimal";
            else if (type == typeof(string))
                return "text only";
            else if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray()) + ">";
            else
                return type.Name;
        }

        

    }
}
