using System;

namespace IsItRaining.Services.Models
{
    public class WeatherDay
    {
        public DateTime Date { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}