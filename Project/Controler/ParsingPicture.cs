using System;
using System.Diagnostics;
using OpenCvSharp;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using OpenCvSharp.CPlusPlus;

namespace Droid.Image
{
    public static class ParsingPicture
    {
        #region Enum
        public enum FormStyle
        {
            RECTANGLE,
            ELIPSE
        }
        #endregion  

        #region Struct
        public struct DetectZone
        {
            public FormStyle Style;
            public Color Color;
            public OpenCvSharp.CPlusPlus.Point Point;
            public OpenCvSharp.CPlusPlus.Size Axes;
            public double Angle;
            public int StartAngle;
            public int EndAngle;
            public int Width;
            public int Height;
            public int Thickness;
            public OpenCvSharp.CPlusPlus.Mat Src;
        }
        #endregion

        #region Attribute
        private const string ENVPATH = "../../";
        #endregion

        #region Methods public
        public static List<DetectZone> ProcessAll(byte[] imgArray)
        {
            List<DetectZone> rectangles = new List<DetectZone>();

            //rectangles.AddRange(DetectPedestrian(imgArray));
            rectangles.AddRange(DetectGeneric(imgArray, "eyes", Color.Yellow));
            rectangles.AddRange(DetectGeneric(imgArray, "hand", Color.Green));
            rectangles.AddRange(DetectGeneric(imgArray, "front_face", Color.FromArgb(200, 140, 140, 140)));
            //rectangles.AddRange(DetectGeneric(imgArray, "mouth", Color.Red));

            return rectangles;
        }

        public static List<DetectZone> DetectPedestrian(byte[] imgArray)
        {
            List<DetectZone> zones = new List<DetectZone>();
            try
            {
                var hog = new OpenCvSharp.CPlusPlus.HOGDescriptor();
                hog.SetSVMDetector(OpenCvSharp.CPlusPlus.HOGDescriptor.GetDefaultPeopleDetector());

                Console.WriteLine("CheckDetectorSize: {0}", hog.CheckDetectorSize());

                var watch = Stopwatch.StartNew();

                OpenCvSharp.CPlusPlus.Rect[] found = hog.DetectMultiScale(OpenCvSharp.CPlusPlus.Mat.FromImageData(imgArray, OpenCvSharp.LoadMode.Unchanged), 0, new OpenCvSharp.CPlusPlus.Size(1, 1), null, 1.05, 2);

                watch.Stop();
                Console.WriteLine("Detection time = {0}ms, {1} region(s) found", watch.ElapsedMilliseconds, found.Length);

                foreach (OpenCvSharp.CPlusPlus.Rect rect in found)
                {
                    //Rectangle rec = new Rectangle(
                    //    rect.X + (int)Math.Round(rect.Width * 0.1),
                    //    rect.Y + (int)Math.Round(rect.Height * 0.1),
                    //    (int)Math.Round(rect.Width * 0.8),
                    //    (int)Math.Round(rect.Height * 0.8)
                    //);
                    zones.Add(new DetectZone() {
                        Style = FormStyle.RECTANGLE,
                        Color = Color.Red,
                        Point = new OpenCvSharp.CPlusPlus.Point(rect.X + (int)Math.Round(rect.Width * 0.1), rect.Y + (int)Math.Round(rect.Height * 0.1)),
                        Width = (int)Math.Round(rect.Width * 0.8),
                        Height = (int)Math.Round(rect.Height * 0.8)
                    });
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Crahs in pedestrian research : " + exp.Message);
            }
            return zones;
        }
        public static List<DetectZone> DetectGeneric(byte[] imgArray, string objectName, Color col)
        {
            List<DetectZone> zones = new List<DetectZone>();
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream objStream = assembly.GetManifestResourceStream(string.Format("Droid.Image.Resources.XML_Recognition.{0}.xml", objectName));
            StreamReader objReader = new StreamReader(objStream);
            string recognigionFileData = objReader.ReadToEnd();

            string workingFolder = Interface_image.WORKINGDIRECTORY;
            if (!Directory.Exists(workingFolder)) { Directory.CreateDirectory(workingFolder); }
            if (!Directory.Exists(Path.Combine(workingFolder, "XML_Recognition"))) { Directory.CreateDirectory(Path.Combine(workingFolder, "XML_Recognition")); }
            
            string path = string.Format("{0}\\XML_Recognition\\{1}.xml", workingFolder, objectName);
            if (!File.Exists(path)) { using (StreamWriter writer = new StreamWriter(path)) { writer.Write(recognigionFileData); } }

            zones.AddRange(Process(col, imgArray, new OpenCvSharp.CPlusPlus.CascadeClassifier(path)));

            return zones;
        }
        #endregion

        #region Methods private
        private static List<DetectZone> Process(Color col, byte[] imgArray, OpenCvSharp.CPlusPlus.CascadeClassifier cascade)
        {
            List<DetectZone> zones = new List<DetectZone>();

            using (var src = OpenCvSharp.CPlusPlus.Mat.FromImageData(imgArray))
            using (var gray = new OpenCvSharp.CPlusPlus.Mat())
            {
                OpenCvSharp.CPlusPlus.Cv2.CvtColor(src, gray, OpenCvSharp.ColorConversion.BgrToGray);

                // Detect faces
                OpenCvSharp.CPlusPlus.Rect[] areas = cascade.DetectMultiScale(gray, 1.08, 2, HaarDetectionType.ScaleImage, new OpenCvSharp.CPlusPlus.Size(30, 30));
                
                // Render all detected faces
                foreach (OpenCvSharp.CPlusPlus.Rect area in areas)
                {
                    //var center = new OpenCvSharp.CPlusPlus.Point
                    //{
                    //    X = (int)(face.X + face.Width * 0.5),
                    //    Y = (int)(face.Y + face.Height * 0.5)
                    //};
                    //var axes = new OpenCvSharp.CPlusPlus.Size
                    //{
                    //    Width = (int)(face.Width * 0.5),
                    //    Height = (int)(face.Height * 0.5)
                    //};

                    //Rectangle rect = new Rectangle()
                    //{
                    //    X = (int)(face.X),
                    //    Y = (int)(face.Y),
                    //    Width = (int)(face.Width),
                    //    Height = (int)(face.Height)
                    //};

                    zones.Add(new DetectZone()
                    {
                        Style = FormStyle.ELIPSE,
                        Color = col,
                        Point = new OpenCvSharp.CPlusPlus.Point(area.X, area.Y - (int)Math.Round(area.Height * 0.2)),
                        Width = (int)Math.Round(area.Width * 1.0),
                        Height = (int)Math.Round(area.Height * 1.2)
                    });

                    //zones.Add(new DetectZone()
                    //{
                    //    Style = FormStyle.ELIPSE,
                    //    Color = col,
                    //    Point = center,
                    //    Axes = axes,
                    //    Angle = 0,
                    //    StartAngle = 0,
                    //    EndAngle = 360,
                    //    Thickness = 4,
                    //    Src = src
                    //});

                    //zones.Add(new DetectZone() { Zone = rec, Color = col });
                    //Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255),4);
                }
            }
            return zones;
        }

        //private static List<DetectZone> Process2(Color col, byte[] imgArray, OpenCvSharp.CPlusPlus.CascadeClassifier cascade)
        //{
        //    List<DetectZone> zones = new List<DetectZone>();

        //    Image sourceImage = null;
        //    Bitmap bmp = new Bitmap(sourceImage);

        //    // Lock the bitmap's bits.  
        //    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        //    System.Drawing.Imaging.BitmapData bmpData =
        //        bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
        //        bmp.PixelFormat);

        //    // Get the address of the first line.
        //    IntPtr ptr = bmpData.Scan0;

        //    // Declare an array to hold the bytes of the bitmap.
        //    int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
        //    byte[] rgbBuffer = new byte[bytes];

        //    // Copy the RGB values into the array.
        //    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbBuffer, 0, bytes);

        //    // Do your OpenCV processing...
        //    // ...

        //    // Unlock the bits.
        //    bmp.UnlockBits(bmpData);

        //    Mat newImg = new OpenCvSharp.CPlusPlus.Mat(sourceImage.Height, sourceImage.Width, , CV_8UC3, ptrImageData);


        //    return zones;   
        //}
        #endregion
    }
}
