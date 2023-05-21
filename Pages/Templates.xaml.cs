using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Templates.xaml
    /// </summary>
    public partial class Templates : Page
    {
        public Templates()
        {
            InitializeComponent();
            Loaded += LoadDefaultTemplates;
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
        private void LoadDefaultTemplates(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeImageControlSource(HeadImageControl, App.HeadsPath);
                ChangeImageControlSource(BodyImageControl, App.BodiesPath);
                ChangeImageControlSource(LegsImageControl, App.LegsPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void UploadHead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.HeadsPath = LoadStringFromDialog();
                ChangeImageControlSource(HeadImageControl, App.HeadsPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteHead_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void UploadBody_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.BodiesPath = LoadStringFromDialog();
                ChangeImageControlSource(BodyImageControl, App.BodiesPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBody_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        private void UploadLegs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.LegsPath = LoadStringFromDialog();
                ChangeImageControlSource(LegsImageControl, App.LegsPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteLegs_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            //
        }


    }
}
