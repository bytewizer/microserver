using System;

using Bytewizer.TinyCLR.Http;

using Bytewizer.Playground.Json.Models;

namespace Bytewizer.Playground.Json
{
    class Program
    {
        private static Person[] _persons;

        static void Main()
        {
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
                        endpoints.Map("/person", context =>
                        {
                            // Post a request with the following content as the body and content type of application/json.
                            // {"Id": 100,"Suffix": "I", "Title": "Mr.","LastName": "Crona","Phone":
                            // "(458)-857-7797","Gender": 0,"FirstName": "Roscoe","MiddleName": "Jerald","Email": "Roscoe@gmail.com",
                            // "DOB": "2017-01-01T00:00:53.967Z"}

                            if (context.Request.Method != HttpMethods.Post)
                            {
                                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                                return;
                            }

                            if (context.Request.ReadFromJson(typeof(Person)) is Person person)
                            {
                                string response = "<doctype !html><html><head><title>Hello, world!" +
                                "</title></head><body><h1>" + person.FirstName + " " + person.LastName + "</h1></body></html>";

                                context.Response.StatusCode = StatusCodes.Status202Accepted;
                                context.Response.Write(response);
                            }
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status204NoContent;
                            }
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
                Suffix = "I",
                Phone = "(458)-857-7797"
            };
            _persons[0] = rjc;

            var jpc = new Person()
            {
                Id = 101,
                FirstName = "Jennifer",
                MiddleName = "Parker",
                LastName = "Crona",
                Title = "Ms.",
                DOB = DateTime.Now,
                Email = "Jpc@gmail.com",
                Gender = Gender.Female,
                Suffix = "K",
                Phone = "(342)-337-4397"
            };
            _persons[1] = jpc;
        }
    }
}