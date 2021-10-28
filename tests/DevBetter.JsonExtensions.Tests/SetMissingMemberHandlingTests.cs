using DevBetter.JsonExtensions.Extensions;
using DevBetter.JsonExtensions.Tests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace DevBetter.JsonExtensions.Tests
{
  public class SetMissingMemberHandlingTests
  {
    [Fact]
    public void ReturnsObjectWhenEmptyInt()
    {
      var jsonString = @"{""Id"":{}, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Days"": [""Sunday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Id.ShouldBe(0);
    }

    [Fact]
    public void ReturnsObjectWhenEmptyString()
    {
      var jsonString = @"{""Id"":5, ""Title"": {},""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Days"": [""Sunday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Title.ShouldBe(null);
    }

    [Fact]
    public void ReturnsObjectWhenEmptyDateTime()
    {
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": {}, ""Days"": [""Sunday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.CreatedDate.ShouldBe(default(DateTime));
    }

    [Fact]
    public void ReturnsObjectWhenEmptyList()
    {
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Days"": {}}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Days.ShouldBe(default(List<string>));
    }
  }
}

