using System.Drawing;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ImageMagick;
using System.Drawing.Imaging;

namespace SpriteGenerator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static BitmapImage DefaultHeadTemplate { get; set; }
        public static BitmapImage DefaultBodyTemplate { get; set; }
        public static BitmapImage DefaultLegsTemplate { get; set; }



        public static string HeadsPath = "../../Images/Heads/classic.png"; // замените на нужный путь
        public static string BodiesPath = "../../Images/Bodies/classic.png"; // замените на нужный путь
        public static string LegsPath = "../../Images/Legs/classic.png"; // замените на нужный путь

        public static void ResetPaths()
        {
            HeadsPath = "../../Images/Heads/classic.png"; // замените на нужный путь
            BodiesPath = "../../Images/Bodies/classic.png"; // замените на нужный путь
            LegsPath = "../../Images/Legs/classic.png"; // замените на нужный путь
        }

        public static IMagickImage ToMagickImage(Bitmap bmp)
        {
            IMagickImage img = null;
            MagickFactory f = new MagickFactory();
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                img = new MagickImage(f.Image.Create(ms));
            }
            return img;
        }

        public static byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            /*
             * Здесь мы вычисляем ширину одной строки пикселей в байтах. 
             * PixelWidth представляет ширину изображения в пикселях, а BitsPerPixel - количество битов на пиксель в формате изображения. 
             * Для вычисления ширины строки умножаем ширину изображения на количество байтов на пиксель и делим на 8.
             */

            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];
            bitmapSource.CopyPixels(pixels, stride, 0);
            return pixels;
        }
        public static string GetNextFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string pathName = Path.GetDirectoryName(fileName);
            string fileNameOnly = Path.Combine(pathName, Path.GetFileNameWithoutExtension(fileName));
            int i = 0;
            // If the file exists, keep trying until it doesn't
            while (File.Exists(fileName))
            {
                i += 1;
                fileName = string.Format("{0}({1}){2}", fileNameOnly, i, extension);
            }
            return fileName;
        }

        //head
        public static int headX = 0;
        public static int headY = 0;
        public static int headWidth = 25;
        public static int headHeight = 22;

        //body
        public static int bodyX = 0;
        public static int bodyY = 0;
        public static int bodyWidth = 25;
        public static int bodyHeight = 22;

        //legs
        public static int legsX = 0;
        public static int legsY = 0;
        public static int legsWidth = 15;
        public static int legsHeight = 17;

        //final size
        public static int FinalWidth = 25;
        public static int FinalHeight = 47;
    }
}
