using OpenCvSharp;
using Sprinti.Stream;

namespace Sprinti.Tests.Stream;

public class ImageMaskTests
{
    [Fact]
    public void TestMasks()
    {
        // Provide the relative path to your image file
        var imagePath = TestFiles.GetTestFileFullName("1.png"); // Replace "YourImage.jpg" with the actual file name
        using var image = Cv2.ImRead(imagePath);

        using var actualBlue = ImageMask.BlueMask(image);
        using var expectedBlue = Cv2.ImRead(TestFiles.GetTestFileFullName("1-blue-mask.png"), ImreadModes.Grayscale);
        Assert.True(EqualImages(expectedBlue, actualBlue));

        using var actualRed = ImageMask.RedMask(image);
        using var expectedRed = Cv2.ImRead(TestFiles.GetTestFileFullName("1-red-mask.png"), ImreadModes.Grayscale);
        Assert.True(EqualImages(expectedRed, actualRed));

        using var actualYellow = ImageMask.YellowMask(image);
        using var expectedYellow = Cv2.ImRead(TestFiles.GetTestFileFullName("1-yellow-mask.png"), ImreadModes.Grayscale);
        Assert.True(EqualImages(expectedYellow, actualYellow));

        using var actualWhite = ImageMask.WhiteMask(image);
        using var expectedWhite = Cv2.ImRead(TestFiles.GetTestFileFullName("1-white-mask.png"), ImreadModes.Grayscale);
        Assert.True(EqualImages(expectedWhite, actualWhite));

        using var actualNone = ImageMask.NoneMask(image);
        using var expectedNone = Cv2.ImRead(TestFiles.GetTestFileFullName("1-none-mask.png"), ImreadModes.Grayscale);
        Assert.True(EqualImages(expectedNone, actualNone));
    }

    private static bool EqualImages(Mat image1, Mat image2)
    {
        // Check if the images have the same size
        if (image1.Size() != image2.Size())
        {
            // Images have different sizes, so they cannot be equal
            return false;
        }

        // Compare pixel values
        for (var y = 0; y < image1.Rows; y++)
        {
            for (var x = 0; x < image1.Cols; x++)
            {
                // Get pixel values of each image at the same position
                Scalar pixel1 = image1.Get<byte>(y, x);
                Scalar pixel2 = image2.Get<byte>(y, x);

                // Compare pixel values
                if (pixel1 != pixel2)
                {
                    // Pixels are different, images are not equal
                    return false;
                }
            }
        }

        // All pixels are equal, images are equal
        return true;
    }
}