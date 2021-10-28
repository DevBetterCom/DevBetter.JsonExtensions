using DevBetter.JsonExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevBetter.JsonExtensions.Converters
{
  public class MissingMemberConverter : JsonConverter<object>
  {
    public override bool CanConvert(Type typeToConvert)
    {
      if (string.IsNullOrEmpty(typeToConvert?.FullName))
      {
        return false;
      }
      if (typeToConvert.FullName.Contains("List"))
      {
        return true;
      }
      switch (Type.GetTypeCode(typeToConvert))
      {
        case TypeCode.Byte:
        case TypeCode.String:
        case TypeCode.DateTime:
        case TypeCode.SByte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Decimal:
        case TypeCode.Double:
        case TypeCode.Single:
          return true;
        default:
          return false;
      }
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      if (reader.TokenType == JsonTokenType.Null)
        return null;
      var readerClone = reader;
      using (var jsonDocument = JsonDocument.ParseValue(ref reader))
      {
        var jsonString = jsonDocument.RootElement.GetRawText();
        if (jsonString == "{}")
        {
          reader.TrySkip();
          return GetDefault(typeToConvert);
        }
      }

      return ExtractValue(ref readerClone, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, options);
    }

    private static object GetDefault(Type type)
    {
      if (type.IsValueType)
      {
        return Activator.CreateInstance(type);
      }
      return null;
    }

    private object ExtractValue(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      switch (reader.TokenType)
      {
        case JsonTokenType.String:
          if (typeToConvert == typeof(DateTime))
          {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
              return date;
            }else
            {
              return default(DateTime);
            }
          }          
          return reader.GetString();
        case JsonTokenType.False:
          return false;
        case JsonTokenType.True:
          return true;
        case JsonTokenType.Null:
          return null;
        case JsonTokenType.Number:
          return reader.GetValue(typeToConvert);
        case JsonTokenType.StartObject:
          return Read(ref reader, typeToConvert, options);
        case JsonTokenType.StartArray:
          if (typeToConvert.FullName.Contains("List`1[[System.String"))
          {
            var list = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
              list.Add((string)ExtractValue(ref reader, typeToConvert, options));
            }
            return list;
          }
          else
          {

            var list = new List<object>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
              object value = GetObjectValue(ref reader, typeToConvert.GenericTypeArguments[0], options);
              list.Add(value);
            }

            return ListCreator(list, typeToConvert.GenericTypeArguments[0]);
          }
        default:
          throw new JsonException($"'{reader.TokenType}' is not supported");
      }
    }

    static object ListCreator(List<object> listData, Type genericType)
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

    static object GetObjectValue(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      using (var jsonDocument = JsonDocument.ParseValue(ref reader))
      {
        var jsonString = jsonDocument.RootElement.GetRawText();
        return JsonSerializer.Deserialize(jsonString, typeToConvert, options);
      }
    }
  }
}

