using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Microsoft.Extensions.Options;

namespace Sprinti.Display;

internal class DisplayService(GraphicDisplay display, IOptions<DisplayOptions> options, ILogger<DisplayService> logger)
    : IDisplayService
{
    public void Debug(string text)
    {
        display.ClearScreen();
        using var image = Render(text);
        display.DrawBitmap(image);
    }

    private BitmapImage Render(string text, int x = 0, int y = 0)
    {
        var image = BitmapImage.CreateBitmap(options.Value.Width, options.Value.Height, PixelFormat.Format32bppArgb);
        image.Clear(Color.Black);
        var g = image.GetDrawingApi();
        g.DrawText(text, options.Value.Font, options.Value.FontSize, Color.White, new Point(x, y));
        return image;
    }

    public void UpdateProgress(int stepNumber, string text)
    {
        var textToRender = $"{stepNumber}: {text}";
        logger.LogInformation("Display: {step} - {text}", stepNumber, text);
        using var image = Render(textToRender);
        display.ClearScreen();
        display.DrawBitmap(image);
    }
}