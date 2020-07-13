using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace Bytewizer.Sockets
{
    public delegate void WaitCallback(object state);

    public delegate void UnhandledThreadPoolExceptionDelegate(object state, Exception ex);

    public static class ThreadPool
    {
        static ThreadPool()
        {
            SetMinThreads(3);
        }

        public static bool QueueUserWorkItem(WaitCallback callback)
        {
            return QueueUserWorkItem(callback, null);
        }

        public static bool QueueUserWorkItem(WaitCallback callback, object state)
        {
            lock (_ItemsQueue.SyncRoot)
            {
                var thread = GetThread();
                if (thread != null)
                {
                    thread.Item = new ThreadPoolItem(callback, state);
                }
                else
                {
                    _ItemsQueue.Enqueue(new ThreadPoolItem(callback, state));
                }
                return true;
            }
        }

        private static ThreadPoolThread GetThread()
        {
            lock (_Threads)
            {
                foreach (ThreadPoolThread thread in _Threads)
                {
                    if (!thread.IsBusy)
                    {
                        thread.IsBusy = true;
                        return thread;
                    }
                }
                if (_Threads.Count < _maxThreadCount)
                {
                    var thread = new ThreadPoolThread { IsBusy = true };
                    _Threads.Add(thread);
                    return thread;
                }
                return null;
            }
        }

        private static int _minThreadCount;
        private static int _maxThreadCount = 10;
        private static readonly ArrayList _Threads = new ArrayList();
        private static readonly Queue _ItemsQueue = new Queue();

        public static int GetMinThreads()
        {
            return _minThreadCount;
        }

        public static bool SetMinThreads(int count)
        {
            _minThreadCount = count;

            while (_Threads.Count < _minThreadCount)
            {
                CreateNewThread();
            }
            return true;
        }

        public static int GetMaxThreads()
        {
            return _maxThreadCount;
        }

        public static bool SetMaxThreads(int count)
        {
            _maxThreadCount = count;
            return true;
        }

        private static void CreateNewThread()
        {
            lock (_Threads)
            {
                _Threads.Add(new ThreadPoolThread());
            }
        }

        public static void Shutdown()
        {
            lock (_Threads)
            {
                foreach (ThreadPoolThread thread in _Threads)
                {
                    thread.Dispose();
                }
                _Threads.Clear();
            }
        }

        internal static bool NotifyThreadIdle(ThreadPoolThread thread)
        {
            lock (_Threads)
            {
                if (_Threads.Count > _maxThreadCount)
                {
                    thread.Dispose();
                    _Threads.Remove(thread);
                    Debug.WriteLine(string.Concat("ThreadPool | ",  DateTime.Now.ToString("MM/dd/yyyy | HH:mm:ss.fff") , " | Thread stopped: #" + _Threads.Count));
                    return false;
                }
            }
            // start next enqueued item
            lock (_ItemsQueue.SyncRoot)
            {
                if (_ItemsQueue.Count > 0)
                {
                    thread.Item = _ItemsQueue.Dequeue() as ThreadPoolItem;
                    return true;
                }
            }
            return false;
        }

        internal static void OnUnhandledThreadPoolException(ThreadPoolItem item, Exception exception)
        {
            var tmp = UnhandledThreadPoolException;
            if (tmp != null)
            {
                tmp(item.State, exception);
            }
        }

        public static event UnhandledThreadPoolExceptionDelegate UnhandledThreadPoolException;
    }

    internal class ThreadPoolItem
    {
        public WaitCallback Callback { get; private set; }
        public object State { get; private set; }

        public ThreadPoolItem(WaitCallback callback, object state)
        {
            Callback = callback;
            State = state;
        }
    }

    internal class ThreadPoolThread : IDisposable
    {
        public ThreadPoolThread()
        {
            _thread = new Thread(ThreadProc);
            _thread.Start();
        }

        private void ThreadProc()
        {
            while (_thread != null)
            {
                try
                {
                    _WaitEvent.WaitOne();

                    if (_thread != null && _item != null)
                    {
                        _item.Callback(_item.State);
                    }
                }
                catch (Exception ex)
                {
                    ThreadPool.OnUnhandledThreadPoolException(Item, ex);
                }

                if (_thread != null)
                {
                    _WaitEvent.Reset();
                    _item = null;
                    IsBusy = ThreadPool.NotifyThreadIdle(this);
                }
            }
        }

        public bool IsBusy { get; set; }

        private ThreadPoolItem _item;

        public ThreadPoolItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (_item != null)
                {
                    IsBusy = true;
                    _WaitEvent.Set();
                }
            }
        }

        private readonly ManualResetEvent _WaitEvent = new ManualResetEvent(false);

        private Thread _thread;

        public void Dispose()
        {
            IsBusy = true;
            _thread = null;
            _WaitEvent.Set();
        }
    }
}