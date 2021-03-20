using System;

using Bytewizer.TinyCLR.Http.Mvc;

using Bytewizer.TinyCLR.HelloWorld.Models;

namespace Bytewizer.TinyCLR.HelloWorld
{
    public class JsonController : Controller
    {
        private readonly Person[] _persons;
        
        public JsonController()
        {
            _persons = new Person[2];
            
            var rjc = new Person()
            {
                Id = 100,
                FirstName = "Roscoe",
                MiddleName = "Jerald",
                LastName = "Crona",
                Title = "Mr.",
                DOB = DateTime.Now,
                Email = "Roscoe@gmail.com",
                Gender = Gender.Male,
                SSN = "939-69-5554",
                Suffix = "I",
                Phone = "(458)-857-7797"
            };
           _persons[0]= rjc;
           
            var jpc = new Person()
            {
                Id = 100,
                FirstName = "Jennifer",
                MiddleName = "Parker",
                LastName = "Crona",
                Title = "Ms.",
                DOB = DateTime.Now,
                Email = "Jpc@gmail.com",
                Gender = Gender.Female,
                SSN = "223-69-5534",
                Suffix = "I",
                Phone = "(342)-337-4397"
            };
            _persons[1] = jpc;
        }

        public IActionResult GetPersons()
        {
            return Json(_persons);
        }
    }
}