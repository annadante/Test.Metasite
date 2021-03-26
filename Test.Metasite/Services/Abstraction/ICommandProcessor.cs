using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.Metasite.Services.Abstraction
{
    public interface ICommandProcessor
    {
        string Name { get; }

        void ParseArguments(Dictionary<string, string> args);
        Task ProcessCommand();
    }
}
