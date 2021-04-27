using System;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Http
{
    class Program
    {
        private static Person[] _persons;

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            InitJson();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                              "<style>body { background-color: #43bc69 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";
                            
                            context.Response.Write(response);
                        });
                        endpoints.Map("/json", context =>
                        {
                            context.Response.WriteJson(_persons, true);
                        });
                    });
                });
            });
            server.Start();
        }

        static void InitJson()
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
            _persons[0] = rjc;

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

        public enum Gender
        {
            Male,
            Female
        }
        
        public class Person
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string MiddleName { get; set; }

            public string LastName { get; set; }

            public string Title { get; set; }

            public DateTime DOB { get; set; }

            public string Email { get; set; }

            public Gender Gender { get; set; }

            public string SSN { get; set; }

            public string Suffix { get; set; }

            public string Phone { get; set; }
        }
    }
}