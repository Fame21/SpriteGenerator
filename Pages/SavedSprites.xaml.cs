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

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для SavedSprites.xaml
    /// </summary>
    public partial class SavedSprites : Page
    {
        //head
        int headX = 1;
        int headY = 1;
        int headWidth = 25;
        int headHeight = 22;

        //body
        int bodyX = 1;
        int bodyY = 1;
        int bodyWidth = 25;
        int bodyHeight = 22;

        //legs
        int legsX = 1;
        int legsY = 1;
        int legsWidth = 15;
        int legsHeight = 17;

        BitmapImage head = new BitmapImage(new Uri("../../Images/redacted/head.png", UriKind.Relative));
        BitmapImage body = new BitmapImage(new Uri("../../Images/redacted/body.png", UriKind.Relative));
        BitmapImage legs = new BitmapImage(new Uri("../../Images/redacted/legs.png", UriKind.Relative));

        int pageIdx = 1;
        int pageCount = 1;

        public SavedSprites()
        {
            InitializeComponent();
            loadPage();
        }


        private WriteableBitmap mergeParts(int headIdx, int bodyIdx, int legsIdx)
        {
            WriteableBitmap amalgam = new WriteableBitmap(
                27, // width
                49, // height
                96, // dpiX
                96, // dpiY
                PixelFormats.Bgra32, // pixel format
                null); // bitmap pallete

            GetHead(headIdx);
            GetBody(bodyIdx);
            GetLegs(legsIdx);

            BitmapSource headSource = new CroppedBitmap(head, new Int32Rect(headX, headY, headWidth, headHeight));
            BitmapSource bodySource = new CroppedBitmap(body, new Int32Rect(bodyX, bodyY, bodyWidth, bodyHeight));
            BitmapSource legsSource = new CroppedBitmap(legs, new Int32Rect(legsX, legsY, legsWidth, legsHeight));

            byte[] headPixels = BitmapSourceToArray(headSource);
            byte[] bodyPixels = BitmapSourceToArray(bodySource);
            byte[] legsPixels = BitmapSourceToArray(legsSource);


            byte[] amalgamPixels = BitmapSourceToArray(amalgam);


            int amalgamPixelsIdx = 0;
            int PixelsIdx = 0;

            for (int i = 0; i < legsSource.PixelHeight; i++)
            {
                for (int j = 0; j < legsSource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 31 * 4 + 6 * 4);
                    PixelsIdx = i * 4 * legsSource.PixelWidth + j * 4;
                    if (legsPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = legsPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = legsPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = legsPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = legsPixels[PixelsIdx + 3];
                }
            }

            for (int i = 0; i < headSource.PixelHeight; i++)
            {
                for (int j = 0; j < headSource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 1 * 4 + 1 * 4);
                    PixelsIdx = i * 4 * headSource.PixelWidth + j * 4;
                    if (headPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = headPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = headPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = headPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = headPixels[PixelsIdx + 3];
                }
            }

            for (int i = 0; i < bodySource.PixelHeight; i++)
            {
                for (int j = 0; j < bodySource.PixelWidth; j++)
                {
                    amalgamPixelsIdx = j * 4 + (i * 4 * amalgam.PixelWidth) + (amalgam.PixelWidth * 18 * 4 + 1 * 4);
                    PixelsIdx = i * 4 * bodySource.PixelWidth + j * 4;
                    if (bodyPixels[PixelsIdx + 3] != 255)
                    {
                        continue;
                    }
                    amalgamPixels[amalgamPixelsIdx + 0] = bodyPixels[PixelsIdx + 0];
                    amalgamPixels[amalgamPixelsIdx + 1] = bodyPixels[PixelsIdx + 1];
                    amalgamPixels[amalgamPixelsIdx + 2] = bodyPixels[PixelsIdx + 2];
                    amalgamPixels[amalgamPixelsIdx + 3] = bodyPixels[PixelsIdx + 3];
                }
            }

            amalgam.WritePixels(new Int32Rect(0, 0, 27, 49), amalgamPixels, amalgam.PixelWidth * (amalgam.Format.BitsPerPixel / 8), 0);

            return amalgam;
        }

        private byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }


        private void GetLegs(int num = 0)
        {
            legsX = 1;
            legsX = legsX + (legsWidth + 1) * num;
            legsX = legsX % (legs.PixelWidth - 1);
        }
        private void GetBody(int num = 0)
        {
            bodyX = 1;
            bodyX += (bodyWidth + 1) * num;
            bodyX = bodyX % (body.PixelWidth - 1);
        }
        private void GetHead(int num = 0)
        {
            headX = 1;
            headX += (headWidth + 1) * num;
            headX = headX % (head.PixelWidth - 1);
        }



        private dynamic loadJson()
        {
            string jsonPath = "../../SavedSprites/Saved.JSON";
            string jsonData = "";

            if (File.Exists(jsonPath))
            {
                if (File.ReadAllText(jsonPath) == "")
                {
                    return null;
                }
                else
                {
                    jsonData = JsonConvert.DeserializeObject(File.ReadAllText(jsonPath)).ToString();
                }
            }
            else
            {
                return null;
            }

            dynamic jss = new JavaScriptSerializer();
            dynamic data = jss.Deserialize<dynamic>(jsonData);

            return (data, jsonPath);
        }

        private void loadPage(int page = 1)
        {

            dynamic data = loadJson().Item1;

            if (data == null)
            {
                return;
            }


            pageCount = (data.Count / 15) + 1;
            int lastPageCount = data.Count % 15;
            for (int i = 0; i < 15; i++)
            {
                string picIdx = "Pic" + (i + 1);
                string nameIdx = "Name" + (i + 1);
                var myTextBlock = (TextBlock)this.FindName(nameIdx);
                myTextBlock.Text = "Пусто";
                var myImgBlock = (Image)this.FindName(picIdx);
                myImgBlock.Source = null;
            }

            int divisor = pageCount - 1;
            int border = 0;
            if (pageCount == 1)
            {
                divisor++;
                border = lastPageCount;
                NextPageBtn.IsEnabled = false;
                PrevPageBtn.IsEnabled = false;
                PrevPageBtn.Background = new SolidColorBrush(Colors.Transparent);
                NextPageBtn.Background = new SolidColorBrush(Colors.Transparent);
            } 
            else
            {
                int reduced = page * (pageCount - page);
                var ifNotLast = pageCount * reduced / divisor - reduced;
                var ifLast = 1 - ifNotLast;
                border = (15 * ifNotLast) + lastPageCount * (ifLast);
                NextPageBtn.IsEnabled = true;
                PrevPageBtn.IsEnabled = true;
            }


            for (int i = 0; i < border; i++)
            {
                string spriteIdx = "sprite" + (i + (page-1) * 15);
                string picIdx = "Pic" + (i + 1);
                string nameIdx = "Name" + (i + 1);

                string nameText = data[spriteIdx]["name"];
                dynamic partsList = data[spriteIdx]["parts"];

                WriteableBitmap amalgam = mergeParts(partsList[0], partsList[1], partsList[2]);
                var myTextBlock = (TextBlock)this.FindName(nameIdx);
                myTextBlock.Text = nameText;
                var myImgBlock = (Image)this.FindName(picIdx);
                myImgBlock.Source = amalgam;
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

        private void ExportSprite(string spriteName, Image spriteImage)
        {
            string fileName = spriteName + ".png";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savingPath = Path.Combine(desktopPath, fileName);

            savingPath = GetNextFileName(savingPath);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)spriteImage.Source));
            try
            {
                using (FileStream stream = new FileStream(savingPath, FileMode.Create))
                    encoder.Save(stream);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
        } 

        private void DeleteSprite(int deleteIdx)
        {
            dynamic data = loadJson().Item1;
            string path = loadJson().Item2;

            if (data == null)
            {
                return;
            }

            int initialSize = data.Count;
            for (int i = deleteIdx; i < data.Count - 2; i++)
            {
                data["sprite" + i] = data["sprite" + (i + 1)];
            }
            data.Remove("sprite" + (data.Count - 1));


            string output = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, output);

            int page = (deleteIdx) / 15 + 1;

            loadPage(page);
            UnselectSprite();
        }

        private string GetNextFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string pathName = Path.GetDirectoryName(fileName);
            string fileNameOnly = Path.Combine(pathName, Path.GetFileNameWithoutExtension(fileName));
            int i = 0;
            // If the file exists, keep trying until it doesn't
            while (File.Exists(fileName))
            {
                i += 1;
                fileName = string.Format("{0}({1}){2}", fileNameOnly, i, extension);
            }
            return fileName;
        }

        private void SaveSelectedSprite(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                return;
            }
            string idx = selected.Name.Substring(6);
            string name = ((TextBlock)this.FindName("Name" + idx)).Text;
            Image image = (Image)this.FindName("Pic" + idx);
            if (image.Source != null)
            {
                ExportSprite(name, image);
            }
        }

        private void DeleteSelectedSprite(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                return;
            }

            int itemIdx = Int32.Parse(selected.Name.Substring(6));
            int jsonIdx = itemIdx - 1 + (pageIdx - 1) * 15;
            Image image = (Image)this.FindName("Pic" + itemIdx);


            if (image.Source != null)
            {
                DeleteSprite(jsonIdx);
            }
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            UnselectSprite();
            pageIdx = ((pageIdx++) % pageCount) + 1;
            loadPage(pageIdx);
            PageNum.Text = "Страница: " + pageIdx;
        }


        private void PrevPage(object sender, RoutedEventArgs e)
        {
            UnselectSprite();
            pageIdx = (((pageIdx-- + pageCount) % pageCount)) + 1;
            loadPage(pageIdx);
            PageNum.Text = "Страница: " + pageIdx;
        }

    }
}
