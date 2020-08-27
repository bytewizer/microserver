namespace Bytewizer.TinyCLR.Hosting.Internal
{
    internal class GenericWebHostService
    {
        public GenericWebHostService()
        { 
        }

        public IServer Server { get; }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }
}