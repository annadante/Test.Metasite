using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Metasite.Models;

namespace Test.Metasite.Services.Abstraction
{
    public interface IWeatherFormatter
    {
        Task DisplayWeatherAsync(WeatherModel weatherModel);
    }
}
