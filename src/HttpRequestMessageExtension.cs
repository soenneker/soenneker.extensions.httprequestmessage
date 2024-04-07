using System.Collections.Generic;
using System.Threading.Tasks;
using Soenneker.Extensions.HttpContent;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Extensions.HttpRequestMessage;

/// <summary>
/// A collection of helpful HttpRequestMessage extension methods
/// </summary>
public static class HttpRequestMessageExtension
{
    public static async ValueTask<System.Net.Http.HttpRequestMessage> Clone(this System.Net.Http.HttpRequestMessage request)
    {
        var clone = new System.Net.Http.HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        if (request.Content != null)
        {
            clone.Content = await request.Content.Clone().NoSync();
        }

        foreach (KeyValuePair<string, object?> option in request.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }

        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}