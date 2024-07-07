using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.HttpContent;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Extensions.HttpRequestMessage;

/// <summary>
/// A collection of helpful HttpRequestMessage extension methods
/// </summary>
public static class HttpRequestMessageExtension
{
    /// <summary>
    /// Creates a deep clone of the given <see cref="HttpRequestMessage"/> including headers, options, and content.
    /// </summary>
    /// <param name="request">The original <see cref="HttpRequestMessage"/> to clone.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with a cloned <see cref="HttpRequestMessage"/> as the result.</returns>
    public static async ValueTask<System.Net.Http.HttpRequestMessage> Clone(this System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var clone = new System.Net.Http.HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        if (request.Content != null)
        {
            clone.Content = await request.Content.Clone(cancellationToken: cancellationToken).NoSync();
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