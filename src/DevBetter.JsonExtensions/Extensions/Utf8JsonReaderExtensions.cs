using System;
using System.Text.Json;

namespace DevBetter.JsonExtensions.Extensions
{
  public static class Utf8JsonReaderExtensions
  {
    public static object GetValue(this Utf8JsonReader reader, Type type)
    {
      if (!type.IsValueType)
      {
        return null;
      }

      if (type == typeof(string))
      {
        if (reader.TokenType == JsonTokenType.None || reader.TokenType == JsonTokenType.Null)
        {
          return null;
        }

        return reader.GetString();
      }
      else if (type == typeof(int) || type == typeof(Int32))
      {
        if (reader.TryGetInt32(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(Int16))
      {
        if (reader.TryGetInt16(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(Int64))
      {
        if (reader.TryGetInt64(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(uint) || type == typeof(UInt32))
      {
        if (reader.TryGetUInt32(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(UInt16))
      {
        if (reader.TryGetUInt16(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(UInt64))
      {
        if (reader.TryGetUInt64(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(double) || type == typeof(Double))
      {
        if (reader.TryGetDouble(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(decimal) || type == typeof(Decimal))
      {
        if (reader.TryGetDecimal(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(float))
      {
        if (reader.TryGetDecimal(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(byte) || type == typeof(Byte) || type == typeof(char) || type == typeof(Char))
      {
        if (reader.TryGetByte(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(DateTime))
      {
        if (reader.TryGetDateTime(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(DateTimeOffset))
      {
        if (reader.TryGetDateTimeOffset(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(Guid))
      {
        if (reader.TryGetGuid(out var value))
        {
          return value;
        }

        return type.GetDefault();
      }
      else if (type == typeof(bool) || type == typeof(Boolean))
      {
        if (reader.TokenType == JsonTokenType.True)
        {
          return true;
        }
        else if (reader.TokenType == JsonTokenType.False)
        {
          return false;
        }
        else
        {
          return type.GetDefault();
        }
      }

      return null;
    }
  }
}
