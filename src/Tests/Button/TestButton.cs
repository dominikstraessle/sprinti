// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Button;

namespace Sprinti.Tests.Button;

public class TestButton : ButtonBase
{
    public TestButton()
    {
    }

    public TestButton(TimeSpan debounceTime, TimeSpan holdingTime)
        : base(TimeSpan.FromSeconds(5), holdingTime, debounceTime)
    {
    }

    public void PressButton()
    {
        HandleButtonPressed();
    }

    public void ReleaseButton()
    {
        HandleButtonReleased();
    }
}