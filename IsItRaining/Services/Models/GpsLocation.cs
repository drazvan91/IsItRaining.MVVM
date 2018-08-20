namespace IsItRaining.Services.Models
{
    public class GpsLocation
    {
        public GpsLocation()
        {
        }

        public GpsLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
