using System;

namespace IsItRaining.Exceptions
{
    public class GpsNotFoundException : Exception
    {
        public GpsNotFoundException() : base("Device position can not be idenfied")
        {
        }
    }
}
