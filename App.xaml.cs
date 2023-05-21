using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public static int headX = 1;
        public static int headY = 1;
        public static int headWidth = 25;
        public static int headHeight = 22;

        //body
        public static int bodyX = 1;
        public static int bodyY = 1;
        public static int bodyWidth = 25;
        public static int bodyHeight = 22;

        //legs
        public static int legsX = 1;
        public static int legsY = 1;
        public static int legsWidth = 15;
        public static int legsHeight = 17;


    }
}
