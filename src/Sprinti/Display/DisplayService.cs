using Iot.Device.Graphics;

namespace Sprinti.Display;

public interface IDisplayService
{
    void UpdateProgress(string text, string text2 = "");
    void Print(string text);
}

internal class DisplayService(GraphicDisplay display, IRenderService renderService, ILogger<DisplayService> logger)
    : IDisplayService
{
    private const int MaxProgress = 5;
    private int _progress;

    public void Print(string text)
    {
        display.ClearScreen();
        using var image = renderService.Render(text);
        display.DrawBitmap(image);
    }

    public void UpdateProgress(string text, string text2 = "")
    {
        var percent = IncreaseProgress();
        var textToRender = $"{percent}% - {text2}\n{text}";

        logger.LogInformation("Display: {Text}", textToRender);
        using var image = renderService.Render(textToRender);

        display.ClearScreen();
        display.DrawBitmap(image);
    }

    private int IncreaseProgress()
    {
        _progress %= MaxProgress;
        _progress += 1;
        var percent = 100 / MaxProgress * _progress;
        return percent;
    }
}