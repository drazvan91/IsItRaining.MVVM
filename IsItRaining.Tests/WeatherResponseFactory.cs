using IsItRaining.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsItRaining.Tests
{
    public static class WeatherResponseFactory
    {
        static Random random = new Random();
        static string[] descriptions = new string[]
        {
            "clouds", "wind", "rain", "sunny"
        };

        static string[] images = new string[]
        {
            "img1", "img2", "img3"
        };

        public static WeatherResponse GenerateRandom(DateTime startDate, int numberOfDays)
        {
            var response = new WeatherResponse();
            response.Days = new List<WeatherDay>();

            for(int i = 0; i < numberOfDays; i++)
            {
                response.Days.Add(new WeatherDay()
                {
                    Date = startDate.AddDays(i),
                    MaxTemperature = random.Next(20, 50),
                    MinTemperature = random.Next(-10, 20),
                    Description = descriptions[random.Next(descriptions.Length)],
                    Image = images[random.Next(images.Length)]
                });
            }

            return response;
        }
    }
}
