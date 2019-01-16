using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droid.Image
{
    /// <summary>
    /// Helper class for setting up and applying a color matrix
    /// http://www.gutgames.com/post/Adjusting-Brightness-of-an-Image-in-C.aspx
    /// </summary>
    public class ColorMatrix
    {
        #region Properties
        /// <summary>
        /// Matrix containing the values of the ColorMatrix
        /// </summary>
        public float[][] Matrix { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorMatrix()
        {
        }
        #endregion

        #region Methods Public
        /// <summary>
        /// Applies the color matrix
        /// </summary>
        /// <param name="OriginalImage">Image sent in</param>
        /// <returns>An image with the color matrix applied</returns>
        public Bitmap Apply(Bitmap OriginalImage)
        {
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix();
                using (ImageAttributes Attributes = new ImageAttributes())
                {
                    Attributes.SetColorMatrix(NewColorMatrix);
                    NewGraphics.DrawImage(OriginalImage,
                        new Rectangle(0, 0, OriginalImage.Width, OriginalImage.Height),
                        0, 0, OriginalImage.Width, OriginalImage.Height,
                        GraphicsUnit.Pixel,
                        Attributes);
                }
            }
            return NewBitmap;
        }
        #endregion
    }
}
