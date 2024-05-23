using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Detection;

public static class ImageMask
{
    // https://pseudopencv.site/utilities/hsvcolormask/
    private static readonly Scalar LowerBlue = new(50, 70, 0);
    private static readonly Scalar UpperBlue = new(160, 255, 255);
    private static readonly Scalar LowerYellow = new(10, 90, 0);
    private static readonly Scalar UpperYellow = new(70, 255, 255);
    private static readonly Scalar LowerRed1 = new(0, 50, 50);
    private static readonly Scalar UpperRed1 = new(10, 255, 255);
    private static readonly Scalar LowerRed2 = new(163, 50, 50);
    private static readonly Scalar UpperRed2 = new(180, 255, 255);
    private static readonly Scalar LowerWhite = new(100, 0, 190);
    private static readonly Scalar UpperWhite = new(180, 80, 255);

    public static Mat BlueMask(Mat image)
    {
        return GetMask(image, LowerBlue, UpperBlue);
    }

    public static Mat YellowMask(Mat image)
    {
        return GetMask(image, LowerYellow, UpperYellow);
    }

    public static Mat WhiteMask(Mat image)
    {
        var mask = GetMask(image, LowerWhite, UpperWhite);
        using var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
        Cv2.MorphologyEx(mask, mask, MorphTypes.Open, kernel);
        return mask;
    }

    public static Mat RedMask(Mat image)
    {
        var mask = new Mat();
        using var redMask1 = GetMask(image, LowerRed1, UpperRed1);
        using var redMask2 = GetMask(image, LowerRed2, UpperRed2);
        Cv2.BitwiseOr(redMask1, redMask2, mask);
        return mask;
    }

    public static Mat NoneMask(Mat image)
    {
        var mask = new Mat();
        using var redMask = RedMask(image);
        using var yellowMask = YellowMask(image);
        using var blueMask = BlueMask(image);
        Cv2.BitwiseOr(redMask, yellowMask, mask);
        Cv2.BitwiseOr(blueMask, mask, mask);
        Cv2.BitwiseNot(mask, mask);
        return mask;
    }

    public static Mat GetMask(Color color, Mat image)
    {
        return color switch
        {
            Color.None => NoneMask(image),
            Color.Red => RedMask(image),
            Color.Blue => BlueMask(image),
            Color.Yellow => YellowMask(image),
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    private static Mat GetMask(Mat imageHsv, Scalar lower, Scalar upper)
    {
        // Create mask
        var mask = new Mat();
        Cv2.InRange(imageHsv, lower, upper, mask);
        return mask;
    }
}