using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Templates.xaml
    /// </summary>
    public partial class Templates : Page
    {
        private List<string> _uploadedFiles = new List<string>();
        public Templates()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                _uploadedFiles.Add(filePath);
                UploadedFilesComboBox.ItemsSource = _uploadedFiles;
            }
        }
    }
}
