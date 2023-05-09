using System;
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

        private Button _lastClickedBtn = null;
        BrushConverter bc = new BrushConverter();
        public MainWindow()
        {
            InitializeComponent();
            _lastClickedBtn = BtnConstructor;
            Main.Content = new Pages.Constructor();
        }

        private void BtnChangeState(Button lastClickedBtn, Button sender)
        {
            //sender.IsEnabled = false;
            //lastClickedBtn.IsEnabled = true;
            lastClickedBtn.Background = (Brush)(bc.ConvertFrom("#252540"));
            sender.Background = (Brush)bc.ConvertFrom("#151525");
        }

        private void BtnClickConstructor(object sender, RoutedEventArgs e)
        {
            if (_lastClickedBtn != (Button)sender)
            {
                Main.Content = new Pages.Constructor();
                BtnChangeState(_lastClickedBtn, (Button)sender);
                _lastClickedBtn = (Button)sender;
            }
        }

        private void BtnClickSaved(object sender, RoutedEventArgs e)
        { 
            if (_lastClickedBtn != (Button)sender)
            {
                Main.Content = new Pages.SavedSprites();
                BtnChangeState(_lastClickedBtn, (Button)sender);
                _lastClickedBtn = (Button)sender;
            }
        }

        private void BtnClickTemplates(object sender, RoutedEventArgs e)
        {    
            if (_lastClickedBtn != (Button)sender)
            {
                Main.Content = new Pages.Templates();
                BtnChangeState(_lastClickedBtn, (Button)sender);
                _lastClickedBtn = (Button)sender;
            }
        }
    }
}
