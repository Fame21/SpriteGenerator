using ImageMagick;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveJumpAnimation()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + ".gif");
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
                        graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, resultImage.Width, resultImage.Height));
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

        }

        private void ExportGIF(object sender, RoutedEventArgs e)
        {


            //try
            //{
            SaveJumpAnimation();
            MessageBox.Show("Спрайт персонажа экпортирован на рабочий стол.");
            return;

            //}
            //catch (Exception exeption)
            //{
            //    MessageBox.Show(exeption.Message);
            //    return;
            //}
        }
    }
}
