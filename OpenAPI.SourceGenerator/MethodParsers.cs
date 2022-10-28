using Microsoft.OpenApi.Models;
using System.Text;

namespace OpenAPI.SourceGenerator;
internal static class MethodParsers
{
    public static string GenerateWriteMethod(KeyValuePair<OperationType, OpenApiOperation> operation, string path)
    {
        var operationName = operation.Value.OperationId;
        var description = GenerateDescription(operation.Value);
        var start = $"public Task {operationName}(){{throw new NotImplementedException();}}";
        return description + "\n" + start;
    }

    public static string GenerateDeleteMethod(KeyValuePair<OperationType, OpenApiOperation> operation, string path)
    {
        var methodBuilder = new List<string>();
        var description = GenerateDescription(operation.Value);
        var operationName = operation.Value.OperationId;
        var methodParams = operation.Value.Parameters.Where(c => c.In == ParameterLocation.Path).Select(param =>
        {
            return $"{param.Schema.GetMatchingType(path)} {param.Name}";
        });
        var start = $"public Task<HttpResponseMessage> {operationName}({string.Join(", ", methodParams)}) {{";
        methodBuilder.AddRange(GenerateDescription(operation.Value).Split('\n').Where(_ => !string.IsNullOrEmpty(_)));
        methodBuilder.Add(start);
        methodBuilder.Add($"\tvar requestUri = \"{path}\";");
        foreach (var arg in methodParams)
        {
            var param = arg.Split(' ').Last();
            methodBuilder.Add($"\trequestUri = requestUri.Replace(\"{{{param}}}\", {param}.ToString());");
        }
        methodBuilder.Add("\treturn _client.DeleteAsync(requestUri);");
        methodBuilder.Add("}");
        var lines = methodBuilder.Select(line => $"{line}").ToList();
        return string.Join("\n", lines);
    }

    public static string GenerateGetMethod(KeyValuePair<OperationType, OpenApiOperation> operation, string path)
    {
        var methodBuilder = new List<string>();
#nullable disable
        var successCode = operation.Value.Responses.FirstOrDefault(c => c.Key == "200");
        if (successCode.Key is null) return "";
#nullable restore
        var jsonResponse = successCode.Value.Content.First(c => c.Key.Contains("json"));
        var responseType = jsonResponse.Value.Schema.GetMatchingType(path);
        var methodName = operation.Value.OperationId;
        var methodDeclaration = $"public async Task<{responseType}> {methodName}";
        var methodParams = new List<string>();

        var queryParams = new List<(string, bool isEnumerable)>();
        var pathParams = new List<(string, bool isEnumerable)>();

        foreach (var param in operation.Value.Parameters)
        {
            var matchingType = param.Schema.GetMatchingType(path);
            if (param.In == ParameterLocation.Path) pathParams.Add((param.Name, matchingType.StartsWith("IEnumerable")));
            if (param.In == ParameterLocation.Query) queryParams.Add((param.Name, matchingType.StartsWith("IEnumerable")));
            methodParams.Add($"{matchingType} {param.Name}");
        }

        var methodParamsString = string.Join(",", methodParams);

        var methodSignature = $"{methodDeclaration}({methodParamsString}) {{";
        methodBuilder.AddRange(GenerateDescription(operation.Value).Split('\n'));
        methodBuilder.Add(methodSignature);
        if (queryParams.Any())
        {
            methodBuilder.Add("\tvar queryParamStrings = new List<string>();");
            foreach (var queryParam in queryParams)
            {
                if (queryParam.isEnumerable) {
                    methodBuilder.Add($"\tqueryParamStrings.Add($\"{queryParam.Item1}={{string.Join(',', {queryParam.Item1})}}\");"); ; }
                else
                {
                    methodBuilder.Add($"\tqueryParamStrings.Add($\"{queryParam.Item1}={{{queryParam.Item1}}}\");");
                }
            }
        }
        var queryPath = !queryParams.Any() ? $"\"{path}\"" : $"$\"{path}?{{string.Join(\"&\", queryParamStrings)}}\"";
        methodBuilder.Add($"\tvar path = {queryPath};");
        methodBuilder.Add("\tvar response = await _client.GetStreamAsync(path);");
        methodBuilder.Add($"\tvar content = await JsonSerializer.DeserializeAsync<{responseType}>(response);");
        methodBuilder.Add($"\treturn content!;");
        methodBuilder.Add("}");
        return string.Join("\n", methodBuilder.Select(row => $"{row}")) + "\n";
    }

    private static string GenerateDescription(OpenApiOperation operation)
    {
        if (string.IsNullOrEmpty(operation.Summary)) return "";
        var description = new StringBuilder();
        description.AppendLine("/// <summary>");
        foreach (var line in operation.Summary.Split('\n').Select(c => $"/// {c}"))
            description.AppendLine(line);
        description.Append("/// </summary>");
        return description.ToString();
    }
}
