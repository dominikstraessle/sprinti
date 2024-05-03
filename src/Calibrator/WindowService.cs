using OpenCvSharp;
using Sprinti.Stream;

namespace Calibrator;

public class WindowService(string filename, Mat image)
{
    private readonly IList<int[]> _selectorPoints = [];
    private readonly IList<int[]> _points = [];
    private readonly IList<int> _lookup = [];
    private int _key = -1;

    public LookupConfig? Calibrate(CancellationToken stoppingToken)
    {
        Cv2.NamedWindow(filename);
        Cv2.ResizeWindow(filename, image.Size());
        Cv2.ImShow(filename, image);
        Cv2.SetMouseCallback(filename, MouseClick);

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Next:");
            var key = Cv2.WaitKey();

            if (key == 'q')
            {
                break;
            }

            _key = key - 176;

            PrintSelector();
        }

        Cv2.DestroyWindow(filename);

        if (_selectorPoints.Count != 2 || _points.Count < 3)
        {
            return null;
        }

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
        if (mouseEventTypes is not MouseEventTypes.LButtonDown)
        {
            return;
        }

        Circle(x, y, 2);
        Console.WriteLine("Click");

        switch (_key)
        {
            case >= 0 and <= 7:
            {
                _points.Add([x, y]);
                _lookup.Add(_key);
                Console.WriteLine($"Point: ({x}, {y}) is key {_key}");
                Circle(x, y, 4);
                break;
            }
            case 8:
            {
                _selectorPoints.Add([x, y, 255]);
                Console.WriteLine($"Selector: ({x}, {y}) is key white");
                Circle(x, y, 4);
                break;
            }
            case 9:
            {
                _selectorPoints.Add([x, y, 0]);
                Console.WriteLine($"Selector: ({x}, {y}) is key black");
                Circle(x, y, 4);
                break;
            }
        }
    }

    private void Circle(int x, int y, int thickness)
    {
        Cv2.Circle(image, x, y, 3, 255, thickness);
        Cv2.ImShow(filename, image);
    }
}