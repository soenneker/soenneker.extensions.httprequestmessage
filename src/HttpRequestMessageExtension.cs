using Soenneker.Extensions.HttpContent;
using Soenneker.Extensions.ValueTask;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Extensions.HttpRequestMessage;

public static class HttpRequestMessageExtension
{
    /// <summary>
    /// Creates a deep copy of the specified <see cref="System.Net.Http.HttpRequestMessage"/> instance, including its
    /// headers, properties, and content.
    /// </summary>
    /// <remarks>The cloned request will have the same HTTP method, request URI, version, version policy,
    /// headers, options, and content as the original. If the original request contains content, the content is also
    /// cloned. The returned request is independent of the original and can be sent or modified separately. Note that
    /// some properties, such as the request's underlying transport state, are not copied.</remarks>
    /// <param name="request">The <see cref="System.Net.Http.HttpRequestMessage"/> to clone. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous cloning of the request content. The default
    /// value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> that represents the asynchronous operation. The result contains a new <see
    /// cref="System.Net.Http.HttpRequestMessage"/> instance with the same method, URI, headers, options, and content as
    /// the original request.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
    public static ValueTask<System.Net.Http.HttpRequestMessage> Clone(this System.Net.Http.HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        // Fast path: no async state machine if there is no content
        if (request.Content is not null)
            return CloneWithContent(request, cancellationToken);

        var clone = new System.Net.Http.HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (KeyValuePair<string, object?> option in request.Options)
            clone.Options.TryAdd(option.Key, option.Value);

        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        return new ValueTask<System.Net.Http.HttpRequestMessage>(clone);
    }

    private static async ValueTask<System.Net.Http.HttpRequestMessage> CloneWithContent(System.Net.Http.HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new System.Net.Http.HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        clone.Content = await request.Content!.Clone(cancellationToken: cancellationToken)
                                     .NoSync();

        foreach (KeyValuePair<string, object?> option in request.Options)
            clone.Options.TryAdd(option.Key, option.Value);

        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        return clone;
    }
}