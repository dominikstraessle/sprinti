using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageSelectorTests(IImageSelector imageSelector)
{
    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("1.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equal(DetectionOptions.DefaultLookupConfigs[0], config);
    }

    [Fact]
    public void TrySelectImage2Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("2.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equal(DetectionOptions.DefaultLookupConfigs[1], config);
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