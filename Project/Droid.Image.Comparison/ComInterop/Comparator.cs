using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Droid.Image.Comparison
{
    public class Comparator : IImageTool
    {
        public float GetPercentageDifference(string image1Path, string image2Path, byte threshold)
        {
            return ImageTool.GetPercentageDifference(image1Path, image2Path, threshold);
        }
        public float GetPercentageDifference(Bitmap image1Path, Bitmap image2Path)
        {
            return ImageTool.GetPercentageDifference(image1Path, image2Path);
        }
    }
}
