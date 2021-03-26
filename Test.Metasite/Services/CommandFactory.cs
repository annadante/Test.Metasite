using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Metasite.Services.Abstraction;

namespace Test.Metasite.Services
{
    public class CommandFactory
    {
        private CommandLineArgs _commandLineArgs;

        private readonly ILogger<CommandFactory> _logger;

        private Dictionary<string, ICommandProcessor> _processors;
        public CommandFactory(
            CommandLineArgs commandLineArgs, 
            IEnumerable<ICommandProcessor> processors, 
            ILogger<CommandFactory> logger)
        { 
            _commandLineArgs = commandLineArgs;
            _logger = logger;
            _processors = processors.ToDictionary(x=>x.Name);
        }

        public ICommandProcessor GetCommandProcessor()
        {
            var args = _commandLineArgs.Args;
            if(args == null)
            {
                _logger.LogError("Command line arguments are null");
                throw new ArgumentNullException();
            }

            var commandProcessor = _processors[args[0]];

            var arguments = new Dictionary<string, string>();
            string paramName = string.Empty;

            for(var i = 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    paramName = args[i].Substring(2);
                }

                var j = i + 1;
                string values = string.Empty;

                for(; j<args.Length; j++)
                {
                    if (args[j].StartsWith("--"))
                    {
                        break;
                    }
                    values += args[j];
                }
                i = j - 1;
                arguments[paramName] = values;
            }

            commandProcessor.ParseArguments(arguments);
            return commandProcessor;
        }
    }
}
