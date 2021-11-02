using System;
using System.Collections.Generic;

namespace DevBetter.JsonExtensions.Extensions
{
  internal static class ListExtensions
  {
    public static object CreateGeneric(this List<object> listData, Type genericType)
    {
      Type genericListType = typeof(List<>);
      Type concreteListType = genericListType.MakeGenericType(genericType);

      Array values = Array.CreateInstance(genericType, listData.Count);
      for (int i = 0; i < listData.Count; i++)
      {
        values.SetValue(listData[i], i);
      }

      return Activator.CreateInstance(concreteListType, new object[] { values });
    }
  }
}
