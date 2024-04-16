using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageSelectorTests
{
    private readonly ImageSelector _imageSelector;

    public ImageSelectorTests()
    {
        var options = new OptionsWrapper<ImageOptions>(new ImageOptions());

        _imageSelector = new ImageSelector(options, NullLogger<ImageSelector>.Instance);
    }

    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("1.png"); // Replace "YourImage.jpg" with the actual file name
        var image = Cv2.ImRead(imagePath);

        var result = _imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equal(ImageOptions.DefaultLookupConfigs[0], config);
    }

    [Fact]
    public void TrySelectImage2Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("2.png"); // Replace "YourImage.jpg" with the actual file name
        var image = Cv2.ImRead(imagePath);

        var result = _imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equal(ImageOptions.DefaultLookupConfigs[1], config);
    }

    [Fact]
    public void TrySelectImageFailedTest()
    {
        var imagePath = TestFiles.GetTestFileFullName("3.png"); // Replace "YourImage.jpg" with the actual file name
        var image = Cv2.ImRead(imagePath);

        var result = _imageSelector.TrySelectImage(image, out var config);
        Assert.False(result);
        Assert.Null(config);
    }
}