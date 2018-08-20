using System.Collections.Generic;

namespace IsItRaining.Services.Models
{
    public class WeatherResponse
    {
        public List<WeatherDay> Days { get; set; }
    }
}
