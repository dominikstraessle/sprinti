using Microsoft.Extensions.Options;
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

/**
 * [rtsp @ 0x7f0d2bc51dc0] method DESCRIBE failed: 503 Service Unavailable
[ WARN:0@48.177] global cap_gstreamer.cpp:1173 isPipelinePlaying OpenCV | GStreamer warning: GStreamer: pipeline have not been created
[ERROR:0@48.177] global cap.cpp:164 open VIDEOIO(GSTREAMER): raised OpenCV exception:

OpenCV(4.9.0) /build/source/modules/videoio/src/cap_gstreamer.cpp:1468: error: (-215:Assertion failed) uridecodebin in function 'open'
 */
/**
 * [rtsp @ 0x7fb118002d00] method DESCRIBE failed: 503 Service Unavailable
[ WARN:4@119.693] global cap.cpp:204 open VIDEOIO(FFMPEG): backend is generally available but can't be used to capture by name
 */
public class StreamCaptureFactory(IOptions<StreamOptions> options)
{
    public StreamCapture Create()
    {
        return new StreamCapture(new VideoCapture(options.Value.RtspSource, options.Value.VideoCaptureAPIs));
    }
}