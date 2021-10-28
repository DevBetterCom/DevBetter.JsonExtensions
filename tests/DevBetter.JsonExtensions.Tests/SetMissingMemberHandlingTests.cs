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
    public void ReturnsObjectWhenEmptyObject()
    {
      var jsonString = @"{""Id"":{}, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Country"":{}, ""Days"": [""Sunday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Id.ShouldBe(0);
    }

    [Fact]
    public void ReturnsObjectWhenHasObject()
    {
      var jsonString = @"{""Id"":3, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Country"":{ ""Id"":5, ""Name"": ""Country""}, ""Days"": [""Sunday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Country.Id.ShouldBe(5);
    }

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
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": {}, ""Days"": [""Sunday"", ""Monday""]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.CreatedDate.ShouldBe(default(DateTime));
    }

    [Fact]
    public void ReturnsObjectWhenEmptyListString()
    {
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Days"": {}}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Days.ShouldBe(default(List<string>));
    }

    [Fact]
    public void ReturnsObjectWhenEmptyListObject()
    {
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""2021-01-01T18:25:43.511Z"", ""Others"": {}}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.Others.ShouldBe(default(List<Other>));
    }

    [Fact]
    public void ReturnsObjectWhenHasListObject()
    {
      var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": ""01/01/2021"", ""Others"": [{""Id"" : 2, ""Name"": ""Test""}, {""Id"" : 3, ""Name"": ""Test""}]}";
      var deserializeOptions = new JsonSerializerOptions()
          .SetMissingMemberHandling(MissingMemberHandling.Ignore);

      var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);

      weatherForecast.CreatedDate.ShouldBe(DateTime.Parse("01/01/2021"));
      weatherForecast.Others.Count.ShouldBe(2);
      weatherForecast.Others[0].Id.ShouldBe(2);
      weatherForecast.Others[0].Name.ShouldBe("Test");
      weatherForecast.Others[1].Id.ShouldBe(3);
      weatherForecast.Others[1].Name.ShouldBe("Test");
    }
  }
}

