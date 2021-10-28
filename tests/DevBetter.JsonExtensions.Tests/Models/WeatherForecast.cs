using System;
using System.Collections.Generic;

namespace DevBetter.JsonExtensions.Tests.Models
{
    internal class WeatherForecast
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TemperatureC { get; set; }
        public DateTime CreatedDate { get; set; }
        public Country Country { get; set; }
        public List<string> Days { get; set; }
        public List<Other> Others { get; set; }
    }
}
