[![NuGet](https://img.shields.io/nuget/v/DevBetter.JsonExtensions.svg)](https://www.nuget.org/packages/DevBetter.JsonExtensions)[![NuGet](https://img.shields.io/nuget/dt/DevBetter.JsonExtensions.svg)](https://www.nuget.org/packages/DevBetter.JsonExtensions)

# DevBetter.JsonExtensions

Some extensions to add more features to text.json.  


## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Credits

Big thanks to [all of the great contributors to this project](https://github.com/DevBetterCom/DevBetter.JsonExtensions/graphs/contributors)!

## Getting Started

Install [DevBetter.JsonExtensions](https://github.com/DevBetterCom/DevBetter.JsonExtensions) from Nuget using:

(in Visual Studio)

```powershell
Install-Package DevBetter.JsonExtensions
```

(using the dotnet cli)

```powershell
dotnet add package DevBetter.JsonExtensions
```

### SetMissingMemberHandling

Will help to not go to exception when the property set to {} in json.  
You can enable it by calling `SetMissingMemberHandling`.  
```csharp
var jsonString = @"{""Id"":5, ""Title"": ""Title"",""TemperatureC"": 28,""CreatedDate"": {}, ""Days"": [""Sunday""]}";
var deserializeOptions = new JsonSerializerOptions()
    .SetMissingMemberHandling(MissingMemberHandling.Ignore);

var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, deserializeOptions);
```

Please check the unit tests for more examples [here](https://github.com/DevBetterCom/DevBetter.JsonExtensions/tree/main/tests/DevBetter.JsonExtensions.Tests)
