using System;
using System.IO;
using System.Collections;

using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.Extensions.Configuration.Json
{
    internal class JsonConfigurationFileParser
    {
        private JsonConfigurationFileParser() { }

        private readonly Dictionary _data = new Dictionary();
        private readonly Stack _context = new Stack();
        private string _currentPath;

        public static Dictionary Parse(Stream input)
           => new JsonConfigurationFileParser().ParseStream(input);

        private Dictionary ParseStream(Stream input)
        {
            _data.Clear();

            using (var reader = new StreamReader(input))
            {

                // TODO: Finish parser
                var data = JsonConverter.Deserialize(reader);
            }

            return _data;
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context);
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context);
        }
    }
}