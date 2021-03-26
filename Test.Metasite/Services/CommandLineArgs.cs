using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Metasite.Services
{
    public class CommandLineArgs
    {
        public string[] Args { get; private set; }

        public CommandLineArgs(string[] args)
        {
            Args = args;
        }
    }
}
