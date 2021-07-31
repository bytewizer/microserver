# Diagnostics

## Developer Exception Page

Enables and captures exception instances from the pipeline and generates HTML error responses. Use the
UseDeveloperException() extension method to render the exception during the development mode. This method
adds middleware into the request pipeline which displays developer-friendly exception detail page. This
middleware should not be used in production.

```CSharp
options.Pipeline(app =>
{
    app.UseDeveloperExceptionPage(); // Should be called first in the pipeline.
});
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Http.Diagnostics
```