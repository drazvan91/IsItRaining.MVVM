using IsItRaining.Services.Models;
using System.Threading.Tasks;

namespace IsItRaining.Services.Mocks
{
    public class MockedGpsLocatorService : IGpsLocatorService
    {
        public Task<string> GetCivilAddressAsync()
        {
            return Task.FromResult("Romania, Cluj Napoca");
        }

        public Task<GpsLocation> GetCurrentLocationAsync()
        {
            return Task.FromResult(new GpsLocation()
            {
                Latitude = 46.76667,
                Longitude = 23.6,
            });
        }
    }
}
