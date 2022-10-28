# OpenApi Parser as a Source Generator

__disclaimer__

Look... This... Code isn't good...

I wouldn't recommend it for use in production code at this point, but it was meant for me playing with Source Generators.

## With that said!

### What is this?

This is a Source Generator used to take an OpenAPI definition and can generate a strongly typed HTTP client using it at compile time.

### Example usage

```csharp
namespace OpenAPI.SourceGenerator.Examples;

[OpenApiClient("sample.yaml")]
public partial class Client {
    // At the moment this _exact_ field is required.
    // No other names work.
    private readonly HttpClient _client;
    public Client(HttpClient client) {
        _client = client;
    }
}
```

_Hypothetically_ this should code gen:
 - A strongly typed client based on a the input.
 - All models will be placed in the `OpenAPI.SourceGenerator.Examples` namespace.

An example of the files generated can be found in [the example project generated files directory](ExampleUsage\generated\sampleOutput\OpenAPI.SourceGenerator).

## Currently untested features:
  - GET endpoints
  - Delete endpoints
  - _some level_ of automated summary tags

## TODO:
 - POST/PATCH/PUT endpoints
 - Handle nullable vs non-nullable projects
 - Somehow write tests...
 - Find some way to handle naming of functions/properties to be more in line with C# standards.
 - Maybe allow some way for users to override 
 - CI
 - Fix complier warnings.
 - Ensure if this breaks, so does things that depend on it.
 - Find some way of dealing with incomplete OpenApi docs.
 - I would _kind of_ love the idea of allowing for remote pulling of OpenAPI docs as well as on disk...