namespace Sprinti.Api.Video;

public class StreamOptions
{
    public const string Stream = "Stream";
    public bool Enabled { get; set; }
    public string Username { get; set; } = "pren";
    public string Password { get; set; } = "463997";
    public string Host { get; set; } = "147.88.48.131/axis-media/media.amp?streamprofile=pren_profile_small";
}