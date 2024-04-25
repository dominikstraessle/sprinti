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
        const int fontSize = 25;
        const string font = "DejaVu Sans";
        const int y = 0;
        using var image =
            BitmapImage.CreateBitmap(options.Value.Width, options.Value.Height, PixelFormat.Format32bppArgb);
        image.Clear(Color.Black);
        var g = image.GetDrawingApi();
        g.DrawText(text, font, fontSize, Color.White, new Point(0, y));
        display.DrawBitmap(image);
    }

    public Task UpdateProgress(int stepNumber, string text, CancellationToken cancellationToken)
    {
        logger.LogInformation("Display: {step} - {text}", stepNumber, text);
        return Task.CompletedTask;
    }
}