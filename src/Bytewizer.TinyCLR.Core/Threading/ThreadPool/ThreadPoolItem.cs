namespace System.Threading
{
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
}