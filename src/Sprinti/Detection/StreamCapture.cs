using OpenCvSharp;

namespace Sprinti.Detection;

public interface IStreamCapture
{
    bool Read(Mat mat);
}

public class StreamCapture(VideoCapture capture) : IStreamCapture
{
    public bool Read(Mat mat)
    {
        return capture.Read(mat);
    }
}