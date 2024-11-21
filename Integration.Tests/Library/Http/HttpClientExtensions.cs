namespace Integration.Tests.Library.Http;

internal static class HttpClientExtensions
{
    public static HttpClient WithHeader(this HttpClient client, string name, string? value)
    {
        client.DefaultRequestHeaders.Remove(name);
        client.DefaultRequestHeaders.Add(name, value);
        return client;
    }
}
