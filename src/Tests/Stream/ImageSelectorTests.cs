using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageSelectorTests(IImageSelector imageSelector, IOptions<DetectionOptions> options)
{
    [Theory]
    [InlineData("20240419091756.png")]
    [InlineData("20240419091430.png")]
    [InlineData("20240419091435.png")]
    [InlineData("20240419091440.png")]
    [InlineData("20240419091445.png")]
    [InlineData("20240419091455.png")]
    [InlineData("20240419091500.png")]
    [InlineData("20240419091505.png")]
    [InlineData("20240419091510.png")]
    [InlineData("20240419091515.png")]
    public void TrySelectAllTest(string filename)
    {
        var imagePath = TestFiles.GetDetectionFileName(filename);
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Contains(config, options.Value.LookupConfigs);
    }

    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("1.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[0], config);
    }

    [Fact]
    public void TrySelectImage2Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("2.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[1], config);
    }

    [Fact]
    public void TrySelectImageFailedTest()
    {
        var imagePath = TestFiles.GetTestFileFullName("3.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.False(result);
        Assert.Null(config);
    }
}