using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Sprinti.Confirmation;
using Sprinti.Domain;
using static Sprinti.Confirmation.ModuleRegistry;

namespace Sprinti.Tests.Confirmation;

public class ConfirmationAdapterTests
{
    private readonly ConfirmationAdapter _adapter;

    private readonly ConfirmationOptions _confirmationOptions = new()
    {
        BaseAddress = new Uri("http://52.58.217.104:5000"),
        TeamName = "team29",
        Password = "noauth"
    };

    public ConfirmationAdapterTests()
    {
        Mock<HttpMessageHandler> mockHttpMessageHandler = new();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        var options = new OptionsWrapper<ConfirmationOptions>(_confirmationOptions);
        _adapter = new ConfirmationAdapter(client, options, new NullLogger<ConfirmationAdapter>());
    }

    [Fact]
    public void TestClientFactory()
    {
        var client = new HttpClient();
        ConfigureClient(_confirmationOptions, client);
        Assert.Equal(_confirmationOptions.BaseAddress, client.BaseAddress);
        Assert.True(client.DefaultRequestHeaders.Contains(AuthHeaderName));
        Assert.Equal(_confirmationOptions.Password, client.DefaultRequestHeaders.GetValues(AuthHeaderName).First());
    }

    [Fact]
    public void TestConfirmationSerialization()
    {
        var config = new CubeConfig
        {
            Time = new DateTime(2023, 10, 10, 17, 10, 5, DateTimeKind.Utc),
            Config = new SortedDictionary<int, Color>
            {
                { 1, Color.Red },
                { 2, Color.Blue },
                { 3, Color.Red },
                { 4, Color.Yellow },
                { 5, Color.None },
                { 6, Color.None },
                { 7, Color.Yellow },
                { 8, Color.Red }
            }
        };
        const string expected = """
                                {"time":"2023-10-10 17:10:05","config":{"1":"red","2":"blue","3":"red","4":"yellow","5":"","6":"","7":"yellow","8":"red"}}
                                """;


        var result = ConfirmationAdapter.SerializeCubeConfig(config);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task TestStartAsync()
    {
        var exceptionAsync = await Record.ExceptionAsync(() => _adapter.StartAsync(CancellationToken.None));
        Assert.Null(exceptionAsync);
    }
}