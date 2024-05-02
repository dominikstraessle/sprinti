using Microsoft.Extensions.Logging;
using Sprinti.Button;

namespace Sprinti.Tests.Button;

public class ButtonTest(ILogger<ButtonService> logger)
{
    [Fact]
    public async Task TestButtonPressTimeout()
    {
        var testButton = new TestButton();
        var service = new ButtonService(testButton, logger);
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromTicks(1));
        var exception = await Record.ExceptionAsync(() => service.WaitForSignalAsync(cancellationTokenSource.Token));
        Assert.IsType<TimeoutException>(exception);
    }

    [Fact]
    public async Task TestButtonPress()
    {
        var testButton = new TestButton();
        var service = new ButtonService(testButton, logger);
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var exception = await Record.ExceptionAsync(() =>
        {
            var waitForSignalAsync = service.WaitForSignalAsync(cancellationTokenSource.Token);
            testButton.PressButton();
            testButton.ReleaseButton();
            return waitForSignalAsync;
        });
        Assert.Null(exception);
    }
}