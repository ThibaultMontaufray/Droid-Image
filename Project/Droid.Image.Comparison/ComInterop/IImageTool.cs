using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Droid.Image.Comparison
{
    public interface IImageTool
    {
        float GetPercentageDifference(string image1Path, string image2Path, byte threshold);
    }
}
