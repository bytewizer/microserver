using System;
using System.Collections;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Http.Authenticator;


namespace Bytewizer.TinyCLR.WebServer.Authentication
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}