[![](https://img.shields.io/nuget/v/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.httprequestmessage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.httprequestmessage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httprequestmessage/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httprequestmessage/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpRequestMessage
A collection of helpful HttpRequestMessage extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpRequestMessage
```

## Quick start

```csharp
using Soenneker.Extensions.HttpRequestMessage;

// Given an existing System.Net.Http.HttpRequestMessage named request:
var result = request.Clone();
```

## Common operations

- `Clone()` - Creates a deep copy of the specified `System.Net.Http.HttpRequestMessage` instance, including its headers, properties, and content.
