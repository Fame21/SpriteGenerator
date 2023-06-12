using Microsoft.Win32;
using System;
using System.IO;
using System.Linq.Expressions;
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

        private void ChangeImageControlSource(Image imageControl, string imagePath)
        {
            //string[] filesEntries = Directory.GetFiles(imagePath);
            //string classicPath = Array.Find(filesEntries, file => Path.GetFileName(file) == "classic.png");
            try
            {
                string fullPath = Path.GetFullPath(imagePath);
                Uri uri = new Uri($"file:{fullPath}");
                BitmapImage bitmap = new BitmapImage(uri);
                imageControl.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }
        private void LoadDefaultTemplates(object sender, RoutedEventArgs e)
        {
            try
            {

                ChangeImageControlSource(HeadImageControl, App.HeadsPath);
                ChangeImageControlSource(BodyImageControl, App.BodiesPath);
                ChangeImageControlSource(LegsImageControl, App.LegsPath);
                HeadName.Text = Path.GetFileNameWithoutExtension(App.HeadsPath);
                BodyName.Text = Path.GetFileNameWithoutExtension(App.BodiesPath);
                LegsName.Text = Path.GetFileNameWithoutExtension(App.LegsPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void UploadHead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newFilePath = LoadStringFromDialog();
                if (newFilePath != null)
                {
                    App.HeadsPath = newFilePath;
                    ChangeImageControlSource(HeadImageControl, App.HeadsPath);
                    HeadName.Text = Path.GetFileNameWithoutExtension(App.HeadsPath);
                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadBody_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newFilePath = LoadStringFromDialog();
                if (newFilePath != null)
                {
                    App.BodiesPath = newFilePath;
                ChangeImageControlSource(BodyImageControl, App.BodiesPath);
                BodyName.Text = Path.GetFileNameWithoutExtension(App.BodiesPath);
                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UploadLegs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newFilePath = LoadStringFromDialog();
                if (newFilePath != null)
                {
                    App.LegsPath = newFilePath;
                ChangeImageControlSource(LegsImageControl, App.LegsPath);
                LegsName.Text = Path.GetFileNameWithoutExtension(App.LegsPath);
                    return;
                }
                else
                {
                    return;
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
