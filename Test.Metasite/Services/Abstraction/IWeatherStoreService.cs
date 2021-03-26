using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Metasite.Models;

namespace Test.Metasite.Services.Abstraction
{
    public interface IWeatherStoreService
    {
        Task SaveWeatherAsync(WeatherModel[] weather);
    }
}
