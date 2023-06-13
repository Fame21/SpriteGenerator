using ImageMagick;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
            openFileDialog.Filter = "PNG files (*.png)|*.png";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
        private void UploadSprite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newFilePath = LoadStringFromDialog();
                if (newFilePath == null)
                {
                    return;
                }

                loadedSprite = newFilePath;
                using (MagickImage sourceImage = new MagickImage(loadedSprite))
                {
                    if ((int)sourceImage.Height != 49 || (int)sourceImage.Width != 27)
                    {
                        loadedSprite = String.Empty;
                        sourceImage.Dispose();
                        throw new Exception();
                        return;

                    }
                }

                    ChangeImageControlSource(PreviewImageControl, loadedSprite);
                SpriteNameControl.Content = Path.GetFileNameWithoutExtension(loadedSprite);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Пожалуйста загрузите спрайт созданный в данном приложении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        int totalFrames = 30; //Количетво кадров в анимации
        int animationDelay = 5; //Задержка между кадрами


        private void SaveJumpAnimation()
        {
            //try
            //{
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_Jump.gif");
            savingPath = App.GetNextFileName(savingPath);

            // Создание коллекции кадров для анимации
            MagickImageCollection frames = new MagickImageCollection();
            // Загрузка исходного изображения
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                float currentAngle = 0;
                var offsetY = sourceImage.Height;

                for (int i = 0; i < totalFrames; i++)
                {


                    int currentOffsetY = 0;
                    if (i >= 0 && i <= totalFrames / 2)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * i;
                    }
                    else if (i > totalFrames / 2 && i < totalFrames)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * (totalFrames - i);
                    }


                    MagickImage canvas = new MagickImage(MagickColors.Transparent, sourceImage.Width, sourceImage.Height * 2);
                    MagickImage newFrameImage = (MagickImage)sourceImage.Clone();

                    newFrameImage.BackgroundColor = MagickColors.Transparent;
                    newFrameImage.Rotate(currentAngle);

                    int drawX = (canvas.Width - newFrameImage.Width) / 2;
                    int drawY = canvas.Height - newFrameImage.Height / 2 - sourceImage.Height / 2 - currentOffsetY;


                    canvas.Composite(newFrameImage, drawX, drawY, CompositeOperator.Over);

                    // Добавление кадра в коллекцию
                    frames.Add(canvas);
                }

                // Установка задержки между кадрами
                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }
                frames.Optimize();
                frames.Coalesce();
                // Сохранение GIF анимации
                frames.Write(savingPath, MagickFormat.Gif);
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
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_FrontFlip.gif");
            savingPath = App.GetNextFileName(savingPath);

            MagickImageCollection frames = new MagickImageCollection();
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                float currentAngle = 0;
                var offsetY = sourceImage.Height;

                for (int i = 0; i < totalFrames; i++)
                {
                    MagickImage canvas = new MagickImage(MagickColors.Transparent, sourceImage.Height * 2, sourceImage.Height * 2);
                    MagickImage newFrameImage = (MagickImage)sourceImage.Clone();
                    currentAngle = 360f / totalFrames * i;

                    int currentOffsetY = 0;
                    int currentOffsetX = 0;

                    if (i >= 0 && i <= totalFrames / 2)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * i;
                    }
                    else if (i > totalFrames / 2 && i < totalFrames)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * (totalFrames - i);
                    }

                    currentOffsetX = ((canvas.Width - sourceImage.Width) / totalFrames) * i;


                    newFrameImage.BackgroundColor = MagickColors.Transparent;
                    newFrameImage.Rotate(currentAngle);

                    int drawX = (canvas.Width  - newFrameImage.Width) / 2 - canvas.Width  / 2 + sourceImage.Width / 2 + currentOffsetX;
                    int drawY =  canvas.Height - newFrameImage.Height / 2 - sourceImage.Height / 2 - currentOffsetY;


                    canvas.Composite(newFrameImage, drawX, drawY, CompositeOperator.Over);

                    frames.Add(canvas);
                }

                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }
                frames.Optimize();
                frames.Coalesce();
                frames.Write(savingPath, MagickFormat.Gif);
            }
            frames.Dispose();
        }

        private void SaveBackAnimation()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, Path.GetFileNameWithoutExtension(loadedSprite) + "_BackFlip.gif");
            savingPath = App.GetNextFileName(savingPath);

            MagickImageCollection frames = new MagickImageCollection();
            using (MagickImage sourceImage = new MagickImage(loadedSprite))
            {
                float currentAngle = 0;
                var offsetY = sourceImage.Height;

                for (int i = 0; i < totalFrames; i++)
                {
                    MagickImage canvas = new MagickImage(MagickColors.Transparent, sourceImage.Height * 2, sourceImage.Height * 2);
                    MagickImage newFrameImage = (MagickImage)sourceImage.Clone();
                    currentAngle = 360f / totalFrames * i;

                    int currentOffsetY = 0;
                    int currentOffsetX = 0;

                    if (i >= 0 && i <= totalFrames / 2)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * i;
                    }
                    else if (i > totalFrames / 2 && i < totalFrames)
                    {
                        currentOffsetY = (offsetY / (totalFrames / 2)) * (totalFrames - i);
                    }

                    currentOffsetX = ((canvas.Width - sourceImage.Width) / totalFrames) * i;


                    newFrameImage.BackgroundColor = MagickColors.Transparent;
                    newFrameImage.Rotate(-currentAngle);

                    int drawX = (canvas.Width - newFrameImage.Width) / 2 + canvas.Width / 2 - sourceImage.Width / 2 - currentOffsetX;
                    int drawY = canvas.Height - newFrameImage.Height / 2 - sourceImage.Height / 2 - currentOffsetY;


                    canvas.Composite(newFrameImage, drawX, drawY, CompositeOperator.Over);

                    frames.Add(canvas);
                }

                foreach (MagickImage frame in frames)
                {
                    frame.AnimationDelay = animationDelay;
                }
                frames.Optimize();
                frames.Coalesce();
                frames.Write(savingPath, MagickFormat.Gif);
            }
            frames.Dispose();
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
                    MessageBox.Show("Сначала выберите тип анимации");
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
