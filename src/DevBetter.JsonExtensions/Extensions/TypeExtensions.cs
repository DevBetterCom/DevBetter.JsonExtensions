using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevBetter.JsonExtensions.Extensions
{
  public static class TypeExtensions
  {
    public static object GetDefault(this Type type)
    {
      if (type.IsValueType)
      {
        return Activator.CreateInstance(type);
      }

      return null;
    }

    public static string[] GetPropertiesNames(this Type type)
    {
      var result = new List<string>();

      PropertyInfo[] propertyInfos;
      propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

      foreach (PropertyInfo propertyInfo in propertyInfos)
      {
        result.Add(propertyInfo.Name);
      }

      return result.ToArray();
    }

    public static Type GetTypeByName(this Type type, string propertyName)
    {
      foreach (PropertyInfo propertyInfo in type.GetProperties())
      {
        if (propertyInfo.Name == propertyName)
        {
          return propertyInfo.PropertyType;
        }
      }

      return null;
    }
  }
}
