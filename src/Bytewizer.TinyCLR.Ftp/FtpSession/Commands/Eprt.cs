namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {

        private void Eprt()
        {
            //TODO: ??????

            //_listener.Stop();
            //_endpoint = new IPEndPoint(IPAddress.Parse(ip), port);

            _context.Request.DataMode = DataMode.ExtendedActive;
            _context.Response.Write(200, $"Data Connection Established");
        }

    }
}