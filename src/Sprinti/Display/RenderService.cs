using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Microsoft.Extensions.Options;

namespace Sprinti.Display;

public interface IRenderService
{
    BitmapImage Render(string text, int x = 0, int y = 0);
}

public class RenderService(IOptions<DisplayOptions> options) : IRenderService
{
    public BitmapImage Render(string text, int x = 0, int y = 0)
    {
        var image = BitmapImage.CreateBitmap(options.Value.Width, options.Value.Height, PixelFormat.Format32bppArgb);
        image.Clear(Color.Black);
        var g = image.GetDrawingApi();
        g.DrawText(text, options.Value.Font, options.Value.FontSize, Color.White, new Point(x, y));
        return image;
    }
}