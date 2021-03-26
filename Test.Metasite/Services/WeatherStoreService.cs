using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Test.Metasite.Models;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite.Services
{
    public class WeatherStoreService : IWeatherStoreService
    {
        private readonly ILogger<WeatherStoreService> _logger;

        public WeatherStoreService(ILogger<WeatherStoreService> logger)
        {
            _logger = logger;
        }

        public async Task SaveWeatherAsync(WeatherModel[] weather)
        {
            if(weather == null)
            {
                _logger.LogError("Weather model is empty", weather);
                throw new ArgumentNullException();
            }
            using var writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "weatherDB.csv"));
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(weather);
        }
    }
}
