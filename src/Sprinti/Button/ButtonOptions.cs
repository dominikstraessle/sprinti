﻿namespace Sprinti.Button;

public class ButtonOptions
{
    public const string Button = "Button";
    public int Pin { get; set; } = 26;
    public int GpioChip { get; set; } = 4;
    public bool IsPullUp { get; set; } = true;
    public bool UseExternalResistor { get; set; } = false;
}