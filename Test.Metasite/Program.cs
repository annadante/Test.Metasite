using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.Metasite.Models;
using Test.Metasite.Services;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite
{
    class Program
    {  

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IWeatherService, WeatherService>();
                    services.AddSingleton<IWeatherStoreService, WeatherStoreService>();
                    services.AddSingleton<IWeatherFormatter, WeatherFormatter>();
                    services.AddSingleton(new CommandLineArgs(args));
                    services.AddSingleton<CommandFactory>();
                    services.AddSingleton<ICommandProcessor, WeatherCommandProcessor>();
                    services.AddHttpClient();

                    services.Configure<WeatherApiSettings>(hostContext.Configuration.GetSection("WeatherApiSettings"));
                });
    }
}
