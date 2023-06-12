using System;
using System.IO;
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
        int headX = App.headX;
        int headY = App.headY;
        int headWidth = App.headWidth;
        int headHeight = App.headHeight;

        //body
        int bodyX = App.bodyX;
        int bodyY = App.bodyY;
        int bodyWidth = App.bodyWidth;
        int bodyHeight = App.bodyHeight;

        //legs
        int legsX = App.legsX;
        int legsY = App.legsY;
        int legsWidth = App.legsWidth;
        int legsHeight = App.legsHeight;

        //BitmapImage head = new BitmapImage(new Uri("../../Images/Heads/classic.png", UriKind.Relative));
        //BitmapImage body = new BitmapImage(new Uri("../../Images/Bodies/classic.png", UriKind.Relative));
        //BitmapImage legs = new BitmapImage(new Uri("../../Images/Legs/classic.png", UriKind.Relative));

        BitmapImage head = new BitmapImage(new Uri(App.HeadsPath, UriKind.Relative));
        BitmapImage body = new BitmapImage(new Uri(App.BodiesPath, UriKind.Relative));
        BitmapImage legs = new BitmapImage(new Uri(App.LegsPath, UriKind.Relative));

        private void SetTemplates()
        {
            head = new BitmapImage(new Uri(App.HeadsPath, UriKind.Relative));
            body = new BitmapImage(new Uri(App.BodiesPath, UriKind.Relative));
            legs = new BitmapImage(new Uri(App.LegsPath, UriKind.Relative));
        }

        private bool checkSize()
        {
            if ((int)head.Width % (int)headWidth != 0 || (int)head.Height != (int)headHeight)
            { return false; }
            if ((int)body.Width % (int)bodyWidth != 0 || (int)body.Height != (int)bodyHeight)
            { return false; }
            if ((int)legs.Width % (int)legsWidth != 0 || (int)legs.Height != (int)legsHeight)
            { return false; }
            return true;
        }

        public Constructor()
        {
            InitializeComponent();

            Update();
        }


        private WriteableBitmap MergeParts()
        {

            WriteableBitmap amalgam = new WriteableBitmap(
                27, // width
                49, // height
                96, // dpiX
                96, // dpiY
                PixelFormats.Bgra32, // pixel format
                null); // bitmap pallete

            BitmapSource headSource = null;
            BitmapSource bodySource = null;
            BitmapSource legsSource = null;
            try
            {
                if (head.Format.ToString() != "Bgra32" || body.Format.ToString() != "Bgra32" || legs.Format.ToString() != "Bgra32")
                { throw new Exception("Wrong format"); }
                else if  (!checkSize())
                { throw new Exception("Wrong size"); }
                headSource = new CroppedBitmap(head, new Int32Rect(headX, headY, headWidth, headHeight));
                bodySource = new CroppedBitmap(body, new Int32Rect(bodyX, bodyY, bodyWidth, bodyHeight));
                legsSource = new CroppedBitmap(legs, new Int32Rect(legsX, legsY, legsWidth, legsHeight));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Будут загружены стандартные шаблоны\nПроверьте размер загруженного изображения\nПроверте формат загруженного изображения\nТребуемый формат Bgra32", "Ошибка загрузки шаблона", MessageBoxButton.OK, MessageBoxImage.Warning);
                App.ResetPaths();
                SetTemplates();
                headSource = new CroppedBitmap(head, new Int32Rect(headX, headY, headWidth, headHeight));
                bodySource = new CroppedBitmap(body, new Int32Rect(bodyX, bodyY, bodyWidth, bodyHeight));
                legsSource = new CroppedBitmap(legs, new Int32Rect(legsX, legsY, legsWidth, legsHeight));
            }

            byte[] headPixels = App.BitmapSourceToArray(headSource);
            byte[] bodyPixels = App.BitmapSourceToArray(bodySource);
            byte[] legsPixels = App.BitmapSourceToArray(legsSource);
            byte[] amalgamPixels = App.BitmapSourceToArray(amalgam);

            int amalgamPixelsIdx;
            int PixelsIdx;
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


            amalgam.WritePixels(new Int32Rect(0, 0, 27, 49), amalgamPixels, amalgam.PixelWidth * (amalgam.Format.BitsPerPixel / 8), 0);
            return amalgam;
        }


        private void Update()
        {
            WriteableBitmap amalgam = MergeParts();
            Char.Source = amalgam;
        }


        // Saving sprite

        private void ExportSprite(object sender, RoutedEventArgs e)
        {
            string fileName = SpriteName.Text + ".png";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, fileName);
            savingPath = App.GetNextFileName(savingPath);
            try
            {
                using (FileStream stream = new FileStream(savingPath, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Char.Source));
                    encoder.Save(stream);
                }
                MessageBox.Show("Спрайт персонажа экпортирован на рабочий стол.");
                return;
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
                return;
            }
        }

        private void SaveSprite(object sender, RoutedEventArgs e)
        {
            string savedPath = "../../SavedSprites/";
            string fileName = SpriteName.Text + ".png";
            string savingPath = Path.Combine(savedPath, fileName);
            savingPath = App.GetNextFileName(savingPath);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Char.Source));
            try
            {
                using (FileStream stream = new FileStream(savingPath, FileMode.Create))
                    encoder.Save(stream);
                MessageBox.Show("Спрайт персонажа сохранен.");
                return;
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
                return;
            }
        }


        // Next/prev buttons
        private void NextHead(object sender, RoutedEventArgs e)
        {
            headX += headWidth;
            headX = headX % (head.PixelWidth);
            Update();
        }

        private void NextBody(object sender, RoutedEventArgs e)
        {
            bodyX += bodyWidth;
            bodyX = bodyX % (body.PixelWidth);
            Update();
        }

        private void NextLegs(object sender, RoutedEventArgs e)
        {
            legsX += legsWidth;
            legsX = legsX % (legs.PixelWidth);
            Update();
        }

        private void PrevLegs(object sender, RoutedEventArgs e)
        {
            legsX -= legsWidth;
            legsX = (legsX + legs.PixelWidth) % (legs.PixelWidth);
            Update();
        }

        private void PrevBody(object sender, RoutedEventArgs e)
        {
            bodyX -= bodyWidth;
            bodyX = (bodyX + body.PixelWidth) % (body.PixelWidth);
            Update();
        }

        private void PrevHead(object sender, RoutedEventArgs e)
        {
            headX -= headWidth;
            headX = (headX + head.PixelWidth) % (head.PixelWidth);
            Update();
        }
    }
}
