using DevBetter.JsonExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevBetter.JsonExtensions.Converters
{
  public class MissingMemberErrorConverter : JsonConverter<object>
  {
    public override bool CanConvert(Type typeToConvert)
    {
      if (typeToConvert.FullName.Contains("List"))
      {
        return true;
      }

      switch (Type.GetTypeCode(typeToConvert))
      {
        case TypeCode.Object:        
          return true;
        default:
          return false;
      }
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      if (reader.TokenType == JsonTokenType.Null)
        return null;

      if (reader.TokenType == JsonTokenType.StartArray)
      {
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
            object value = DeserializeValue(ref reader, typeToConvert.GenericTypeArguments[0], options);
            list.Add(value);
          }

          return list.CreateGeneric(typeToConvert.GenericTypeArguments[0]);
        }
      }
      

      var jsonString = string.Empty;
      var readerClone = reader;
      var objectPropertiesNames = typeToConvert.GetPropertiesNames();

      using (var jsonDocument = JsonDocument.ParseValue(ref reader))
      {
        jsonString = jsonDocument.RootElement.GetRawText();
        if (jsonString == "{}")
        {
          return JsonSerializer.Deserialize(jsonString, typeToConvert, options);
        }
        else
        {
          var jsonPropertiesNames = jsonDocument.RootElement
            .EnumerateObject()
            .Select(p => p.Name)
            .ToArray();
          
          for (var i = 0; i < jsonPropertiesNames.Length; i++)
          {
            bool isFound = false;
            var jsonProperty = jsonPropertiesNames[i];
            for (var j = 0; j < objectPropertiesNames.Length; j++)
            {
              var objectProperty = objectPropertiesNames[j];
              if (jsonProperty.ToLower() == objectProperty.ToLower())
              {
                isFound = true;
                break;
              }
            }
            if (!isFound)
            {
              throw new KeyNotFoundException($"Property {jsonProperty} not found!");
            }
          }
        }
      }

      var obj = Activator.CreateInstance(typeToConvert);
      for (var i = 0; (i < objectPropertiesNames.Length); i++)
      {
        var propertyName = objectPropertiesNames[i];
        using (var jsonDocument = JsonDocument.Parse(jsonString))
        {
          JsonElement root = jsonDocument.RootElement;
          if (!root.TryGetProperty(propertyName, out var element))
          {
            continue;
          }
          var type = typeToConvert.GetTypeByName(propertyName);
          if (type.FullName.Contains("List`1") && !type.FullName.Contains("String"))
          {
            var tempList = new List<object>();
            var tempListGeneric = tempList.CreateGeneric(type.GenericTypeArguments[0]);

            var arrayValued = JsonSerializer.Deserialize(element.GetRawText(), tempListGeneric.GetType(), options);
            obj.SetPropertyValueByName(propertyName, arrayValued);
          }
          else {
            var value = JsonSerializer.Deserialize(element.GetRawText(), type, options);
            obj.SetPropertyValueByName(propertyName, value);
          }          
        }
      }
      
      return obj;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, options);
    }

    private object ExtractValue(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      switch (reader.TokenType)
      {
        case JsonTokenType.String:
          return reader.GetString();
        case JsonTokenType.False:
          return false;
        case JsonTokenType.True:
          return true;
        case JsonTokenType.Null:
          return null;
        case JsonTokenType.Number:
          return Read(ref reader, typeToConvert, options);
        case JsonTokenType.StartObject:
          return Read(ref reader, typeToConvert, options);
        case JsonTokenType.StartArray:
          return Read(ref reader, typeToConvert, options);
        default:
          throw new JsonException($"'{reader.TokenType}' is not supported");
      }
    }

    static object DeserializeValue(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      using (var jsonDocument = JsonDocument.ParseValue(ref reader))
      {
        var jsonString = jsonDocument.RootElement.GetRawText();
        return JsonSerializer.Deserialize(jsonString, typeToConvert, options);
      }
    }
  }
}

