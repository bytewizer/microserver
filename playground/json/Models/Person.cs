using System;

namespace Bytewizer.Playground.Json.Models
{
    public class Person
    {        
        public int Id { get; set; }

        public string Suffix { get; set; }

        public string Title { get; set; }
        
        public string LastName { get; set; }

        public string Phone { get; set; }

        public Gender Gender { get; set; }

        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public DateTime DOB { get; set; }      
    }
}
