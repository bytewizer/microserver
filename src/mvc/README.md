# Model-View-Controller (MVC) Examples

## Controllers
The controller takes the result of the model's processing and returns either the proper view and its associated view data or the result of the API call. Controller Actions can return anything that produces a response. The action method is responsible for choosing what kind of response. The action result does the responding.  Controllers include the following features:

* <b>Html Context</b> - Encapsulates all HTTP-specific information about an individual HTTP request.
* <b>Action Context<b> - Including OnActionExecuting, OnActionExecuted, OnExecption methods.
* <b>Action Results</b> - Including View, Content, Json File, Ok, Redirect, BadRequest, NotFound and StatusCode responses.
* <b>Action Filters</b> - Are called before the action executes, after executed and on errors.

```Csharp
public class ExampleController : Controller
{
    // Any public IActionResult method inherited from Controller is made available as an endpoint
    public IActionResult GetById(long id)
    {
        string response = "<doctype !html><html><head><title>Hello, world!</title>" +
            "<style>body { background-color: #111 }" +
            "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
            "<body><h1>" + $"{id}" + "</h1></body></html>\r\n";

        return Content(response, "text/html");
    }
}
```
## Model binding for HTTP requests

Controllers and Stubble pages work with data that comes from HTTP requests.

```Html
http://device.bytewizer.local/example/getbyid?id=10
```
Model binding goes through the following steps after the routing system selects the action method:

* Finds the first parameter of GetByID, an integer named id.
* Converts the string "2" into integer 2.

The simple types that the model binder can convert source strings into include the following:

* Boolean
* Byte, SByte, Byte[]
* Char
* Decimal
* Double
* Int16, Int32, Int64
* Single
* UInt16, UInt32, UInt64

## Using HttpContext from a controller

Controllers expose the HttpContext property:

```Csharp
public class HomeController : Controller
{
    public IActionResult About()
    {
        var pathBase = HttpContext.Request.PathBase;

        return Content(pathBase, "text/html");
    }
}
```

## Using Json From a Controller

```Csharp
public class JsonData
{
    public JsonData()
    {
        Name = "Bob Smith";
        Address = "103 Main Street, Burbank, CA 92607";
    }

    public string Name { get; set; }
    public string Address { get; set; }
}


public IActionResult GetJson()
{
    var jsonObject = new JsonData();

    return Json(jsonObject);
}

public IActionResult PostJsonBody()
{
    // Example json posted to the PostJsonBody() endpoint
    // {"TeamId":288272,"PlayerName":"Nazem Kadri","Points":18,"Goals":10,"Assists":16}

    if (HttpContext.Request.Body == null 
        || HttpContext.Request.Method != HttpMethods.Post)
    {
        return BadRequest();
    }

    try
    {
        var stream = HttpContext.Request.Body;
        var player = (PlayerModel)JsonConverter.DeserializeObject(stream, typeof(PlayerModel));
        Debug.WriteLine($"Team ID:{player.TeamId} - Name:{player.PlayerName}");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex.Message);
        return BadRequest();
    }

    return Ok();
}
```

## Controller Error Handeling

```Csharp
public class ErrorController : Controller
{     
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // called before action Throw() method    
        if (filterContext.HttpContext.Request.Method != HttpMethods.Get)
        {
            filterContext.Result = new BadRequestResult();
        }
    }

    public override void OnException(ExceptionContext filterContext)
    {
        // called on action Throw() method execption
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
            
        filterContext.ExceptionHandled = true;
        filterContext.Result = new ContentResult
        {
            Content = $"An error occurred in the {actionName} action.",
            ContentType = "text/plain",
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    public IActionResult Throw()
    {
        throw new ArgumentNullException("thowing server error");
    }
}
```

## Stubble  

A light-weight templating engine simular to <b>Razor</b> used for mutating HTML documents.

### Usage with HTML

```Html
<html>
    <head></head>
    <body>
        {{content}}
    </body>
    <footer>
        {{foot}}
    </footer>
</html>
{{scripts}}
```
Now you can use Stubble to replace the variables within your html file with controller data.

```Csharp
public class HomeController: Controller
{
    public IActionResult Index()
    {
        ViewData["content"] = "Welcome to Stubble";
        ViewData["footer"] = "Bytewizer 2020";
        ViewData["scripts"] = "<script src="/assets/js/jquery-3.5.1.min.js"></script>";
        
        return View(@"views\home\index.html");
    }
}
```

### Show Blocks of HTML
You can render optional blocks of content when neccessary.

```Html
<h3>{{title}}</h3>
{{has-author}}
	<span>Published by {{author}}</span>
{{/has-author}}
```

Now you can use Stubble to render the optional park information.

```Csharp
public class HomeController: Controller
{
    public IActionResult Index()
    {
        ViewData["title"] = "Welcome to Stubble";
        ViewData["author"] = "Mark Smith";
        ViewData.Show("has-author");

        return View(@"views\home\index.html");
    }
}
```

### Import partial Templates
You can also import partial templates from within a template.

```Html
<html>
    {{header "views/shared/header.html"}}
    <body>
        {{content}}
    </body>
    {{footer "views/shared/footer.html"}}
</html>
```

Source for partial view <code>views/shared/head.html</code>

```Html
<head>
    {{head}}
</head>
```

Source for partial view <code>views/shared/footer.html</code>

```Html
<footer>
    {{foot}}
</footer>
```

```Csharp
public class HomeController: Controller
{
    public IActionResult Index()
    {
        ViewData["content"] = "Welcome to Stubble";
        ViewData.Child("header")["head"] = "<h3>Shared Header Content</h3>";
        ViewData.Child("foot")["foot"] = "<h3>Shared Footer Content</h3>";

        return View(@"views\home\index.html");
    }
}
```

### Bind Objects to Templates
You can bind complex C# objects to templates.

```Html
<html>
    <head>
        <h3>{{title}}</h3>
    </head>
    <body>
        <p>{{teamid}} - {{playername}} - {{points}} - {{goals}} - {{assists}}</p>
    </body>
    <footer></footer>
</html>
```
Now you can use Stubble to  to bind variables within your template.

```Csharp
public class HomeController: Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Welcome to Stubble";
        ViewData.Bind(
            new PlayerModel()
            {
                TeamId = 288272,
                PlayerName = "Nazem Kadri",
                Points = 18,
                Goals = 10,
                Assists = 16
            });

        return View(@"views\home\index.html");
    }
}
```