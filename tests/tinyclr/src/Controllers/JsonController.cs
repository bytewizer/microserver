using System;

using Bytewizer.TinyCLR.Http.Mvc;

namespace Bytewizer.TinyCLR.TestHarness
{
    public class JsonController : Controller
    {
        private readonly Person _person;
        
        public JsonController()
        {
            _person = new Person()
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
        }

        public IActionResult GetJsonPerson()
        {
            return Json(_person);
        }
    }
}