using OpenCvSharp;

namespace Sprinti.Stream;

public static class ImageMask
{
    public static readonly Scalar LowerBlue = new(100, 90, 0);
    public static readonly Scalar UpperBlue = new(120, 255, 255);
    private static readonly Scalar LowerYellow = new(22, 93, 0);
    private static readonly Scalar UpperYellow = new(50, 255, 255);
    private static readonly Scalar LowerRed1 = new(0, 50, 50);
    private static readonly Scalar UpperRed1 = new(10, 255, 255);
    private static readonly Scalar LowerRed2 = new(150, 50, 50);
    private static readonly Scalar UpperRed2 = new(180, 255, 255);
    private static readonly Scalar LowerWhite = new(0, 0, 210);
    private static readonly Scalar UpperWhite = new(255, 50, 255);


    public static Mat BlueMask(Mat image) => GetMask(image, LowerBlue, UpperBlue);
    public static Mat YellowMask(Mat image) => GetMask(image, LowerYellow, UpperYellow);
    public static Mat WhiteMask(Mat image) => GetMask(image, LowerWhite, UpperWhite);

    public static Mat RedMask(Mat image)
    {
        var mask = new Mat();
        Cv2.BitwiseOr(GetMask(image, LowerRed1, UpperRed1), GetMask(image, LowerRed2, UpperRed2), mask);
        return mask;
    }

    private static Mat GetMask(Mat imageBgr, Scalar lower, Scalar upper)
    {
        // Convert the image to HSV color space
        var imageHsv = new Mat();
        Cv2.CvtColor(imageBgr, imageHsv, ColorConversionCodes.BGR2HSV);

        // Create mask
        var mask = new Mat();
        Cv2.InRange(imageHsv, lower, upper, mask);

        return mask;
    }
}