using Newtonsoft.Json;

namespace OpenWeatherMap.Api.Dtos
{
    public class MainSectionDto
    {
        [JsonProperty("temp_min")]
        public double MinTemperature { get; set; }

        [JsonProperty("temp_max")]
        public double MaxTemperature { get; set; }
    }
}