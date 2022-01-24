using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Pipeline.Builder;
using GHIElectronics.TinyCLR.Devices.UsbClient;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an implementation of the <see cref="ConsoleServerOptions"/> for creating terminal servers.
    /// </summary>
    public class ConsoleServerOptions : TerminalServerOptions
    {
        private readonly IApplicationBuilder _applicationBuilder = new ApplicationBuilder();

        public UsbClientMode Mode { get; set; } = UsbClientMode.Cdc;
        public string ManufactureName { get; set; } = "Bytewizer Inc.";
        public string ProductName { get; set; } = "TinyCLR OS";
        public string InterfaceName { get; set; } = "TinyCLR OS";
        public string SerialNumber { get; set; } = "12345678";
        public string Guid { get; set; } = "{40082788-8147-4075-8295-8804C5AE4EB9}";
        
        /// <summary>
        /// An application pipeline for registered middleware. 
        /// </summary>
        public IApplication Application { get; private set; }

        /// <summary>
        /// Configures <see cref="IApplication"/> pipeline options. <see cref="Middleware"/> are executed in the order they are added.
        /// </summary>
        /// <param name="configure">The delegate for configuring the <see cref="IApplicationBuilder"/> that will be used to construct the <see cref="IApplication"/>.</param>
        public void Pipeline(ApplicationDelegate configure)
        {
            configure(_applicationBuilder);
            Application = _applicationBuilder.Build();
        }
    }
}