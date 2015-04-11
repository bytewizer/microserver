
using System;

namespace MicroServer.Storage
{
	public interface IStorage : IDisposable
	{
        void Write(string data);
        string Read();
	}
}
