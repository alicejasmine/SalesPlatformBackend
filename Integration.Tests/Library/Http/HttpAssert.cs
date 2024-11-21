using System.Text.Json;
using System.Text.Json.Serialization;

namespace Integration.Tests.Library.Http;

public abstract class HttpAssert
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    static HttpAssert()
    {
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    private static async Task<T?> DeserializeResponseAs<T>(HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(responseAsString, _jsonSerializerOptions);
    }

    public static T CanDeserializeResponseAs<T>(HttpResponseMessage response)
    {
        T? result = default;

        Assert.DoesNotThrowAsync(
            async () => result = await DeserializeResponseAs<T>(response),
            $"Could not deserialize response with the specified type.");

        Assert.That(result, Is.Not.Null);

        return result;
    }

    public static void ResponseHasHeader(HttpResponseMessage response, string headerName)
    {
        Assert.That(response.Headers.Contains(headerName), $"Response does not contain header '{headerName}'.");
    }

    public static void ResponseHeaderHasValue(HttpResponseMessage response, string headerName, string expectedValue)
    {
        var values = response.Headers.GetValues(headerName).ToList();

        Assert.That(
            values,
            Has.Count.EqualTo(1),
            $"Header '{headerName}' has multiple values. Only one expected.");

        Assert.That(
            values[0],
            Is.EqualTo(expectedValue),
            $"Header '{headerName}' has an incorrect value. Expected: '{expectedValue}'. Actual: '{values[0]}'.");
    }
}
