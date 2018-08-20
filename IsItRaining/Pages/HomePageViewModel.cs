using Caliburn.Micro;
using IsItRaining.Components;
using IsItRaining.Exceptions;
using IsItRaining.Services;
using IsItRaining.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IsItRaining.Pages
{
    public class HomePageViewModel : Screen
    {
        private const int NUMBER_OF_DAYS_TO_DISPLAY = 5;
        private readonly IGpsLocatorService _gpsLocatorService;
        private readonly IWeatherService _weatherService;

        private GpsLocation _gpsLocation;
        private DateTime _firstDay = DateTime.Now.Date;

        public HomePageViewModel(IGpsLocatorService gpsLocatorService, IWeatherService weatherService)
        {
            this._gpsLocatorService = gpsLocatorService;
            this._weatherService = weatherService;
        }

        private BindableCollection<DayViewModel> _days = new BindableCollection<DayViewModel>();
        public BindableCollection<DayViewModel> Days
        {
            get
            {
                return _days;
            }
            set
            {
                _days = value;
                NotifyOfPropertyChange(() => Days);
            }
        }

        private string _locationText = "... please wait ...";
        public string LocationText
        {
            get
            {
                return _locationText;
            }
            set
            {
                _locationText = value;
                NotifyOfPropertyChange(() => LocationText);
            }
        }

        public async void NextDays()
        {
            _firstDay = _firstDay.AddDays(NUMBER_OF_DAYS_TO_DISPLAY);
            ResetToUnavailableData();

            await RefreshWeather();
        }

        public async void PreviousDays()
        {
            _firstDay = _firstDay.AddDays(-NUMBER_OF_DAYS_TO_DISPLAY);
            ResetToUnavailableData();

            await RefreshWeather();
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            ResetToUnavailableData();

            try
            {
                _gpsLocation = await _gpsLocatorService.GetCurrentLocationAsync();
            }
            catch (GpsNotFoundException)
            {
                LocationText = "Your position cound not be located";
                return;
            }

            try
            {
                LocationText = await _gpsLocatorService.GetCivilAddressAsync();
            }
            catch (UnknownAddressException)
            {
                LocationText = string.Format("Your location cannot be identified but your GPS is {0:0.####} {1:0.####}", _gpsLocation.Latitude, _gpsLocation.Longitude);
            }

            await RefreshWeather();
        }

        private void ResetToUnavailableData()
        {
            var days = new BindableCollection<DayViewModel>();

            for (int i = 0; i < NUMBER_OF_DAYS_TO_DISPLAY; i++)
            {
                days.Add(new DayViewModel()
                {
                    Day = _firstDay.AddDays(i),
                    MaxTemperature = null,
                });
            }

            this.Days = days;
        }

        private async Task RefreshWeather()
        {
            var weatherResponse = await this._weatherService.GetWeatherAsync(
                _firstDay,
                NUMBER_OF_DAYS_TO_DISPLAY,
                _gpsLocation.Latitude,
                _gpsLocation.Longitude);

            foreach (var day in this.Days)
            {
                var forecast = weatherResponse.Days.FirstOrDefault(d => d.Date == day.Day);
                if (forecast == null)
                {
                    day.MaxTemperature = null;
                    day.MinTemperature = null;
                    day.Description = null;
                    day.Image = null;
                }
                else
                {
                    day.MaxTemperature = forecast.MaxTemperature;
                    day.MinTemperature = forecast.MinTemperature;
                    day.Description = forecast.Description;
                    day.Image = forecast.Image;
                }
            }
        }
    }
}
