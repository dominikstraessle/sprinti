using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageSelectorTests(IImageSelector imageSelector, IOptions<DetectionOptions> options)
{
    [Fact]
    public void DebugConfig()
    {
        foreach (var config in options.Value.LookupConfigs)
        {
            var imagePath = TestFiles.GetDetectionFileName(config.Filename);
            using var image = Cv2.ImRead(imagePath);
            Cv2.Circle(image, new Point(config.SelectorPoints.P1[0], config.SelectorPoints.P1[1]), 1, new Scalar(0), 5);
            Cv2.Circle(image, new Point(config.SelectorPoints.P2[0], config.SelectorPoints.P2[1]), 1, new Scalar(0), 5);


            for (var i = 0; i < config.Points.Length; i++)
            {
                var p = config.Points[i];
                var center = new Point(p[0], p[1]);
                Cv2.Circle(image, center, 1, new Scalar(0), 5);
                Cv2.PutText(image, config.Lookup.ElementAt(i).ToString(), center, HersheyFonts.HersheyTriplex, 1,
                    new Scalar(0));
            }

            Assert.True(image.SaveImage(TestFiles.GetDebugPath(config.Filename)));
        }
    }

    [Fact]
    public void TrySelectAllTest()
    {
        foreach (var config in options.Value.LookupConfigs)
        {
            var imagePath = TestFiles.GetDetectionFileName(config.Filename);
            using var image = Cv2.ImRead(imagePath);

            var result = imageSelector.TrySelectImage(image, out var actual);
            var message = $"Invalid lookup selected for {config.Filename}: {config.Lookup}";
            Assert.True(result, message);
            Assert.NotNull(actual);
            Assert.Equal(config.Lookup, actual.Lookup);
        }
    }

    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("1.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[0].Lookup, config.Lookup);
    }

    [Fact]
    public void TrySelectImage2Test()
    {
        var imagePath = TestFiles.GetTestFileFullName("2.png");
        using var image = Cv2.ImRead(imagePath);

        var result = imageSelector.TrySelectImage(image, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        Assert.Equivalent(DetectionOptions.DefaultLookupConfigs[1].Lookup, config.Lookup);
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