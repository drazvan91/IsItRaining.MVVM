using IsItRaining.Services.Models;
using OpenWeatherMap.Api;
using OpenWeatherMap.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsItRaining.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly OpenWeatherMapClient _client;
        private readonly Random _random = new Random();

        public OpenWeatherMapService()
        {
            // todo: would be nice to move the API KEY in a configuration service somewhere
            this._client = new OpenWeatherMapClient("f426d5672979d3151d56d51c4fc93245");
        }

        public async Task<WeatherResponse> GetWeatherAsync(DateTime startDate, int numberOfDays, double latitude, double longitude)
        {
            var response = await _client.Get5DaysForecastAsync(latitude, longitude);

            var dailyForcast = MapForecast(startDate, numberOfDays, response).ToList();
            return new WeatherResponse()
            {
                Days = dailyForcast,
            };
        }

        private IEnumerable<WeatherDay> MapForecast(DateTime startDate, int numberOfDays, Get5DaysForecastResponse response)
        {
            for (int i = 0; i < numberOfDays; i++)
            {
                var unixStart = GetUnixTime(startDate.AddDays(i));
                var unixEnd = GetUnixTime(startDate.AddDays(i + 1));
                var forecastItems = response.List.Where(item => item.UnixDate >= unixStart && item.UnixDate < unixEnd);

                var maxTemperatureItem = forecastItems.OrderByDescending(item => item.Main.MaxTemperature).FirstOrDefault();
                var minTemperatureItem = forecastItems.OrderBy(item => item.Main.MinTemperature).FirstOrDefault();

                if (maxTemperatureItem != null)
                {
                    yield return new WeatherDay()
                    {
                        Date = startDate.AddDays(i),
                        MaxTemperature = ToCelsius(maxTemperatureItem.Main.MaxTemperature),
                        MinTemperature = ToCelsius(minTemperatureItem.Main.MinTemperature),
                        Description = maxTemperatureItem.Weather.FirstOrDefault()?.Description,
                        Image = maxTemperatureItem.Weather.FirstOrDefault()?.Icon,
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

        private int GetUnixTime(DateTime date)
        {
            return (int)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        private double ToCelsius(double value)
        {
            return value - 273.15;
        }
    }
}
