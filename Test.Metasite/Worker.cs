using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Metasite.Services;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly CommandFactory _commandFactory;

        public Worker(ILogger<Worker> logger,  CommandFactory commandFactory)
        {
            _logger = logger;
            _commandFactory = commandFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var command = _commandFactory.GetCommandProcessor();
            while (!stoppingToken.IsCancellationRequested)
            {
                await command.ProcessCommand();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
