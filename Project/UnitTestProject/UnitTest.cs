using Droid_Image;
using NUnit.Framework;
using System.Drawing;

namespace UnitTestProject
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void TestUTRuns()
        {
            Assert.IsTrue(true);
        }
        [Test]
        public void Test_image_take_picture()
        {
            Assert.IsNotNull(Interface_image.ACTION_130_take_picture("himalaya"));
        }
        [Test]
        public void Test_image_crop()
        {
            Image img = new Bitmap(100, 100);
            try
            {
                Interface_image.ACTION_131_crop_picture(img, 2, 2, 0, 0);
                Assert.IsTrue(true);
            }
            catch (System.Exception exp)
            {
                Assert.Fail(exp.Message);
            }
        }
        [Test]
        public void Test_image_flip_horizontal()
        {
            Image img = new Bitmap(100, 100);
            try
            {
                Interface_image.ACTION_134_flip_horizontal(img);
                Assert.IsTrue(true);
            }
            catch (System.Exception exp)
            {
                Assert.Fail(exp.Message);
            }
        }
        [Test]
        public void Test_image_flip_vertical()
        {
            Image img = new Bitmap(100, 100);
            try
            {
                Interface_image.ACTION_133_flip_vertical(img);
                Assert.IsTrue(true);
            }
            catch (System.Exception exp)
            {
                Assert.Fail(exp.Message);
            }
        }
        [Test]
        public void Test_image_resize()
        {
            Image img = new Bitmap(100, 100);
            try
            {
                Interface_image.ACTION_132_resize_picture(img, 10, 10);
                Assert.IsTrue(true);
            }
            catch (System.Exception exp)
            {
                Assert.Fail(exp.Message);
            }
        }
    }
}
