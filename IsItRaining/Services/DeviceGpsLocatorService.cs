using IsItRaining.Exceptions;
using IsItRaining.Services.Models;
using System.Device.Location;
using System.Threading.Tasks;

namespace IsItRaining.Services
{
    public class DeviceGpsLocatorService : IGpsLocatorService
    {
        private readonly TaskCompletionSource<GpsLocation> _gpsLocationCompletionSource;
        private readonly TaskCompletionSource<string> _addressCompletionSource;
        private readonly GeoCoordinateWatcher _watcher;

        public DeviceGpsLocatorService()
        {
            _gpsLocationCompletionSource = new TaskCompletionSource<GpsLocation>();
            _addressCompletionSource = new TaskCompletionSource<string>();

            _watcher = new GeoCoordinateWatcher();
            _watcher.PositionChanged += watcher_PositionChanged;
            _watcher.Start(true);
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate location = e.Position.Location;
            if (location.IsUnknown)
            {
                _gpsLocationCompletionSource.SetException(new GpsNotFoundException());
                _addressCompletionSource.SetException(new UnknownAddressException());
            }
            else
            {
                var result = new GpsLocation(location.Latitude, location.Longitude);

                _gpsLocationCompletionSource.SetResult(result);

                ResolveCivilAddress(location);
            }

            _watcher.Stop();
        }

        private void ResolveCivilAddress(GeoCoordinate location)
        {
            var resolver = new CivicAddressResolver();
            var address = resolver.ResolveAddress(location);

            if (!address.IsUnknown)
            {
                var civilAddress = $"{address.CountryRegion}, {address.City}";

                _addressCompletionSource.SetResult(civilAddress);
            }
            else
            {
                _addressCompletionSource.SetException(new UnknownAddressException());
            }
        }

        public Task<GpsLocation> GetCurrentLocationAsync()
        {
            return _gpsLocationCompletionSource.Task;
        }

        public Task<string> GetCivilAddressAsync()
        {
            return _addressCompletionSource.Task;
        }
    }
}
