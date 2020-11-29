using System;
using System.Collections;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Http.Authenticator;


namespace Bytewizer.Playground.Mvc.Authentication
{
    public class User : IUser
    {
        public User()
        {
        }

        public User(int id, string firstName, string lastName, string username, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}