using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text;

namespace OpenAPI.SourceGenerator;
internal static class ModelParser
{
    public static string ParseEnum(OpenApiSchema schema, string enumName, string @namespace)
    {
        return $@"namespace {@namespace}.Models
{{
" + CreateEnum(enumName, schema) + @"
}";
    }

    private static string CreateEnum(string enumName, OpenApiSchema schema)
    {
        var str = new StringBuilder();
        str.Append("public enum ");
        str.Append(enumName);
        str.AppendLine(" {");
        foreach (var enumKey in schema.Enum)
        {
            str.AppendLine($"\t{((OpenApiString)enumKey).Value},");
        }
        str.AppendLine("}");
        return String.Join("\n", str.ToString().Split('\n').Select(c => $"\t\t{c}"));
    }

    public static string ParseModels(string @namespace, string className, IEnumerable<(string propertyType, string propertyName, string? description, bool obsolete)> properties, OpenApiSchema schema)
    {
        var start = $@"
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace {@namespace}.Models
{{
    public partial record " + className + @"
    {
";
        var body = new StringBuilder();

        var enums = new List<string>();

        foreach (var property in properties)
        {
            var (propertyType, propertyName, description, obsolete) = property;
            if (!string.IsNullOrEmpty(description))
            {
                body.AppendLine("\t\t/// <summary>");
                body.AppendLine($"\t\t/// {description}");
                body.AppendLine("\t\t/// </summary>");
            }
            if (obsolete)
            {
                body.AppendLine("\t\t[Obsolete]");
            }

            body.AppendLine($"\t\t[JsonPropertyName(\"{propertyName}\")]");
            var isOptional = schema.Required.Any(c => string.Equals(c, propertyName, StringComparison.OrdinalIgnoreCase));
            if (isOptional)
                propertyType += "?";
            var matchingSchemaEntry = schema.Properties.Single(c => c.Key == propertyName).Value;
            if (matchingSchemaEntry.Type == "string" && (matchingSchemaEntry.Enum?.Any() ?? false))
            {
                propertyType = matchingSchemaEntry.GetMatchingType(propertyName);
                enums.Add(CreateEnum(propertyType, matchingSchemaEntry));
            }
            body.AppendLine($"\t\tpublic {propertyType} {CultureDetails.TextInfo.ToTitleCase(propertyName)} {{ get; set; }}");
        }
        
        foreach (var parsedEnum in enums)
        {
            foreach (var line in parsedEnum.Split('\n'))
                body.AppendLine(line);
        }

        var end = "\t}\n}";


        return start + body.ToString() + end;
    }

}
