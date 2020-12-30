# Model-View-Controller (MVC)

## Controllers
The controller takes the result of the model's processing and returns either the proper view and its associated view data or the result of the API call. Controller Actions can return anything that produces a response. The action method is responsible for choosing what kind of response. The action result does the responding.  Controllers include the following features:

* <b>Html Context</b> - Encapsulates all HTTP-specific information about an individual HTTP request.
* <b>Action Context</b> - Including OnActionExecuting, OnActionExecuted, OnExecption methods.
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
## Model Binding for HTTP requests

Controllers and Stubble pages work with data that comes from HTTP requests.

```Html
http://device.bytewizer.local/example/getbyid?id=10
```
Model binding goes through the following steps after the routing system selects the action method:

* Finds the first parameter of GetByID, an integer named id.
* Converts the string "2" into integer 2.

The simple types that the model binder can convert source strings into include the following:

* String, Boolean, Double
* Byte, SByte, Byte[]
* Int16, Int32, Int64
* UInt16, UInt32, UInt64

## Using HttpContext from a Controller

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