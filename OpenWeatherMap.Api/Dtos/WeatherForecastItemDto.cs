using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenWeatherMap.Api.Dtos
{
    public class WeatherForecastItemDto
    {
        [JsonProperty("dt")]
        public int UnixDate { get; set; }

        public MainSectionDto Main { get; set; }
        public List<WeatherSectionDto> Weather {get;set;}

        [JsonProperty("dt_txt")]
        public string DateText { get; set; }
    }
}