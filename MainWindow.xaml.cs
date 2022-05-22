using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpriteGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            RadioButton radioButton = new RadioButton();
            InitializeComponent();
            Main.Content = new Pages.Constructor();
        }

        private void BtnClickConstructor(object sender, RoutedEventArgs e)
        {
            Main.Content = new Pages.Constructor();
            var bc = new BrushConverter();
            BtnConstructor.Background = (Brush)(bc.ConvertFrom("#151525"));
            BtnSaved.Background = (Brush)(bc.ConvertFrom("#252540"));
        }

        private void BtnClickSaved(object sender, RoutedEventArgs e)
        {
            Main.Content = new Pages.SavedSprites();
            var bc = new BrushConverter();
            BtnSaved.Background = (Brush)(bc.ConvertFrom("#151525"));
            BtnConstructor.Background = (Brush)(bc.ConvertFrom("#252540"));
        }
    }
}
