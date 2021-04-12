# Diagnostics

## Developer Exception Page

Captures asynchronous Exception instances from the pipeline and generates HTML error responses. Use the
UseDeveloperException() extension method to render the exception during the development mode. This method
adds middleware into the request pipeline which displays developer-friendly exception detail page. This
middleware should not be used in production.

```CSharp
options.Pipeline(app =>
{
    app.UseDeveloperExceptionPage(); // Should be called first in the pipeline.
});
```