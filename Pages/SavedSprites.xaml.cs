using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для SavedSprites.xaml
    /// </summary>
    public partial class SavedSprites : Page
    {

        private BitmapImage bitmap;
        string[] imageFiles = new string[0];
        int pageCount = 1;
        int currentPage = 1;

        private void LoadImages()
        {
            pageCount = 1;
            string dirPath = @"../../SavedSprites";
            imageFiles = new string[0];
            bitmap = null;
            if (Directory.Exists(dirPath))
            {
                imageFiles = Directory.GetFiles(dirPath, "*.png"); // Здесь можно указать нужное расширение файлов
                imageFiles = imageFiles.OrderBy(f => File.GetCreationTime(f)).ToArray();
                if (imageFiles.Length > 15)
                {
                    pageCount += imageFiles.Length / 15;
                }
            }
        }

        private void LoadPage(int id)
        {
            PageNum.Text = id.ToString();
            int pageInc = (id - 1) * 15;
            for (int i = 0; i < 15; i++)
            {
                TextBlock nameControl = ((TextBlock)FindName($"Name{i + 1}"));
                Image imageControl = (Image)FindName($"Pic{i + 1}");
                try
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(Path.GetFullPath(imageFiles[i + pageInc]));
                    bitmap.EndInit();
                    imageControl.Source = bitmap.Clone();
                    nameControl.Text = Path.GetFileNameWithoutExtension(imageFiles[i + pageInc]);
                }
                catch (Exception ex)
                {
                    imageControl.Source = null;
                    nameControl.Text = "Empty";
                }

            }
        }



        private Border selected = null;
        private void UnselectSprite()
        {
            if (selected != null)
            {
                selected.BorderBrush = System.Windows.Media.Brushes.Transparent;
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
            ((Border)sender).BorderBrush = System.Windows.Media.Brushes.White;
        }

        public SavedSprites()
        {
            InitializeComponent();
            LoadImages();
            LoadPage(currentPage);
        }

        private void PrevPage(object sender, RoutedEventArgs e)
        {
            currentPage = (currentPage - 2 + pageCount) % pageCount + 1;
            UnselectSprite();
            LoadPage(currentPage);
        }
        private void NextPage(object sender, RoutedEventArgs e)
        {
            currentPage = (currentPage % pageCount) + 1;
            UnselectSprite();
            LoadPage(currentPage);
        }

        private void SaveSelectedSprite(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Выберите спрайт персонажа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string idx = selected.Name.Substring(6);
            string name = ((TextBlock)FindName("Name" + idx)).Text;
            Image image = (Image)FindName("Pic" + idx);

            if (image.Source == null)
            {
                MessageBox.Show("Выбрана пустая ячейка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string fileName = name + ".png";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, fileName);

            savingPath = App.GetNextFileName(savingPath);

            try
            {
                using (FileStream fileStream = new FileStream(savingPath, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Используйте подходящий кодек изображения
                    BitmapSource bitmapSource = (BitmapSource)image.Source;
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(fileStream);

                }
                MessageBox.Show("Спрайт персонажа экпортирован на рабочий стол.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (Exception exeption)
            {
                MessageBox.Show($"Непредвиденная ошибка: {exeption}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        private void DeleteSelectedSprite(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Выберите спрайт персонажа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string idx = selected.Name.Substring(6);
            TextBlock name = (TextBlock)FindName("Name" + idx);
            Image image = (Image)FindName("Pic" + idx);

            if (image.Source == null)
            {
                MessageBox.Show("Выбрана пустая ячейка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                var deletingPath = imageFiles[int.Parse(idx)-1];
                // Check if the file exists before deleting
                if (File.Exists(deletingPath))
                {
                    UnselectSprite();
                    image.Source = null;
                    name.Text = "Empty";
                    bitmap = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    File.Delete(deletingPath);
                    MessageBox.Show("Спрайт успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{deletingPath}", "Неудача", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                LoadImages();
                LoadPage(currentPage);
            }
        }

    }


}
