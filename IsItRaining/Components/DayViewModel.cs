using Caliburn.Micro;
using System;

namespace IsItRaining.Components
{
    public class DayViewModel: PropertyChangedBase
    {
        private DateTime _day;
        public DateTime Day
        {
            get
            {
                return _day;
            }
            set
            {
                this._day = value;
                NotifyOfPropertyChange(() => Day);
                NotifyOfPropertyChange(() => Title);
            }
        }

        public string Title
        {
            get
            {
                if (_day.Year <= DateTime.MinValue.Year)
                {
                    return "NA";
                }

                return $"{_day.DayOfWeek.ToString().Substring(0, 3)}, {_day.Day}";
            }
        }

        private double? _maxTemperature;
        public double? MaxTemperature
        {
            get
            {
                return _maxTemperature;
            }
            set
            {
                _maxTemperature = value;
                NotifyOfPropertyChange(() => MaxTemperature);
                NotifyOfPropertyChange(() => MaxTemperatureText);
            }
        }

        public string MaxTemperatureText
        {
            get
            {
                if (this.MaxTemperature.HasValue)
                    return string.Format("{0:0.0}C", MaxTemperature.Value);


                return "NA";
            }
        }

        private double? _minTemperature;
        public double? MinTemperature
        {
            get
            {
                return _minTemperature;
            }
            set
            {
                _minTemperature = value;
                NotifyOfPropertyChange(() => MinTemperature);
                NotifyOfPropertyChange(() => MinTemperatureText);
            }
        }

        public string MinTemperatureText
        {
            get
            {
                if (this.MinTemperature.HasValue)
                    return string.Format("{0:0.0}C", MinTemperature.Value);

                return "NA";
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyOfPropertyChange(() => Description);

            }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
                NotifyOfPropertyChange(() => ImageUrl);
            }
        }

        public string ImageUrl
        {
            get
            {
                if (Image == null)
                    return "";

                return $"http://openweathermap.org/img/w/{ Image }.png";
            }
        }
    }
}
