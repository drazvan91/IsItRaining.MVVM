using IsItRaining.Services.Models;
using System.Threading.Tasks;

namespace IsItRaining.Services
{
    public interface IGpsLocatorService
    {
        Task<GpsLocation> GetCurrentLocationAsync();
        Task<string> GetCivilAddressAsync();
    }
}
