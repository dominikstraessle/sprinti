namespace Sprinti.Stream;

public class StreamOptions
{
    public const string Stream = "Stream";
    public bool Enabled { get; set; } = false;
    public string Username { get; set; } = "pren";
    public string Password { get; set; } = "463997";
    public string Host { get; set; } = "147.88.48.131/axis-media/media.amp?streamprofile=pren_profile_small";
    public Capture Capture { get; set; } = new();
    public string RtspSource => $"rtsp://{Username}:{Password}@{Host}";
}

public class Capture
{
    public bool Enabled { get; set; } = true;
    public string SaveImagePathFromProjectRoot { get; set; } = "img";
    public int CaptureIntervalInSeconds { get; set; } = 5;
}