using OpenWeatherMap.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsItRaining.Services.Models;
using OpenWeatherMap.Api.Dtos;

namespace IsItRaining.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private OpenWeatherMapClient client;

        public OpenWeatherMapService()
        {
            this.client = new OpenWeatherMapClient("f426d5672979d3151d56d51c4fc93245");
        }

        public async Task<WeatherResponse> GetWeatherAsync(DateTime startDate, int numberOfDays, double latitude, double longitude)
        {
            var response = await client.Get5DaysForecastAsync(latitude, longitude);

            var dailyForcast = MapForecast(startDate, numberOfDays, response).ToList();
            return new WeatherResponse()
            {
                Days = dailyForcast
            };
        }

        Random random = new Random();

        private IEnumerable<WeatherDay> MapForecast(DateTime startDate, int numberOfDays, Get5DaysForecastResponse response)
        {

            for (int i = 0; i < numberOfDays; i++)
            {
                var unixStart =(Int32)(startDate.AddDays(i).ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var unixEnd =(Int32)(startDate.AddDays(i+1).ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var forecastItems = response.List.Where(item => item.UnixDate >= unixStart && item.UnixDate < unixEnd);

                var maxTemperatureItem = forecastItems.OrderByDescending(item => item.Main.MaxTemperature).FirstOrDefault();
                var minTemperatureItem = forecastItems.OrderBy(item => item.Main.MinTemperature).FirstOrDefault();

                if(maxTemperatureItem != null)
                {
                    yield return new WeatherDay()
                    {
                        Date = startDate.AddDays(i),
                        MaxTemperature = ToCelsius(maxTemperatureItem.Main.MaxTemperature),
                        MinTemperature = ToCelsius(minTemperatureItem.Main.MinTemperature),
                        Description = maxTemperatureItem.Weather.FirstOrDefault()?.Description,
                        Image = maxTemperatureItem.Weather.FirstOrDefault()?.Icon
                    };
                }
                else
                {
                    // this is just for demo purposes, because the API gives us only 5 days we will generate random values for the extra days
                    /*yield return new WeatherDay()
                    {
                        Date = startDate.AddDays(i),
                        MaxTemperature = random.Next(50, 60),
                        MinTemperature = random.Next(0,10)
                    };*/
                }
            }
        }

        private double ToCelsius(double value)
        {
            return value - 273.15;
        }
    }
}
