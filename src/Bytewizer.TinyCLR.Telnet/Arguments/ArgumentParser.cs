using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{

    public delegate bool CallbackAction(string key, string value);

    /// <summary>
    /// Provides methods for parsing and manipulating argument strings.
    /// </summary>
    internal static class ArgumentParser
    {

        /// <summary>
        /// Parse a argument string into its component key and value parts.
        /// </summary>
        /// <param name="args">The raw argument string value.</param>
        /// <param name="parserCallback">The raw argument string value.</param>
        public static Hashtable ParseArguments(ArrayList args, CallbackAction parserCallback = null)
        {
            Hashtable options = new Hashtable();

            for (int i = 0; i < args.Count; i++)
            {
                if (((string)args[i]).StartsWith("--"))
                {
                    string key = null;
                    string value = null;
                    if (((string)args[i]).IndexOf("=") > 0)
                    {
                        key = ((string)args[i]).Substring(0, ((string)args[i]).IndexOf("="));
                        value = ((string)args[i]).Substring(((string)args[i]).IndexOf("=") + 1);
                    }
                    else
                    {
 
                        key = (string)args[i];
                    }

                    //Skip the leading --
                    key = key.Substring(2).ToLower();
                    if (!string.IsNullOrEmpty(value) && value.Length > 1
                        && value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    options[key] = new ArgumentValue(key, value);

                    //Last argument overwrites the current
                    if (parserCallback == null || parserCallback(key, value))
                    {
                        options[key] = new ArgumentValue(key, value);
                    }

                    args.RemoveAt(i);
                    i--;
                }
            }

            return options;
        }
    }
}
