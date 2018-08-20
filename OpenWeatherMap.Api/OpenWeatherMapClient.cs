using Newtonsoft.Json;
using OpenWeatherMap.Api.Dtos;
using System;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace OpenWeatherMap.Api
{
    public class OpenWeatherMapClient
    {
        private readonly string apiKey;
        ObjectCache _cache = MemoryCache.Default;

        public OpenWeatherMapClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<Get5DaysForecastResponse> Get5DaysForecastAsync(double latitude, double longitude)
        {
            string CACHE_KEY = "OpenWeatherMapClient-" + latitude + "|" + longitude;
            if (_cache[CACHE_KEY] is Get5DaysForecastResponse cacheObject)
            {
                return cacheObject;
            }

            var result = await Fetch5DaysForecastAsync(latitude, longitude);

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);

            _cache.Set(CACHE_KEY, result, policy);

            return result;
        }

        private async Task<Get5DaysForecastResponse> Fetch5DaysForecastAsync(double latitude, double longitude)
        {
            WebClient webClient = new WebClient();
            var uri = new Uri($"https://api.openweathermap.org/data/2.5/forecast?lat={ latitude }&lon={ longitude }&&APPID={apiKey}");
            var responseString = await webClient.DownloadStringTaskAsync(uri);

            return JsonConvert.DeserializeObject<Get5DaysForecastResponse>(responseString);
        }
    }
}
