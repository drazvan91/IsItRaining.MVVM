using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace OpenWeatherMap.Api.Dtos
{
    public class Get5DaysForecastResponse
    {
        public string Cod { get; set; }
        public double Message { get; set; }
        public int Cantity { get; set; }
        public List<WeatherForecastItemDto> List { get; set; }
    }
}
