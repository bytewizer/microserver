using System;
using System.Threading;
using System.Collections;

namespace Gadgeteer.Modules.GHIElectronics
{
    public class CyclingDisplay : CharacterDisplay
    {
        object _Lock = new object();
        bool _IsCycle = false;
        Hashtable _Messages = new Hashtable();
        Thread _Thread = null;


        /// <summary>
        /// Constructor for creating an intance of the CyclingDisplay
        /// </summary>
        /// <param name="socketNumber">Socket to use. Defaults to socket 3</param>
        public CyclingDisplay(int socketNumber = 3)
            : base(socketNumber)
        {
            StartStopCycle(true);
        }

        /// <summary>
        /// This starts or stops messages from cycling
        /// </summary>
        /// <param name="cycle">boolean on whether to start or stop cycling</param>
        /// <param name="interval">Message display interval while cycling</param>
        public void StartStopCycle(bool cycle, int interval = 5)
        {
            if (_IsCycle == cycle)
                return;

            _IsCycle = cycle;
            if (_Thread == null)
            {
                _Thread = new Thread(() =>
                {
                    while (_IsCycle)
                    {
                        foreach (var key in _Messages.Keys)
                        {
                            if (!_IsCycle)
                                break;
                            base.Clear();
                            base.CursorHome();
                            base.Clear();
                            base.CursorHome();
                            base.Print(_Messages[key].ToString() + "\n");
                            Thread.Sleep(interval * 1000);
                        }

                    }
                });
            }
            if (_IsCycle)
            {
                _Thread.Start();
            }
            else
                _Thread.Abort();
        }
        /// <summary>
        /// Prints a message on the display
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="cycleDelay">If cycling, set dele</param>
        public void Print(string message, int cycleDelay)
        {
            if (_Thread.IsAlive && _IsCycle)
                _Thread.Suspend();
            base.Clear();
            base.CursorHome();
            base.Clear();
            base.CursorHome();
            base.Print(message);
            Thread.Sleep(cycleDelay * 1000);
            if (_Thread.IsAlive && _IsCycle)
                _Thread.Resume();
        }
        /// <summary>
        /// Adds a message to the messages collection
        /// </summary>
        /// <param name="display"></param>
        /// <param name="key">The lookup identifier</param>
        /// <param name="value">The message to add.</param>
        public void AddMessage(object key, object value)
        {
            lock (_Lock)
            {
                _Messages[key] = value;
            }
        }
    }
}