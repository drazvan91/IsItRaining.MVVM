using IsItRaining.Exceptions;
using IsItRaining.Services.Models;
using System.Device.Location;
using System.Threading.Tasks;
using System;

namespace IsItRaining.Services
{
    public class DeviceGpsLocatorService : IGpsLocatorService
    {
        TaskCompletionSource<GpsLocation> _gpsLocationCompletionSource;
        TaskCompletionSource<string> _addressCompletionSource;
        GeoCoordinateWatcher _watcher;

        public DeviceGpsLocatorService()
        {
            _gpsLocationCompletionSource = new TaskCompletionSource<GpsLocation>();
            _addressCompletionSource = new TaskCompletionSource<string>();

            _watcher = new GeoCoordinateWatcher();
            _watcher.PositionChanged += _watcher_PositionChanged;
            _watcher.Start(true);
        }

        private void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                _gpsLocationCompletionSource.SetException(new GpsNotFoundException());
                _addressCompletionSource.SetException(new UnknownAddressException());
            }
            else
            {
                var result = new GpsLocation()
                {
                    Latitude = e.Position.Location.Latitude,
                    Longitude = e.Position.Location.Longitude
                };
                _gpsLocationCompletionSource.SetResult(result);

                ResolveCivilAddress(e.Position.Location);
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
