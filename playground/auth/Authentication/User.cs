using System;
using System.Collections;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Http;


namespace Bytewizer.Playground.Authentication
{
    public class User : IUser
    {
        public User()
        {
        }

        public User(int id, string firstName, string lastName, string username, string ha1)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            HA1 = ha1;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string HA1 { get; set; }
    }
}