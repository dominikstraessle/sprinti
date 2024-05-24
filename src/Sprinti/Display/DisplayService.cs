using Iot.Device.Graphics;

namespace Sprinti.Display;

public interface IDisplayService
{
    void UpdateProgress(int stepNumber, string text);
    void Debug(string text);
}

internal class DisplayService(GraphicDisplay display, IRenderService renderService, ILogger<DisplayService> logger)
    : IDisplayService
{
    public void Debug(string text)
    {
        display.ClearScreen();
        using var image = renderService.Render(text);
        display.DrawBitmap(image);
    }

    public void UpdateProgress(int stepNumber, string text)
    {
        var textToRender = $"{stepNumber}: {text}";
        logger.LogInformation("Display: {step} - {text}", stepNumber, text);
        using var image = renderService.Render(textToRender);
        display.ClearScreen();
        display.DrawBitmap(image);
    }
}