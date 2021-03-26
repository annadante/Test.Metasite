using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.Metasite.Models;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite.Services
{
    public class WeatherService : IWeatherService
    {

        private readonly IOptionsMonitor<WeatherApiSettings> _options;

        private readonly ILogger<WeatherService> _logger;

        private readonly SemaphoreSlim _semaphoreSlim;

        private string _token;

        private HashSet<string> _listOfCities;

        private HttpClient _client;

        public WeatherService(
            IOptionsMonitor<WeatherApiSettings> options,
            HttpClient client,
            ILogger<WeatherService> logger)
        {
            _client = client;
            _options = options;
            _logger = logger;
            _semaphoreSlim = new SemaphoreSlim(1);
        }

        private async Task AuthorizationAsync()
        {
            if (String.IsNullOrEmpty(_token))
            {
                _logger.LogError("Token is empty", _token);
                throw new ArgumentNullException();
            }
                

            await _semaphoreSlim.WaitAsync();

            try
            {
                if (_token != null)
                {
                    _logger.LogError("Token is empty", _token);
                    throw new ArgumentNullException();
                }

                var authorizationModel = new AuthorizationModel();
                authorizationModel.Password = _options.CurrentValue.Password;
                authorizationModel.Username = _options.CurrentValue.Username;

                var objAsJson = JsonConvert.SerializeObject(authorizationModel);
                var body = new StringContent(objAsJson, Encoding.UTF8, "application/json");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.CurrentValue.AuthorizationUrl}/authorize");
                httpRequest.Content = body;

                var response = await _client.SendAsync(httpRequest);
                var responseJson = await response.Content.ReadAsStringAsync();

                _token = JsonConvert.DeserializeObject<TokenModel>(responseJson).Bearer;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<WeatherModel> GetWeatherAsync(string city)
        {
            await AuthorizationAsync();

            if (!await CheckAvailability(city))
            {
                _logger.LogWarning($"City is not available. City = {city}", city);
                throw new ArgumentNullException();
            }
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{_options.CurrentValue.GetWeatherUrl}/{city}");
            httpRequest.Headers.Add("Authorization", $"bearer {_token}");
            var response = await _client.SendAsync(httpRequest);
            var responseJson = await response.Content.ReadAsStringAsync();
            var weatherModel = JsonConvert.DeserializeObject<WeatherModel>(responseJson);

            return weatherModel;
        }

        private async Task<bool> CheckAvailability(string city)
        {
            await LoadCitiesAsync();
            return _listOfCities.Contains(city);
        }


        private async Task LoadCitiesAsync()
        {
            if (_listOfCities != null)
            {
                _logger.LogError("List of cities is empty", _listOfCities);
                throw new ArgumentNullException();
            }
               
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_listOfCities != null)
                {
                    _logger.LogError("List of cities is empty", _listOfCities);
                    throw new ArgumentNullException();
                }
              
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{_options.CurrentValue.GetCitiesUrl}");
                httpRequest.Headers.Add("Authorization", $"bearer {_token}");
                var response = await _client.SendAsync(httpRequest);
                var responseJson = await response.Content.ReadAsStringAsync();
                _listOfCities = JsonConvert.DeserializeObject<HashSet<string>>(responseJson);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
           
        }
    }
}
