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
    public bool Enabled { get; set; } = true;
}

public class CaptureOptions : ISprintiOptions
{
    public const string Capture = "Capture";
    public string ImagePathFromContentRoot { get; set; } = "img";
    public int CaptureIntervalInFrames { get; set; } = 24;
    public bool Enabled { get; set; } = true;
}