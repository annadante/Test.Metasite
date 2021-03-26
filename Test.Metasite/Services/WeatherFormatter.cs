using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Test.Metasite.Models;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite.Services
{
    class WeatherFormatter : IWeatherFormatter
    {
        public Task DisplayWeatherAsync(WeatherModel weatherModel)
        {
            var weatherJson = JsonConvert.SerializeObject(weatherModel);
            Console.WriteLine(weatherJson);

            return Task.CompletedTask;
        }
    }
}
