# Json

Enables functionality for serializing and deserializing from JavaScript Object Notation (JSON).

### Json Serializing
Writes the HTTP content that results from serializing the content as Json.

```CSharp
private static Person[] _persons;

static void Main()
{
    InitJson();

    var server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/json", context =>
                {
                    context.Response.WriteJson(_persons);
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
```

### Json Deserializing
Reads the HTTP content and returns the value that results from deserializing the content as Json.

```CSharp
static void Main()
{
    var server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            endpoints.Map("/json", context =>
            {
                // Post a request with the following content as the body and content type of application/json.
                // {"Id": 100,"Suffix": "I","SSN": "939-69-5554","Title": "Mr.","LastName": "Crona","Phone": "(458)-857-7797",
                // "Gender": 0,"FirstName": "Roscoe","MiddleName": "Jerald","Email": "Roscoe@gmail.com","DOB": "2017-01-01T00:00:53.967Z"}
    
                if (context.Request.ReadFromJson(typeof(Person)) is Person person)
                {
                    string response = "<doctype !html><html><head><title>Hello, world!" +
                    "</title></head><body><h1>" + person.FirstName + " " + person.LastName + "</h1></body></html>";

                    context.Response.Write(response);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                }
            });
        });
    });
    server.Start();
}
```

## TinyCLR Packages
```bash
PM> Install-Package Bytewizer.TinyCLR.Http.Json
```

## RFC - Related Request for Comments 
- [RFC 7159 - The JavaScript Object Notation (JSON) Data Interchange](https://tools.ietf.org/html/rfc7159)