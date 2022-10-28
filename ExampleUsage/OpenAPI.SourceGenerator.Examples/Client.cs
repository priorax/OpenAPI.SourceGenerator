namespace OpenAPI.SourceGenerator.Examples;

[OpenApiClient("sample.yaml")]
public partial class Client {
    private readonly HttpClient _client;
    public Client(HttpClient client) {
        _client = client;
    }
}