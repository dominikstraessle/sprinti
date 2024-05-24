using OpenCvSharp;
using Sprinti.Detection;

namespace Sprinti.Tests.Detection;

public class ImageSelectorTests(IImageSelector imageSelector, DetectionOptions options)
{
    [Fact]
    public void DebugConfig()
    {
        foreach (var config in options.LookupConfigs)
        {
            var imagePath = DetectionTestFiles.GetDetectionFileName(config.Filename);
            using var image = Cv2.ImRead(imagePath);
            Assert.False(image.Empty(), config.Filename);
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

            Assert.True(image.SaveImage(DetectionTestFiles.GetDebugPath(config.Filename)), config.Filename);
        }
    }

    public static IEnumerable<object[]> AllSelectorTestFilesData()
    {
        return DetectionTestFiles.GetDetectionFiles().Select(detectionFile => (object[]) [detectionFile]);
    }

    [Theory]
    [MemberData(nameof(AllSelectorTestFilesData))]
    public void TrySelectAllSelectorFiles(string filename)
    {
        using var imageHsv = Cv2.ImRead(filename);
        Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

        var result = imageSelector.TrySelectImage(imageHsv, out var actual);
        var message = $"No lookup selected for {filename}";
        Assert.True(result, message);
        Assert.NotNull(actual);
    }

    [Fact]
    public void TrySelectAllTest()
    {
        foreach (var config in options.LookupConfigs)
        {
            var imagePath = DetectionTestFiles.GetDetectionFileName(config.Filename);
            using var imageHsv = Cv2.ImRead(imagePath);
            Assert.False(imageHsv.Empty(), $"Image is empty: {imagePath}");
            Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

            var result = imageSelector.TrySelectImage(imageHsv, out var actual);
            var message = $"Invalid lookup selected for {config.Filename}: {config.Lookup}";
            Assert.True(result, message);
            Assert.NotNull(actual);
            Assert.Equal(config.Lookup, actual.Lookup);
        }
    }

    [Fact]
    public void TrySelectImage1Test()
    {
        var imagePath = DetectionTestFiles.GetDetectionFileName("20240523083949.png");
        using var imageHsv = Cv2.ImRead(imagePath);
        Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

        var result = imageSelector.TrySelectImage(imageHsv, out var config);
        Assert.True(result);
        Assert.NotNull(config);
        var lookup = options.LookupConfigs.First(c => c.Filename.Equals("20240523083949.png")).Lookup;
        Assert.Equivalent(lookup, config.Lookup);
    }

    [Fact]
    public void TrySelectImageFailedTest()
    {
        var imagePath = DetectionTestFiles.GetImagePath("invalid.png");
        using var imageHsv = Cv2.ImRead(imagePath);
        Cv2.CvtColor(imageHsv, imageHsv, ColorConversionCodes.BGR2HSV);

        var result = imageSelector.TrySelectImage(imageHsv, out var config);
        Assert.False(result);
        Assert.Null(config);
    }
}