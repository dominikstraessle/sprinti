namespace Sprinti.Stream;

public class StreamOptions : ISprintiOptions
{
    public const string Stream = "Stream";
    public string Username { get; set; } = "pren";
    public string Password { get; set; } = "463997";
    public string Host { get; set; } = "147.88.48.131/axis-media/media.amp?streamprofile=pren_profile_small";
    public string RtspSource => $"rtsp://{Username}:{Password}@{Host}";
    public bool Enabled { get; set; } = false;
}

public class CaptureOptions : ISprintiOptions
{
    public const string Capture = "Capture";
    public string ImagePathFromContentRoot { get; set; } = "img";
    public int CaptureIntervalInSeconds { get; set; } = 5;
    public bool Enabled { get; set; } = true;
}