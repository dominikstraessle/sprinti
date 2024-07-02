using Microsoft.Extensions.Options;
using OpenCvSharp;
using Sprinti.Domain;

namespace Sprinti.Detection;

public class ImageMask(IOptions<MaskOptions> options)
{
    public Mat BlueMask(Mat image)
    {
        return GetMask(image, options.Value.LowerBlue, options.Value.UpperBlue);
    }

    public Mat YellowMask(Mat image)
    {
        return GetMask(image, options.Value.LowerYellow, options.Value.UpperYellow);
    }

    public Mat WhiteMask(Mat image)
    {
        var mask = GetMask(image, options.Value.LowerWhite, options.Value.UpperWhite);
        using var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
        Cv2.MorphologyEx(mask, mask, MorphTypes.Open, kernel);
        return mask;
    }

    public Mat RedMask(Mat image)
    {
        var mask = new Mat();
        using var redMask1 = GetMask(image, options.Value.LowerRed1, options.Value.UpperRed1);
        using var redMask2 = GetMask(image, options.Value.LowerRed2, options.Value.UpperRed2);
        Cv2.BitwiseOr(redMask1, redMask2, mask);
        return mask;
    }

    public Mat NoneMask(Mat image)
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

    public Mat GetMask(Color color, Mat image)
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