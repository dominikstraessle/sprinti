using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Sprinti.Confirmation;
using Sprinti.Domain;
using static Sprinti.Confirmation.ModuleRegistry;

namespace Sprinti.Tests.Confirmation;

public class ConfirmationServiceTests
{
    private readonly ConfirmationOptions _confirmationOptions = new()
    {
        BaseAddress = new Uri("http://52.58.217.104:5000"),
        TeamName = "team29",
        Password = "noauth"
    };

    private readonly MockHttpMessageHandler _mockHttp;
    private readonly OptionsWrapper<ConfirmationOptions> _options;
    private readonly ConfirmationService _service;


    public ConfirmationServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _options = new OptionsWrapper<ConfirmationOptions>(_confirmationOptions);
        _service = new ConfirmationService(_mockHttp.ToHttpClient(), _options, new NullLogger<ConfirmationService>());
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
    public async Task TestStartAsync()
    {
        _mockHttp.When(_options.Value.CubesTeamStartPath).Respond(HttpStatusCode.OK);
        var exceptionAsync = await Record.ExceptionAsync(() => _service.StartAsync(CancellationToken.None));
        Assert.Null(exceptionAsync);
    }

    [Fact]
    public async Task TestEndAsync()
    {
        _mockHttp.When(_options.Value.CubesTeamEndPath).Respond(HttpStatusCode.OK);
        var exceptionAsync = await Record.ExceptionAsync(() => _service.EndAsync(CancellationToken.None));
        Assert.Null(exceptionAsync);
    }

    [Fact]
    public async Task TestConfirmAsync()
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

        const string expectedCubeConfigJson = """
                                              {"time":"2023-10-10 17:10:05","config":{"1":"red","2":"blue","3":"red","4":"yellow","5":"","6":"","7":"yellow","8":"red"}}
                                              """;

        _mockHttp.When(_options.Value.CubesTeamConfigPath)
            .WithContent(expectedCubeConfigJson)
            .Respond(HttpStatusCode.OK);
        var exceptionAsync = await Record.ExceptionAsync(() => _service.ConfirmAsync(config, CancellationToken.None));
        Assert.Null(exceptionAsync);
    }

    [Fact]
    public async Task TestHealthCheckAsyncOk()
    {
        _mockHttp.When(_options.Value.CubesPath).Respond(HttpStatusCode.OK);
        var result = await _service.HealthCheckAsync(CancellationToken.None);
        Assert.True(result);
    }

    [Fact]
    public async Task TestHealthCheckAsyncNotOk()
    {
        _mockHttp.When(_options.Value.CubesPath).Respond(HttpStatusCode.InternalServerError);
        var result = await _service.HealthCheckAsync(CancellationToken.None);
        Assert.False(result);
    }

    [Fact]
    public async Task TestHealthCheckAsyncError()
    {
        _mockHttp.When(_options.Value.CubesPath).Throw(new TimeoutException("simulated timeout"));
        var result = await _service.HealthCheckAsync(CancellationToken.None);
        Assert.False(result);
    }

    [Fact]
    public void TestPaths()
    {
        Assert.Equal("http://52.58.217.104:5000/cubes", _options.Value.CubesPath);
        Assert.Equal("http://52.58.217.104:5000/cubes/team29/start", _options.Value.CubesTeamStartPath);
        Assert.Equal("http://52.58.217.104:5000/cubes/team29/config", _options.Value.CubesTeamConfigPath);
        Assert.Equal("http://52.58.217.104:5000/cubes/team29/end", _options.Value.CubesTeamEndPath);
    }
}