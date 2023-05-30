using ImageMagick;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpriteGenerator.Pages
{
    public partial class AnimationPage : Page
    {
        private string loadedSprite = string.Empty;
        public AnimationPage()
        {
            InitializeComponent();
        }

        private void ChangeImageControlSource(System.Windows.Controls.Image imageControl, string imagePath)
        {
            //string[] filesEntries = Directory.GetFiles(imagePath);
            //string classicPath = Array.Find(filesEntries, file => Path.GetFileName(file) == "classic.png");
            string fullPath = Path.GetFullPath(imagePath);
            Uri uri = new Uri($"file:{fullPath}");
            BitmapImage bitmap = new BitmapImage(uri);
            imageControl.Source = bitmap;

        }

        private string LoadStringFromDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else { return null; }
        }
        private void UploadSprite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadedSprite = LoadStringFromDialog();
                ChangeImageControlSource(PreviewImageControl, loadedSprite);
                SpriteNameControl.Content = Path.GetFileNameWithoutExtension(loadedSprite);
            }
            catch (Exception ex)
            {
            }
        }
        private void SaveJumpAnimation()
        {
            //try
            //{
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_Jump.gif");
            savingPath = App.GetNextFileName(savingPath);

            // Создание коллекции кадров для анимации
            MagickImageCollection frames = new MagickImageCollection();

            // Число кадров в анимации
            int totalFrames = 30;

            // Задержка между кадрами (в миллисекундах)
            int animationDelay = 11; // Например, 100 мс

            // Загрузка исходного изображения
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                // Расчет пикселей смещения для движения вверх и вниз
                int offsetY = sourceImage.Height / 15;
                int newHeight = sourceImage.Height + offsetY * 15;
                sourceImage.Extent(sourceImage.Width, newHeight, Gravity.South, MagickColors.White);
                // Добавление первого кадра (исходное изображение)
                frames.Add(sourceImage.Clone());

                System.Drawing.Image image = System.Drawing.Image.FromFile(loadedSprite);

                Bitmap resultImage = new Bitmap(image.Width, newHeight);


                // Добавление кадров с плавным движением вверх и вниз
                for (int i = 1; i <= totalFrames; i++)
                {

                    // Вычисление позиции Y с учетом смещения
                    int currentOffsetY = 0;

                    if (i >= 1 && i <= 15)
                    {
                        // Движение вверх
                        currentOffsetY = offsetY * (15 - (i - 1));
                    }
                    else if (i >= 16 && i <= 30)
                    {
                        // Движение вниз
                        currentOffsetY = offsetY * (i - 15);
                    }

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, resultImage.Width, resultImage.Height));
                        graphics.DrawImage(image, 0, currentOffsetY);
                    }
                    // Добавление кадра в коллекцию
                    frames.Add((MagickImage)App.ToMagickImage(resultImage));
                }

                // Установка задержки между кадрами
                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }

                // Сохранение GIF анимации
                frames.Write(savingPath, MagickFormat.Gif);
                resultImage.Dispose();
                image.Dispose();
            }
            frames.Dispose();
            //}
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("Сначала загрузите изображение");
            //}
        }

        private void SaveFrontAnimation()
        {
            //try
            //{
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_FrontFlip.gif");
            savingPath = App.GetNextFileName(savingPath);

            // Создание коллекции кадров для анимации
            MagickImageCollection frames = new MagickImageCollection();

            // Число кадров в анимации
            int totalFrames = 30;

            // Задержка между кадрами (в миллисекундах)
            int animationDelay = 11; // Например, 100 мс

            // Загрузка исходного изображения
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                // Расчет пикселей смещения для движения вверх и вниз
                int offsetY = sourceImage.Height / 15;
                int newHeight = sourceImage.Height + offsetY * 15;
                int newWidth = sourceImage.Height;
                sourceImage.Extent(newWidth, newHeight, Gravity.South, MagickColors.White);
                // Добавление первого кадра (исходное изображение)
                frames.Add(sourceImage.Clone());

                System.Drawing.Image image = System.Drawing.Image.FromFile(loadedSprite);

                Bitmap resultImage = new Bitmap(newWidth, newHeight);



                float currentAngle = 0;
                // Добавление кадров с плавным движением вверх и вниз
                for (int i = 1; i <= totalFrames; i++)
                {

                    // Вычисление позиции Y с учетом смещения
                    int currentOffsetY = 0;

                    if (i >= 1 && i <= 15)
                    {
                        // Движение вверх
                        currentOffsetY = offsetY * (15 - (i - 1));
                    }
                    else if (i >= 16 && i <= 30)
                    {
                        // Движение вниз
                        currentOffsetY = offsetY * (i - 15);
                    }
                    currentAngle = 360f / totalFrames * i;

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, resultImage.Width, resultImage.Height));

                        //float centerX = resultImage.Width / 2;
                        //float centerY = resultImage.Height - currentOffsetY - image.Height / 2 - offsetY * 15;
                        float pivotX = resultImage.Width / 2;
                        float pivotY = resultImage.Height - image.Height / 2;
                        int correction = 0;

                        graphics.TranslateTransform(pivotX, pivotY);
                        graphics.RotateTransform(currentAngle);
                        graphics.TranslateTransform(-pivotX, -pivotY);

                        //graphics.TranslateTransform(centerX, centerY);
                        //graphics.RotateTransform(currentAngle);
                        //graphics.TranslateTransform(-centerX, -centerY);

                        graphics.DrawImage(image, newWidth/2-image.Width/2, currentOffsetY);
                    }
                    // Добавление кадра в коллекцию
                    frames.Add((MagickImage)App.ToMagickImage(resultImage));
                }

                // Установка задержки между кадрами
                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }

                // Сохранение GIF анимации
                frames.Write(savingPath, MagickFormat.Gif);
                resultImage.Dispose();
                image.Dispose();
            }
            frames.Dispose();
            //}
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("Сначала загрузите изображение");
            //}
        }

        private void SaveBackAnimation()
        {
            //try
            //{
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_FrontFlip.gif");
            savingPath = App.GetNextFileName(savingPath);

            // Создание коллекции кадров для анимации
            MagickImageCollection frames = new MagickImageCollection();

            // Число кадров в анимации
            int totalFrames = 30;

            // Задержка между кадрами (в миллисекундах)
            int animationDelay = 11; // Например, 100 мс

            // Загрузка исходного изображения
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                // Расчет пикселей смещения для движения вверх и вниз
                int offsetY = sourceImage.Height / 15;
                int newHeight = sourceImage.Height + offsetY * 15;
                int newWidth = sourceImage.Height;
                sourceImage.Extent(newWidth, newHeight, Gravity.South, MagickColors.White);
                // Добавление первого кадра (исходное изображение)
                frames.Add(sourceImage.Clone());

                System.Drawing.Image image = System.Drawing.Image.FromFile(loadedSprite);

                Bitmap resultImage = new Bitmap(newWidth, newHeight);


                int currentAngle = 0;
                // Добавление кадров с плавным движением вверх и вниз
                for (int i = 1; i <= totalFrames; i++)
                {

                    // Вычисление позиции Y с учетом смещения
                    int currentOffsetY = 0;

                    if (i >= 1 && i <= 15)
                    {
                        // Движение вверх
                        currentOffsetY = offsetY * (15 - (i - 1));
                    }
                    else if (i >= 16 && i <= 30)
                    {
                        // Движение вниз
                        currentOffsetY = offsetY * (i - 15);
                    }
                    currentAngle = 360 / totalFrames * i;

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, resultImage.Width, resultImage.Height));

                        float centerX = newWidth;
                        float centerY = currentOffsetY * image.Height / 2;
                        graphics.TranslateTransform(centerX, centerY);
                        graphics.RotateTransform(currentAngle);
                        graphics.TranslateTransform(-centerX, -centerY);

                        graphics.DrawImage(image, newWidth, currentOffsetY);
                    }
                    // Добавление кадра в коллекцию
                    frames.Add((MagickImage)App.ToMagickImage(resultImage));
                }

                // Установка задержки между кадрами
                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }

                // Сохранение GIF анимации
                frames.Write(savingPath, MagickFormat.Gif);
                resultImage.Dispose();
                image.Dispose();
            }
            frames.Dispose();
            //}
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("Сначала загрузите изображение");
            //}
        }

        string selectedAnimation = "";
        private void ExportGIF(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedAnimation == "jump")
                {
                    SaveJumpAnimation();
                }
                else if (selectedAnimation == "backflip")
                {
                    SaveBackAnimation();
                }
                else if (selectedAnimation == "frontflip")
                {
                    SaveFrontAnimation();
                }
                else
                {
                    MessageBox.Show("???");
                    return;
                }
                MessageBox.Show("Спрайт персонажа экпортирован на рабочий стол.");
                return;

            }
            catch (ArgumentException)
            {
                MessageBox.Show("Сначала загрузите изображение");
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
                return;
            }
        }

        BrushConverter bc = new BrushConverter();
        private void BtnChangeState(Button sender)
        {
            sender.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#bee6fd");
            sender.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("Black");
        }
        private void BackFlip_Click(object sender, RoutedEventArgs e)
        {
            BtnChangeState((Button)sender);
            selectedAnimation = "backflip";

            Jump_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");
            FrontFlip_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");

            Jump_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");
            FrontFlip_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");
        }

        private void Jump_Click(object sender, RoutedEventArgs e)
        {
            BtnChangeState((Button)sender);
            selectedAnimation = "jump";

            BackFlip_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");
            FrontFlip_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");

            BackFlip_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");
            FrontFlip_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");
        }

        private void FrontFlip_Click(object sender, RoutedEventArgs e)
        {
            BtnChangeState((Button)sender);
            selectedAnimation = "frontflip";

            Jump_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");
            BackFlip_Btn.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#151525");

            Jump_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");
            BackFlip_Btn.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("White");

        }


    }
}
