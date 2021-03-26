using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Metasite.Models;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite.Services
{
    public class WeatherCommandProcessor : ICommandProcessor
    {
        private readonly ILogger<WeatherCommandProcessor> _logger;

        private readonly IWeatherService _weatherService;

        private readonly IWeatherStoreService _weatherStoreService;

        private readonly IWeatherFormatter _weatherFormatter;

        public string[] Cities { get; private set; }

        public string Name => "weather";

        public WeatherCommandProcessor(
            IWeatherService weatherService, 
            IWeatherStoreService weatherStoreService, 
            IWeatherFormatter weatherFormatter, 
            ILogger<WeatherCommandProcessor> logger)
        {
            _weatherService = weatherService;
            _weatherStoreService = weatherStoreService;
            _weatherFormatter = weatherFormatter;
            _logger = logger;
        }

        public void ParseArguments(Dictionary<string, string> args)
        {
            if(args == null)
            {
                _logger.LogError("Command line arguments are null", args);
                throw new ArgumentNullException();
            }
            Cities = args["city"].Split(',').Select(x => x.Trim()).ToArray();
        }

        public async Task ProcessCommand()
        {
            var weatherModels = await Task.WhenAll(Cities.Select(x => ProcessCity(x)));
            weatherModels = weatherModels.Where(x => x != null).ToArray();
            await _weatherStoreService.SaveWeatherAsync(weatherModels);
        }

        private async Task<WeatherModel> ProcessCity(string city)
        {
            var weatherModel = await _weatherService.GetWeatherAsync(city);
            if (weatherModel == null)
            {
                _logger.LogWarning($"The city can't be processed. City = {city}", city);
                throw new ArgumentNullException();
            }

            await _weatherFormatter.DisplayWeatherAsync(weatherModel);
            return weatherModel;
        }
    }
}
