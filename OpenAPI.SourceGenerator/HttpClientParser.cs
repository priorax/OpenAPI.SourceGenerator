using Microsoft.OpenApi.Models;
using System.Text;

namespace OpenAPI.SourceGenerator;

internal static class HttpClientParser
{
    public static string ParseClient(ClassDeclarationSyntax classDeclaration, OpenApiDocument apiDoc)
    {
        var namespaceName = NamespaceParser.GetNamespaceName(classDeclaration);

        var description = new StringBuilder();
        if (!string.IsNullOrEmpty(apiDoc.Info.Description))
        {
            description.AppendLine("\t//<summary>");
            foreach (var row in apiDoc.Info.Description.Split('\n'))
            {
                description.AppendLine($"\t// {row}");
            }
            description.Append("\t//</summary>");
        }

        var methods = new List<string>();
        var ignoredMehodTypes = new[] { OperationType.Head, OperationType.Options };
        foreach (var path in apiDoc.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                if (ignoredMehodTypes.Contains(operation.Key)) continue;
                var content = operation.Key switch
                {
                    OperationType.Put => MethodParsers.GenerateWriteMethod(operation, path.Key),
                    OperationType.Patch => MethodParsers.GenerateWriteMethod(operation, path.Key),
                    OperationType.Post => MethodParsers.GenerateWriteMethod(operation, path.Key),
                    OperationType.Get => MethodParsers.GenerateGetMethod(operation, path.Key),
                    OperationType.Delete => MethodParsers.GenerateDeleteMethod(operation, path.Key),
                    _ => ""
                };
                if (!string.IsNullOrEmpty(content))
                {
                    methods.Add(content);
                }
            }
        }
        var allMethodLines = methods.SelectMany(c => c.Split('\n')).Select(methodLine => "\t\t" + methodLine);
        var start = $@"
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using {namespaceName}.Models;
namespace {namespaceName}
{{
{description.ToString()}
    {classDeclaration.Modifiers} class {classDeclaration.Identifier.Value}
    {{
";

        var end = "\t}\n}";
        return start + string.Join("\n", allMethodLines) + end;
    }


}