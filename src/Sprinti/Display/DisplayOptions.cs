namespace Sprinti.Display;

public class DisplayOptions : ISprintiOptions
{
    public const string Display = "Display";
    public int BusId { get; set; } = 1;
    public int Address { get; set; } = 0x3C;
    public int Width { get; set; } = 128;
    public int Height { get; set; } = 32;
    public bool Enabled { get; set; } = true;
    public int FontSize { get; set; } = 15;
    public string Font { get; set; } = "DejaVu Sans";
}