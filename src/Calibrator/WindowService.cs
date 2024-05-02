using OpenCvSharp;
using Sprinti.Stream;

namespace Calibrator;

public class WindowService(string filename, Mat image)
{
    private readonly IList<int[]> _selectorPoints = [];
    private readonly IList<int[]> _points = [];
    private readonly IList<int> _lookup = [];
    private bool _active;
    static int trackbarValue = 0;

    public LookupConfig Calibrate()
    {
        Cv2.NamedWindow(filename);
        Cv2.ResizeWindow(filename, image.Size());
        Cv2.ImShow(filename, image);
        Cv2.CreateTrackbar("1", filename, ref trackbarValue, 7, (pos, data) => Console.WriteLine($"{pos} {data}"));

        Cv2.SetMouseCallback(filename, MouseClick);
        Cv2.WaitKey();
        Cv2.DestroyAllWindows();
        if (_selectorPoints.Count != 2)
        {
            throw new ArgumentException("Not enough selectors");
        }

        return new LookupConfig(
            new SelectorPoints(_selectorPoints.First(), _selectorPoints.Last()),
            _points.ToArray(),
            _lookup,
            filename
        );
    }

    // Mouse callback function
    private void MouseClick(MouseEventTypes mouseEventTypes, int x, int y, MouseEventFlags flags,
        IntPtr userdata)
    {
        if (mouseEventTypes is not (MouseEventTypes.LButtonDown or MouseEventTypes.RButtonDown) || _active)
        {
            return;
        }

        _active = true;

        Cv2.Circle(image, x, y, 3, 255, 2);
        Cv2.ImShow(filename, image);
        switch (mouseEventTypes)
        {
            case MouseEventTypes.LButtonDown:
            {
                Console.WriteLine("Which lookup? 0-7");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out var lookupNumber) || lookupNumber is < 0 or > 7) return;
                _points.Add([x, y]);
                _lookup.Add(lookupNumber);
                Cv2.Circle(image, x, y, 5, 0, 3);
                Cv2.ImShow(filename, image);
                Console.WriteLine($"Point: ({x}, {y}) is {lookupNumber}");
                _active = false;

                return;
            }
            case MouseEventTypes.RButtonDown:
            {
                Console.WriteLine("Which color? 0 - black | 1 - white");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out var lookupNumber) || lookupNumber is < 0 or > 1) return;
                var color = lookupNumber == 0 ? 0 : 255;
                _selectorPoints.Add([x, y, color]);
                Cv2.Circle(image, x, y, 5, 0, 2);
                Cv2.ImShow(filename, image);
                Console.WriteLine($"Selector: ({x}, {y}) is {lookupNumber}");
                _active = false;

                return;
            }
        }
    }
}