# Stubble View Engine  

A light-weight templating engine simular to <b>Mustache</b> or <b>Razor</b> used for mutating HTML documents.

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

## Show Blocks of HTML
You can render optional blocks of content when neccessary.

```Html
<h3>{{title}}</h3>
{{has-author}}
	<span>Published by {{author}}</span>
{{/has-author}}
```

Now you can use Stubble to render the optional information.

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

## Import Partial Templates
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
        ViewData.Child("footer")["foot"] = "<h3>Shared Footer Content</h3>";

        return View(@"views\home\index.html");
    }
}
```

## Bind Objects to Templates
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
Now you can use Stubble to bind variables within your template.

```Csharp
public class HomeController: Controller
{
    public IActionResult Index()
    {
        ViewData["title"] = "Welcome to Stubble";
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