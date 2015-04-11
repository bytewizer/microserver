using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;

namespace MicroServer.Threading
{
    /// <summary>
    /// Represents a callback method to be executed by a thread pool thread.
    /// </summary>
    /// <param name="state">An object containing information to be used by the callback method.</param>
    public delegate void WaitCallback(object state);

    public delegate void UnhandledThreadPoolExceptionDelegate(object state, Exception ex);

    /// <summary>
    /// Provides a pool of threads that can be used to execute tasks, post work items, 
    /// process asynchronous I/O, wait on behalf of other threads, and process timers.
    /// </summary>
    /// <remarks>
    /// Because the maximum number of threads used by the ThreadPool only short running
    /// operations should be executed by using it.
    /// Multiple long running operations would block the available threads.
    /// New operations will not be called at all if all threads are blocked.
    /// </remarks>
    public static class ThreadPool
    {
        static ThreadPool()
        {
            // create the initial number of threads
            SetMinThreads(3);
        }

        /// <summary>
        /// Queues a method for execution. The method executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="callback">A WaitCallback that represents the method to be executed.</param>
        /// <returns>true if the method is successfully queued.</returns>
        public static bool QueueUserWorkItem(WaitCallback callback)
        {
            return QueueUserWorkItem(callback, null);
        }

        /// <summary>
        /// Queues a method for execution, and specifies an object containing data to be used by the method. 
        /// The method executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="callback">A WaitCallback representing the method to execute.</param>
        /// <param name="state">An object containing data to be used by the method.</param>
        /// <returns>true if the method is successfully queued.</returns>
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

        /// <summary>
        /// Retrieves the minimum number of threads the thread pool creates on demand.
        /// </summary>
        /// <returns>Returns the minimum number of worker threads that the thread pool creates on demand.</returns>
        public static int GetMinThreads()
        {
            return _minThreadCount;
        }

        /// <summary>
        /// Sets the minimum number of threads the thread pool creates on demand.
        /// </summary>
        /// <param name="count">The minimum number of worker threads that the thread pool creates on demand.</param>
        /// <returns>true if the change is successful; otherwise, false.</returns>
        public static bool SetMinThreads(int count)
        {
            _minThreadCount = count;

            while (_Threads.Count < _minThreadCount)
            {
                CreateNewThread();
            }
            return true;
        }

        /// <summary>
        /// Retrieves the number of requests to the thread pool that can be active concurrently. 
        /// All requests above that number remain queued until thread pool threads become available.
        /// </summary>
        /// <returns>The maximum number of asynchronous I/O threads in the thread pool.</returns>
        public static int GetMaxThreads()
        {
            return _maxThreadCount;
        }

        /// <summary>
        /// Sets the number of requests to the thread pool that can be active concurrently. 
        /// All requests above that number remain queued until thread pool threads become available.
        /// </summary>
        /// <param name="count">The maximum number of worker threads in the thread pool.</param>
        /// <returns>true if the change is successful; otherwise, false.</returns>
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

        /// <summary>
        /// Shuts down all threads after they have finished theire work.
        /// </summary>
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
                    Debug.Print(string.Concat("ThreadPool | ",  DateTime.Now.ToString("MM/dd/yyyy | HH:mm:ss.fff") , " | Thread stopped: #" + _Threads.Count));
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

        /// <summary>
        /// Is fired when a excption in one of the worker threads in unhandeld.
        /// </summary>
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
                // ReSharper disable once EmptyGeneralCatchClause
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