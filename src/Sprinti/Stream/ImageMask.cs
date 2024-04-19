using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Stream;

public static class ImageMask
{
    private static readonly Scalar LowerBlue = new(100, 90, 0);
    private static readonly Scalar UpperBlue = new(120, 255, 255);
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

    public static Mat GetMask(Color color, Mat image) => color switch
    {
        Color.None => NoneMask(image),
        Color.Red => RedMask(image),
        Color.Blue => BlueMask(image),
        Color.Yellow => YellowMask(image),
        _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
    };

    private static Mat GetMask(Mat imageHsv, Scalar lower, Scalar upper)
    {
        // Create mask
        var mask = new Mat();
        Cv2.InRange(imageHsv, lower, upper, mask);

        return mask;
    }
}