using System;
using System.Threading;
using System.Collections;

namespace Bytewizer.TinyCLR.Threading
{
	/// <summary>
	/// Provides a queue of short running sequential thread execution.
	/// </summary>
	public class ThreadQueue : IDisposable
	{
		private static ThreadQueue _instance;

		private readonly Queue _taskQueue = new Queue();
		private readonly ManualResetEvent _queueEvent = new ManualResetEvent(false);
        
		private bool _running = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThreadQueue"/> for sequential execution threads.
		/// </summary>
		public static ThreadQueue Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ThreadQueue();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Adds an <see cref="Action"/> to execute with <see cref="ThreadPool"/>.
		/// </summary>
		/// <param name="task">The action to execute.</param>
		public void Enqueue(Action task)
		{
			lock (_taskQueue)
			{
				_taskQueue.Enqueue(task);
				_queueEvent.Set();
			}
		}

		/// <summary>
		/// Frees resources owned by this instance.
		/// </summary>
		public void Dispose()
		{
			_running = false;
			_queueEvent.Set();
		}

		private ThreadQueue()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(Execute));
		}

		private void Execute(object obj)
		{
			while (_queueEvent.WaitOne())
			{
				if (!_running) { break; }

				Action actor = null;
				lock (_taskQueue)
				{
					if (_taskQueue.Count > 0)
					{
						actor = (Action)_taskQueue.Dequeue();
					}
					else
					{
						_queueEvent.Reset();
					}
				}
				if (actor != null)
				{
					try
					{
						actor();
					}
					catch
					{
						lock (_taskQueue)
						{
							_running = false;
							_taskQueue.Clear();
						}
					}
				}
			}
		}
	}
}