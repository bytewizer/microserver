namespace System.Threading
{
    /// <summary>
    /// Unhandled Threadpool Exception event handler would be raised whenever an 
    /// unhandled exception occurs in a threadpool.
    /// </summary>
    /// <param name="state">This argument should be ignored.</param>
    /// <param name="ex">The unhandled exception thrown.</param>
    public delegate void UnhandledThreadPoolExceptionDelegate(object state, Exception ex);
}
