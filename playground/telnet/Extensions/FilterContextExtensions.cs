using Bytewizer.TinyCLR.Telnet;

namespace Bytewizer.Playground.Telnet
{
    /// <summary>
    /// Extension methods for <see cref="ActionExecutingContext"/> related to routing.
    /// </summary>
    public static class FilterContextExtensions
    {
        /// <summary>
        /// Extension method for getting the arguments for the current request.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/> context.</param>
        public static int GetArgumentOrDefault(this ActionExecutingContext filterContext, string key, int defaultValue)
        {
            if (filterContext.Arguments.TryGetValue(key, out string value))
            {
                if (!int.TryParse(value, out int number))
                {
                    filterContext.Result = new ResponseResult($"An error occurred parsing the --{value} flag.");
                }

                return number;
            }

            return defaultValue;
        }
    }
}