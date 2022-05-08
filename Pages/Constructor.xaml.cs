using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Constructor.xaml
    /// </summary>
    public partial class Constructor : Page
    {
        //head
        int headX = 1;
        int headY = 1;
        int headWidth = 25;
        int headHeight = 22;

        //body
        int bodyX = 1;
        int bodyY = 1;
        int bodyWidth = 25;
        int bodyHeight = 22;

        //legs
        int legsX = 1;
        int legsY = 1;
        int legsWidth = 15;
        int legsHeight = 17;

        BitmapImage head = new BitmapImage(new Uri("../../Images/redacted/head.png", UriKind.Relative));
        BitmapImage body = new BitmapImage(new Uri("../../Images/redacted/body.png", UriKind.Relative));
        BitmapImage legs = new BitmapImage(new Uri("../../Images/redacted/legs.png", UriKind.Relative));

        



        public Constructor()
        {
            InitializeComponent();
            update();

        }

        private WriteableBitmap mergeParts()
        {
            WriteableBitmap amalgam = new WriteableBitmap(
                27, // width
                49, // height
                96, // dpiX
                96, // dpiY
                PixelFormats.Bgra32, // pixel format
                null); // bitmap pallete

            BitmapSource headSource = new CroppedBitmap(head, new Int32Rect(headX, headY, headWidth, headHeight));
            BitmapSource bodySource = new CroppedBitmap(body, new Int32Rect(bodyX, bodyY, bodyWidth, bodyHeight));
            BitmapSource legsSource = new CroppedBitmap(legs, new Int32Rect(legsX, legsY, legsWidth, legsHeight));

            byte[] headPixels = BitmapSourceToArray(headSource);
            byte[] bodyPixels = BitmapSourceToArray(bodySource);
            byte[] legsPixels = BitmapSourceToArray(legsSource);


            byte[] amalgamPixels = BitmapSourceToArray(amalgam);


            int amalgamPixelsIdx = 0;
            int PixelsIdx = 0;

            for (int i = 0; i < legsSource.PixelHeight; i++)
            {
                for (int j = 0; j < legsSource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 31 * 4 + 6 * 4);
                    PixelsIdx = i * 4 * legsSource.PixelWidth + j * 4;
                    if (legsPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = legsPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = legsPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = legsPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = legsPixels[PixelsIdx + 3];
                }
            }

            for (int i = 0; i < headSource.PixelHeight; i++)
            {
                for (int j = 0; j < headSource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 1 * 4 + 1 * 4);
                    PixelsIdx = i * 4 * headSource.PixelWidth + j * 4;
                    if (headPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = headPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = headPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = headPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = headPixels[PixelsIdx + 3];
                }
            }

            for (int i = 0; i < bodySource.PixelHeight; i++)
            {
                for (int j = 0; j < bodySource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 18 * 4 + 1 * 4);
                    PixelsIdx = i * 4 * bodySource.PixelWidth + j * 4;
                    if (bodyPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = bodyPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = bodyPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = bodyPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = bodyPixels[PixelsIdx + 3];
                }
            }

            //amalgam.WritePixels(new Int32Rect(6, 31, 15, 17), legsPixels, legsSource.PixelWidth * (legsSource.Format.BitsPerPixel / 8), 0);
            //amalgam.WritePixels(new Int32Rect(1, 1, 25, 22), headPixels, headSource.PixelWidth * (headSource.Format.BitsPerPixel / 8), 0);
            //amalgam.WritePixels(new Int32Rect(1, 18, 25, 22), bodyPixels, bodySource.PixelWidth * (bodySource.Format.BitsPerPixel / 8), 0);

            amalgam.WritePixels(new Int32Rect(0, 0, 27, 49), amalgamPixels, amalgam.PixelWidth * (amalgam.Format.BitsPerPixel / 8), 0);

            return amalgam;
        }

        private byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            // Stride = (width) x (bytes per pixel)
            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }
        

        private void update()
        {
            WriteableBitmap amalgam = mergeParts();
            Char.Source = amalgam;
        }


        private void NextHead(object sender, RoutedEventArgs e)
        {
            headX += headWidth + 1;
            headX = headX % (head.PixelWidth - 1);
            update();
        }

        private void NextBody(object sender, RoutedEventArgs e)
        {
            bodyX += bodyWidth + 1;
            bodyX = bodyX % (body.PixelWidth - 1);
            update();
        }

        private void NextLegs(object sender, RoutedEventArgs e)
        {
            legsX += legsWidth + 1;
            legsX = legsX % (legs.PixelWidth - 1);
            update();
        }

        private void PrevLegs(object sender, RoutedEventArgs e)
        {
            legsX -= legsWidth + 1;
            legsX = (legsX + legs.PixelWidth - 1) % (legs.PixelWidth - 1);
            update();
        }

        private void PrevBody(object sender, RoutedEventArgs e)
        {
            bodyX -= bodyWidth + 1;
            bodyX = (bodyX + body.PixelWidth - 1) % (body.PixelWidth - 1);
            update();
        }

        private void PrevHead(object sender, RoutedEventArgs e)
        {
            headX -= headWidth + 1;
            headX = (headX + head.PixelWidth - 1) % (head.PixelWidth - 1);
            update();
        }
    }
}
