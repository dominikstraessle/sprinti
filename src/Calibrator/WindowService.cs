using OpenCvSharp;
using Sprinti.Detection;

namespace Calibrator;

public class WindowService(string filename, Mat image)
{
    private readonly IList<int> _lookup = [];
    private readonly IList<int[]> _points = [];
    private readonly IList<int[]> _selectorPoints = [];
    private int _key = -1;

    public LookupConfig? Calibrate(CancellationToken stoppingToken)
    {
        Cv2.NamedWindow(filename);
        Cv2.ResizeWindow(filename, image.Width * 2, image.Height * 2);
        Cv2.ImShow(filename, image);
        Cv2.SetMouseCallback(filename, MouseClick);

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Next:");
            var key = Cv2.WaitKey();

            if (key is 'q' or 'n') break;

            _key = key - 176;

            PrintSelector();
        }

        Cv2.DestroyWindow(filename);

        if (_selectorPoints.Count != 2 || _points.Count < 3) return null;

        return new LookupConfig(
            new SelectorPoints(_selectorPoints[0], _selectorPoints[1]),
            _points.ToArray(),
            _lookup,
            filename
        );
    }

    private void PrintSelector()
    {
        switch (_key)
        {
            case >= 0 and <= 7:
            {
                Console.WriteLine($"Select {_key}");
                return;
            }
            case 8:
            {
                Console.WriteLine("Select white");
                return;
            }
            case 9:
            {
                Console.WriteLine("Select black");
                return;
            }
        }
    }

    private void MouseClick(MouseEventTypes mouseEventTypes, int x, int y, MouseEventFlags flags,
        IntPtr userdata)
    {
        if (mouseEventTypes is not MouseEventTypes.LButtonDown) return;

        Circle(x, y, 2, "");
        Console.WriteLine("Click");

        switch (_key)
        {
            case >= 0 and <= 7:
            {
                _points.Add([x, y]);
                _lookup.Add(_key);
                Console.WriteLine($"Point: ({x}, {y}) is key {_key}");
                Circle(x, y, 4, $"{_key}");
                break;
            }
            case 8:
            {
                _selectorPoints.Add([x, y, 255]);
                Console.WriteLine($"Selector: ({x}, {y}) is key white");
                Circle(x, y, 4, "w");
                break;
            }
            case 9:
            {
                _selectorPoints.Add([x, y, 0]);
                Console.WriteLine($"Selector: ({x}, {y}) is key black");
                Circle(x, y, 4, "b");
                break;
            }
        }
    }

    private void Circle(int x, int y, int thickness, string text)
    {
        Cv2.Circle(image, x, y, 3, 255, thickness);
        if (!string.IsNullOrEmpty(text))
        {
            Cv2.Line(image, new Point(x, 0), new Point(x, image.Height - 1), new Scalar(255));
            Cv2.Line(image, new Point(0, y), new Point(image.Width - 1, y), new Scalar(255));
            Cv2.PutText(image, text, new Point(x, y), HersheyFonts.HersheyTriplex, 1, new Scalar(0));
        }

        Cv2.ImShow(filename, image);
    }
}