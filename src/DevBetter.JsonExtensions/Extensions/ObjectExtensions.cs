using System.Reflection;

namespace DevBetter.JsonExtensions.Extensions
{
  internal static class ObjectExtensions
  {
    public static object SetPropertyValueByName(this object obj, string propertyName, object value)
    {
      var propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

      if (propertyInfo == null)
      {
        return obj;
      }

      propertyInfo.SetValue(obj, value, null);

      return obj;
    }
  }
}
