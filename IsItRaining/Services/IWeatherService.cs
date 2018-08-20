using IsItRaining.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsItRaining.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(DateTime startDate, int numberOfDays, double latitude, double longitude);
    }
}
