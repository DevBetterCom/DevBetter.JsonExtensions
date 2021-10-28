using System.Text.Json;
using DevBetter.JsonExtensions.Converters;

namespace DevBetter.JsonExtensions.Extensions
{
  public static class JsonSerializerOptionsExtensions
  {
    public static JsonSerializerOptions SetMissingMemberHandling(this JsonSerializerOptions jsonSerializerOptions,
      MissingMemberHandling missingMemberHandling)
    {
      if (missingMemberHandling == MissingMemberHandling.Ignore)
      {
        jsonSerializerOptions.Converters.Add(new MissingMemberConverter());
      }

      return jsonSerializerOptions;
    }
  }
}
