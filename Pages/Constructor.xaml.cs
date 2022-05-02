using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Constructor.xaml
    /// </summary>
    public partial class Constructor : Page
    {
        //head
        int HeadX = 0;
        int HeadY = 0;
        int HeadWidth = 23;
        int HeadHeight = 20;
        //body
        int BodyX = 0;
        int BodyY = 32;
        int BodyWidth = 23;
        int BodyHeight = 20;
        //legs
        int LegsX = 0;
        int LegsY = 80;
        int LegsWidth = 23;
        int LegsHeight = 20;

        List<BitmapImage> colorSheets = new List<BitmapImage>();
        int currentHeadIdx = 0;
        int currentBodyIdx = 0;
        int currentLegsIdx = 0;
        BitmapImage Original = new BitmapImage(new Uri("../../Images/Idle/Original/Idle.PNG", UriKind.Relative));
        BitmapImage ColorBlue = new BitmapImage(new Uri("../../Images/Idle/ColorBlue/Idle.PNG", UriKind.Relative));
        BitmapImage ColorGreen = new BitmapImage(new Uri("../../Images/Idle/ColorGreen/Idle.PNG", UriKind.Relative));

        public Constructor()
        {
            InitializeComponent();
            colorSheets.Add(Original);
            colorSheets.Add(ColorBlue);
            colorSheets.Add(ColorGreen);
            Head.Source = new CroppedBitmap(colorSheets[currentHeadIdx], new Int32Rect(HeadX, HeadY, HeadWidth, HeadHeight));
            Body.Source = new CroppedBitmap(colorSheets[currentBodyIdx], new Int32Rect(BodyX, BodyY, BodyWidth, BodyHeight));
            Legs.Source = new CroppedBitmap(colorSheets[currentLegsIdx], new Int32Rect(LegsX, LegsY, LegsWidth, LegsHeight));

        }

        private void update()
        {
            Head.Source = new CroppedBitmap(colorSheets[currentHeadIdx], new Int32Rect(HeadX, HeadY, HeadWidth, HeadHeight));
            Body.Source = new CroppedBitmap(colorSheets[currentBodyIdx], new Int32Rect(BodyX, BodyY, BodyWidth, BodyHeight));
            Legs.Source = new CroppedBitmap(colorSheets[currentLegsIdx], new Int32Rect(LegsX, LegsY, LegsWidth, LegsHeight));
        }


        private void NextHead(object sender, RoutedEventArgs e)
        {
            currentHeadIdx++;
            currentHeadIdx = currentHeadIdx % colorSheets.Count;
            update();
        }

        private void NextBody(object sender, RoutedEventArgs e)
        {
            currentBodyIdx++;
            currentBodyIdx = currentBodyIdx % colorSheets.Count;
            update();
        }

        private void NextLegs(object sender, RoutedEventArgs e)
        {
            currentLegsIdx++;
            currentLegsIdx = currentLegsIdx % colorSheets.Count;
            update();
        }

        private void PrevLegs(object sender, RoutedEventArgs e)
        {
            currentLegsIdx--;
            currentLegsIdx = (currentLegsIdx + 3) % colorSheets.Count;
            update();
        }

        private void PrevBody(object sender, RoutedEventArgs e)
        {
            currentBodyIdx--;
            currentBodyIdx = (currentBodyIdx + 3) % colorSheets.Count;
            update();
        }

        private void PrevHead(object sender, RoutedEventArgs e)
        {
            currentHeadIdx--;
            currentHeadIdx = (currentHeadIdx + 3) % colorSheets.Count;
            update();
        }
    }
}
