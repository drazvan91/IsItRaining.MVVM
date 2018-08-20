using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsItRaining.Services.Models
{
    public class WeatherResponse
    {
        public List<WeatherDay> Days { get; set; } 
    }
}
