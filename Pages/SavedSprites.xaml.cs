using Newtonsoft.Json;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Web.Script.Serialization;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для SavedSprites.xaml
    /// </summary>
    public partial class SavedSprites : Page
    {

        private void LoadImages()
        {
            string dirPath = @"../../SavedSprites";
            if (Directory.Exists(dirPath))
            {
                string[] imageFiles = Directory.GetFiles(dirPath, "*.png"); // Здесь можно указать нужное расширение файлов
                 
                for (int i = 0; i < imageFiles.Length && i < 15; i++)
                {
                    TextBlock name = ((TextBlock)this.FindName($"Name{i + 1}"));
                    Image imageControl = (Image)FindName($"Pic{i + 1}");

                    name.Text = Path.GetFileNameWithoutExtension(imageFiles[i]);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Path.GetFullPath(imageFiles[i]));
                    bitmap.EndInit();
                    imageControl.Source = bitmap;
                }
            }
        }


        private Border selected = null;
        private void UnselectSprite()
        {
            if (selected != null)
            {
                selected.BorderBrush = Brushes.Transparent;
            }
            selected = null;
        }
        private void SelectSprite(object sender, MouseEventArgs e)
        {
            if (selected == (Border)sender)
            {
                UnselectSprite();
                return;
            }   
            UnselectSprite();
            selected = (Border)sender;
            ((Border)sender).BorderBrush = Brushes.White;
        }

        public SavedSprites()
        {
            InitializeComponent();
            LoadImages();
        }

        private void PrevPage(object sender, RoutedEventArgs e)
        {
            //
        }
        private void NextPage(object sender, RoutedEventArgs e)
        {
            //
        }
        private void SelectSprite(object sender, RoutedEventArgs e)
        {
            //
        }
        private void SaveSelectedSprite(object sender, RoutedEventArgs e)
        {
            //
        }
        private void DeleteSelectedSprite(object sender, RoutedEventArgs e)
        {
            //
        }

    }


}
