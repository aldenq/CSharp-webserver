namespace CommandLine
{
    public class CommandLineHandler
    {
        private Dictionary<string, string> _arguments;

        public CommandLineHandler(string[] args)
        {
            _arguments = new Dictionary<string, string>();
            ParseArguments(args);
        }

        private void ParseArguments(string[] args)
        {
            string key = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    if (key != null)
                    {
                        // If a new key is found without a value for the previous key, add the previous key with a null value
                        _arguments[key] = null;
                    }
                    key = arg;
                }
                else
                {
                    if (key != null)
                    {
                        _arguments[key] = arg;
                        key = null;
                    }
                    else
                    {
                        // Handle the case where there's a value without a preceding key
                        Console.WriteLine($"Warning: Value '{arg}' found without a preceding key and will be ignored.");
                    }
                }
            }

            // Handle the last key if it doesn't have a value
            if (key != null)
            {
                _arguments[key] = null;
            }
        }

        public string GetArgumentValue(string key)
        {
            if (_arguments.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public bool HasArgument(string key)
        {
            return _arguments.ContainsKey(key);
        }
    }
}