using Microsoft.OpenApi.Models;

namespace OpenAPI.SourceGenerator;

public static class OpenApiSchemaExtensions
{
    public static string GetMatchingType(this OpenApiSchema contentValue, string path)
    {
        var objectType = contentValue switch
        {
            { Type: "integer" } => "int",
            { Type: "string", Format: "date-time" } => "DateTime",
            { Type: "boolean" } => "bool",
            { Type: "string" } => "string",
            { Items: not null, } => "item",
            _ => "object"
        };

        if (objectType == "string" && (contentValue.Enum?.Any() ?? false))
        {
            if (!string.IsNullOrEmpty(contentValue.Description))
            {
                return contentValue.Description.Replace(" ", "");
            }
            return string.Join("",
                path.Split('/')
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => CultureDetails.TextInfo.ToTitleCase(c)
                ));
        }

        if (objectType == "object" && !string.IsNullOrEmpty(contentValue.Reference?.Id))
            return contentValue.Reference.Id;
        if (objectType == "item")
        {
            if (!string.IsNullOrEmpty(contentValue.Items?.Reference?.Id))
                objectType = contentValue.Items.Reference.Id;
            else
                objectType = contentValue.Items.Type;
        }
        var isArray = !string.IsNullOrEmpty(contentValue.Type) && contentValue.Type == "array";
        var objectTypeString = isArray ? $"IEnumerable<{objectType}>" : objectType;
        if (contentValue.Nullable)
            objectTypeString += "?";
        return objectTypeString;
    }
}
