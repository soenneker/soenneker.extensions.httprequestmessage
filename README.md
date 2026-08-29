[![](https://img.shields.io/nuget/v/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpRequestMessage
Clones an `HttpRequestMessage` so it can be modified or sent independently of the original request.

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

using HttpRequestMessage retry = await original.Clone();
retry.Headers.Add("X-Retry", "1");
```

`Clone()` copies the method, URI, HTTP version and version policy, request headers, options, and content headers/body. Content is buffered into a separate stream, so disposing or consuming one request does not consume the other's body. Transport state associated with a request that has already been sent is not copied.

The method returns `ValueTask<HttpRequestMessage>`. It completes synchronously when there is no content; content cloning is asynchronous and honors cancellation. The caller owns and must dispose the returned request. Passing null throws `ArgumentNullException`.
