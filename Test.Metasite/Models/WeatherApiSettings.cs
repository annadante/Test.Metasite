using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Metasite.Models
{
    public class WeatherApiSettings
    {
        public string AuthorizationUrl { get; set; }

        public string GetWeatherUrl { get; set; }

        public string GetCitiesUrl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
