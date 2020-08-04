using System.Drawing;

namespace CG4Pin.Core
{
    /// <summary>
    ///     Used to paint skeleton image.
    /// </summary>
    public class SkeletonImageDisplay : FeatureDisplay<SkeletonImage>
    {
        /// <summary>
        ///     Paints the specified <see cref="SkeletonImage"/> using the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="features">
        ///     The skeleton image to be painted.
        /// </param>
        /// <param name="g">
        ///     The <see cref="Graphics"/> object used to paint the orientation image.
        /// </param>
        public override void Show(SkeletonImage features, Graphics g)
        {
            Image img = features.ConvertToBitmap();
            g.DrawImage(img, 0, 0);
        }
    }
}
