using System;
using System.Threading;
using System.Collections;

namespace Bytewizer.TinyCLR.Threading
{
	public class ThreadQueue : IDisposable
	{
		private static ThreadQueue instance;

		private readonly Queue taskQueue = new Queue();
		private readonly ManualResetEvent queueEvent = new ManualResetEvent(false);
        
		private bool running = true;

		public bool ExistQueueEvent { get; private set; } = false;

		public static ThreadQueue Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ThreadQueue();
				}
				return instance;
			}
		}

		private ThreadQueue()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(Execute));
		}

		private void Execute(object obj)
		{
			while (queueEvent.WaitOne())
			{
				if (!running) { break; }

				Action actor = null;
				lock (taskQueue)
				{
					if (taskQueue.Count > 0)
					{
						actor = (Action)taskQueue.Dequeue();
					}
					else
					{
						ExistQueueEvent = false;
						queueEvent.Reset();
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
						lock (taskQueue)
						{
							running = false;
							taskQueue.Clear();
						}
					}
				}
			}
		}

        public void Enqueue(Action task)
		{
			lock (taskQueue)
			{
				taskQueue.Enqueue(task);
				queueEvent.Set();
				ExistQueueEvent = true;
			}
		}

		public void Dispose()
		{
			running = false;
			ExistQueueEvent = false;
			queueEvent.Set();
		}
	}
}

