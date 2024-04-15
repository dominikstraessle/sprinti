using OpenCvSharp;

namespace Sprinti.Stream;

public class ImageSelector
{
    public static Mat GetMask(Mat imageRgb, Scalar lower, Scalar upper, bool show = false)
    {
        // Convert the image to HSV color space
        var imageHsv = new Mat();
        Cv2.CvtColor(imageRgb, imageHsv, ColorConversionCodes.RGB2HSV);

        // Create mask
        var mask = new Mat();
        Cv2.InRange(imageHsv, lower, upper, mask);

        // Apply masks
        var imageMasked = new Mat();
        Cv2.BitwiseAnd(imageHsv, imageHsv, imageMasked, mask: mask);
        Cv2.CvtColor(imageMasked, imageMasked, ColorConversionCodes.HSV2RGB);

        if (show)
        {
            Cv2.ImShow("Masked Image", imageMasked);
            Cv2.WaitKey();
        }

        return mask;
    }
}