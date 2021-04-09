using System.Diagnostics;

using Bytewizer.TinyCLR.Pipeline;
using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.Playground.Sockets
{
    /// <summary>
    /// Captures memory information from the pipeline.
    /// </summary>
    public class MemoryInfoMiddleware : Middleware
    {
        private long _managedUsedIn;
        private long _managedFreeIn;
        private long _unmanagedUsedIn;
        private long _unmanagedFreeIn;

        private long _managedUsedOut;
        private long _managedFreeOut;
        private long _unmanagedUsedOut;
        private long _unmanagedFreeOut;

        private long _managedUsedDelta;
        private long _managedFreeDelta;
        private long _unmanagedUsedDelta;
        private long _unmanagedFreeDelta;

        /// <inheritdoc/>
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            _managedUsedIn = Memory.ManagedMemory.UsedBytes;
            _managedFreeIn = Memory.ManagedMemory.FreeBytes;
            _unmanagedUsedIn = Memory.UnmanagedMemory.UsedBytes;
            _unmanagedFreeIn = Memory.UnmanagedMemory.FreeBytes;

            Debug.WriteLine($"+-----------+   Pipe In  +------------+");
            Debug.WriteLine($"+-----------+------------+------------+");
            Debug.WriteLine($"| Memory    |       Used |       Free |");
            Debug.WriteLine($"| Managed   | {_managedUsedIn,10:N0} | {_managedFreeIn,10:N0} |");
            Debug.WriteLine($"| Unmanaged | {_unmanagedUsedIn,10:N0} | {_unmanagedFreeIn,10:N0} |");
            Debug.WriteLine($"+-----------+------------+------------+\r\n");

            next(context);

            _managedUsedOut = Memory.ManagedMemory.UsedBytes;
            _managedFreeOut = Memory.ManagedMemory.FreeBytes;
            _unmanagedUsedOut = Memory.UnmanagedMemory.UsedBytes;
            _unmanagedFreeOut = Memory.UnmanagedMemory.FreeBytes;
            
            _managedUsedDelta = _managedUsedOut  - _managedUsedIn;
            _managedFreeDelta = _managedFreeOut - _managedFreeIn; 
            _unmanagedUsedDelta = _unmanagedUsedOut - _unmanagedUsedIn;
            _unmanagedFreeDelta = _unmanagedFreeOut - _unmanagedFreeIn;


            Debug.WriteLine($"+-----------+  Pipe Out  +------------+");
            Debug.WriteLine($"+-----------+------------+------------+");
            Debug.WriteLine($"| Memory    |       Used |       Free |");
            Debug.WriteLine($"| Managed   | {_managedUsedOut,10:N0} | {_managedFreeOut,10:N0} |");
            Debug.WriteLine($"| Unmanaged | {_unmanagedUsedOut,10:N0} | {_unmanagedFreeOut,10:N0} |");
            Debug.WriteLine($"+-----------+------------+------------+");
            Debug.WriteLine($"| Memory Δ  |     Used Δ |     Free Δ |");
            Debug.WriteLine($"| Managed   | {_managedUsedDelta,10:N0} | {_managedFreeDelta,10:N0} |");
            Debug.WriteLine($"| Unmanaged | {_unmanagedUsedDelta,10:N0} | {_unmanagedFreeDelta,10:N0} |");
            Debug.WriteLine($"+-----------+------------+------------+\r\n");
        }
    }
}