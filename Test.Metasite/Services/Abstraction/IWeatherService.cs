using System.Threading.Tasks;
using Test.Metasite.Models;

namespace Test.Metasite.Services.Abstraction
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync(string city);

    }
}