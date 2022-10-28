using Microsoft.OpenApi.Readers;
using System.Text;
using System.IO;

namespace OpenAPI.SourceGenerator;
internal static class HttpClientGenerator
{
    public static IEnumerable<(string filename, string content)> GenerateHttpClientClass(ClassDeclarationSyntax classDeclaration, GeneratorExecutionContext context)
    {
        var namespaceName = NamespaceParser.GetNamespaceName(classDeclaration);
        var openApiAttribute = classDeclaration.AttributeLists
            .SelectMany(c => c.Attributes)
            .FirstOrDefault(c => c.Name.ToString() == "OpenApiClient");
        if (openApiAttribute is null) Array.Empty<(string filename, string content)>();
        var entries = new List<(string filename, string content)>();
        var firstArg = openApiAttribute!.ArgumentList!.Arguments!.FirstOrDefault()?.ToString().Replace("\"", "");
        var filePath = context.AdditionalFiles.FirstOrDefault(c => c.Path.EndsWith(firstArg));
        if (filePath is null)
        {
            var msg = string.Join(Environment.NewLine, context.AdditionalFiles.Select(c => c.Path));
            entries.Add(($"{classDeclaration.Identifier.Text}.g.cs", $"// {firstArg} | {msg}"));
            return entries;
        }
        var file = File.ReadAllText(filePath.Path);
        var fileBuffer = new MemoryStream(Encoding.UTF8.GetBytes(file ?? ""));
        var content = new OpenApiStreamReader().ReadAsync(fileBuffer).Result;

        var enumValues = new List<string>();

        foreach (var entity in content.OpenApiDocument.Paths)
        {
            foreach (var param in entity.Value.Operations.Values.SelectMany(c => c.Parameters))
            {
                if (param.Schema?.Type == "string" && (param.Schema?.Enum?.Any() ?? false))
                {
                    var enumName = param.Schema.GetMatchingType(entity.Key);
                    var sourceContent = ModelParser.ParseEnum(param.Schema, enumName, namespaceName);
                    context.AddSource($"{enumName}.g.cs", SourceText.From(sourceContent, Encoding.UTF8));
                    enumValues.Add(enumName);
                }
            }
        }

        foreach (var entity in content.OpenApiDocument.Components.Schemas)
        {
            var properties = new List<(string propertyType, string propertyName, string? description, bool obsolete)>();
            foreach (var contentValue in entity.Value.Properties)
            {
                string objectTypeString = contentValue.Value.GetMatchingType(entity.Key);
                var foundEnumValue = enumValues.FirstOrDefault(c => string.Equals(objectTypeString, c, StringComparison.OrdinalIgnoreCase));
                if (foundEnumValue is null)
                {
                    properties.Add((objectTypeString, contentValue.Key, contentValue.Value.Description, contentValue.Value.Deprecated));
                    continue;
                }
                properties.Add((foundEnumValue, contentValue.Key, contentValue.Value.Description, contentValue.Value.Deprecated));
            }
            var sourceContent = ModelParser.ParseModels(namespaceName, entity.Key, properties, entity.Value);
            context.AddSource($"{entity.Key}.g.cs", SourceText.From(sourceContent, Encoding.UTF8));
        }
        var requestsWithParams = content.OpenApiDocument.Paths.Select(path =>
        {
            var allPathParams = path.Value.Parameters.Where(c => c.Schema.Enum is not null && c.Schema.Enum.Any());
            return (path.Key, allPathParams);
        });

        var client = HttpClientParser.ParseClient(classDeclaration, content.OpenApiDocument);
        context.AddSource($"{classDeclaration.Identifier.Text}.g.cs", client);
        return entries;
    }


}
