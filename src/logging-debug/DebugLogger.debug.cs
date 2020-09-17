// We need to define the DEBUG symbol because we want the logger
// to work even when this package is compiled on release. Otherwise,
// the call to Debug.WriteLine will not be in the release binary
#define DEBUG

namespace Bytewizer.TinyCLR.Logging.Debug
{
    internal partial class DebugLogger
    {
        private void DebugWriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
