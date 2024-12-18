using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Extensions.HttpRequestMessage.Tests;

[Collection("Collection")]
public class HttpRequestMessageExtensionTests : FixturedUnitTest
{
    public HttpRequestMessageExtensionTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}
