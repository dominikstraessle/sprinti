using OpenCvSharp;

namespace Sprinti.Detection;

public class StreamOptions : ISprintiOptions
{
    public const string Stream = "Stream";
    public string Username { get; set; } = "pren";
    public string Password { get; set; } = "463997";
    public string Host { get; set; } = "147.88.48.131/axis-media/media.amp?streamprofile=pren_profile_med";
    public string RtspSource => $"rtsp://{Username}:{Password}@{Host}";
    public bool Debug { get; set; } = true;
    public string DebugPathFromContentRoot { get; set; } = "debug";
    public VideoCaptureAPIs VideoCaptureAPIs { get; set; } = VideoCaptureAPIs.FFMPEG;
    public int ErrorTimeout { get; set; } = 250;
    public bool Enabled { get; set; } = true;
}

public class CaptureOptions : ISprintiOptions
{
    public const string Capture = "Capture";
    public string ImagePathFromContentRoot { get; set; } = "img";
    public int CaptureIntervalInFrames { get; set; } = 24;
    public bool Enabled { get; set; } = true;
}

public class MaskOptions : ISprintiOptions
{
    // https://pseudopencv.site/utilities/hsvcolormask/
    public const string Mask = "Mask";
    public Scalar LowerBlue { get; set; } = new(50, 70, 0);
    public Scalar UpperBlue { get; set; } = new(160, 255, 255);
    public Scalar LowerYellow { get; set; } = new(10, 90, 0);
    public Scalar UpperYellow { get; set; } = new(70, 255, 255);
    public Scalar LowerRed1 { get; set; } = new(0, 50, 50);
    public Scalar UpperRed1 { get; set; } = new(10, 255, 255);
    public Scalar LowerRed2 { get; set; } = new(163, 50, 50);
    public Scalar UpperRed2 { get; set; } = new(180, 255, 255);
    public Scalar LowerWhite { get; set; } = new(100, 0, 190);
    public Scalar UpperWhite { get; set; } = new(180, 80, 255);
    public bool Enabled { get; set; } = true;
}