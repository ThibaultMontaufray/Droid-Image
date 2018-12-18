﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droid.Image.Controler
{
    public static class PerlinNoise
    {
        #region Functions

        /// <summary>
        /// Generates perlin noise
        /// </summary>
        /// <param name="Width">Width of the resulting image</param>
        /// <param name="Height">Height of the resulting image</param>
        /// <param name="MaxRGBValue">MaxRGBValue</param>
        /// <param name="MinRGBValue">MinRGBValue</param>
        /// <param name="Frequency">Frequency</param>
        /// <param name="Amplitude">Amplitude</param>
        /// <param name="Persistance">Persistance</param>
        /// <param name="Octaves">Octaves</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image containing perlin noise</returns>
        public static Bitmap Generate(int Width, int Height, int MaxRGBValue, int MinRGBValue, float Frequency, float Amplitude, float Persistance, int Octaves, int Seed)
        {
            Bitmap ReturnValue = new Bitmap(Width, Height);
            //BitmapData ImageData = Image.LockImage(ReturnValue);
            //int ImagePixelSize = Image.GetPixelSize(ImageData);
            //float[,] Noise = GenerateNoise(Seed, Width, Height);
            //for (int x = 0; x < Width; ++x)
            //{
            //    for (int y = 0; y < Height; ++y)
            //    {
            //        float Value = GetValue(x, y, Width, Height, Frequency, Amplitude, Persistance, Octaves, Noise);
            //        Value = (Value * 0.5f) + 0.5f;
            //        Value *= 255;
            //        int RGBValue = Math.MathHelper.Clamp((int)Value, MaxRGBValue, MinRGBValue);
            //        Image.SetPixel(ImageData, x, y, Color.FromArgb(RGBValue, RGBValue, RGBValue), ImagePixelSize);
            //    }
            //}
            //Image.UnlockImage(ReturnValue, ImageData);
            return ReturnValue;
        }

        private static float GetValue(int X, int Y, int Width, int Height, float Frequency, float Amplitude, float Persistance, int Octaves, float[,] Noise)
        {
            float FinalValue = 0.0f;
            for (int i = 0; i < Octaves; ++i)
            {
                FinalValue += GetSmoothNoise(X * Frequency, Y * Frequency, Width, Height, Noise) * Amplitude;
                Frequency *= 2.0f;
                Amplitude *= Persistance;
            }
            if (FinalValue < -1.0f)
            {
                FinalValue = -1.0f;
            }
            else if (FinalValue > 1.0f)
            {
                FinalValue = 1.0f;
            }
            return FinalValue;
        }

        private static float GetSmoothNoise(float X, float Y, int Width, int Height, float[,] Noise)
        {
            float FractionX = X - (int)X;
            float FractionY = Y - (int)Y;
            int X1 = ((int)X + Width) % Width;
            int Y1 = ((int)Y + Height) % Height;
            int X2 = ((int)X + Width - 1) % Width;
            int Y2 = ((int)Y + Height - 1) % Height;

            float FinalValue = 0.0f;
            FinalValue += FractionX * FractionY * Noise[X1, Y1];
            FinalValue += FractionX * (1 - FractionY) * Noise[X1, Y2];
            FinalValue += (1 - FractionX) * FractionY * Noise[X2, Y1];
            FinalValue += (1 - FractionX) * (1 - FractionY) * Noise[X2, Y2];

            return FinalValue;
        }

        private static float[,] GenerateNoise(int Seed, int Width, int Height)
        {
            float[,] Noise = new float[Width, Height];
            System.Random RandomGenerator = new System.Random(Seed);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Noise[x, y] = ((float)(RandomGenerator.NextDouble()) - 0.5f) * 2.0f;
                }
            }
            return Noise;
        }

        #endregion
    }
}
