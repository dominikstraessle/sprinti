namespace Sprinti.Display;

public class DisplayOptions
{
    public const string Display = "Display";
    public int BusId { get; set; } = 1;
    public int Address { get; set; } = 0x3C;
    public int Width { get; set; } = 128;
    public int Height { get; set; } = 64;
}