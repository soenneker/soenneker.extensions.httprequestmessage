[![](https://img.shields.io/nuget/v/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpRequestMessage
Extension methods for copying, inspecting, and preparing `HttpRequestMessage` instances for sending, retrying, and diagnostics.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpRequestMessage
```

## Usage

```csharp
using Soenneker.Extensions.HttpRequestMessage;

using var original = new HttpRequestMessage(HttpMethod.Post, "https://api.example.com/orders")
{
    Content = JsonContent.Create(order)
};

using HttpRequestMessage retry = await original.Clone(cancellationToken: cancellationToken);

using HttpResponseMessage firstResponse = await httpClient.SendAsync(original, cancellationToken);

if (!firstResponse.IsSuccessStatusCode)
{
    retry.Headers.Add("X-Retry", "1");
    using HttpResponseMessage retryResponse = await httpClient.SendAsync(retry, cancellationToken);
}
```

Create every clone before sending the original when its content may be backed by a non-seekable stream. Each request message can then be sent once.

`Clone()` copies the method, URI, HTTP version and version policy, request headers, options, and content headers/body. The body is buffered independently, so disposing or consuming one request does not consume the other's body. Transport state associated with a request that has already been sent is not copied.

Option values are copied by reference. If an option contains a mutable object, the original and clone still share that object.

An application already using pooled memory streams can pass its scoped utility while retaining ownership of the long-lived client:

```csharp
using HttpRequestMessage copy = await request.Clone(memoryStreamUtil, cancellationToken);
```

The method returns `ValueTask<HttpRequestMessage>`. It completes synchronously when there is no content; content cloning is asynchronous and honors cancellation. The caller owns and must dispose the returned request. Passing null throws `ArgumentNullException`.
