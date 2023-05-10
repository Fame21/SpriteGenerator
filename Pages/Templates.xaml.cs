using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SpriteGenerator.Pages
{
    /// <summary>
    /// Логика взаимодействия для Templates.xaml
    /// </summary>
    public partial class Templates : Page
    {
        private List<string> _uploadedHeads = new List<string>();
        private List<string> _uploadedBodies = new List<string>();
        private List<string> _uploadedLegs = new List<string>();

        private string HeadsPath = @"../../Images/Heads"; // замените на нужный путь
        private string BodiesPath = @"../../Images/Bodies"; // замените на нужный путь
        private string LegsPath = @"../../Images/Legs"; // замените на нужный путь

        public Templates()
        {
            InitializeComponent();
            Loaded += Templates_Loaded;
        }

        private void Templates_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Load Heads
            if (Directory.Exists(HeadsPath))
            {
                string[] filesEntries = Directory.GetFiles(HeadsPath);
                foreach (string File in filesEntries)
                {
                    _uploadedHeads.Add(File);
                }
                Head_ComboBox.ItemsSource = _uploadedHeads;
            }
            else
            {
                // skip
            }

            // Load Heads
            if (Directory.Exists(BodiesPath))
            {
                string[] filesEntries = Directory.GetFiles(BodiesPath);
                foreach (string File in filesEntries)
                {
                    _uploadedBodies.Add(File);
                }
                Body_ComboBox.ItemsSource = _uploadedBodies;
            }
            else
            {
                // skip
            }

            // Load Heads
            if (Directory.Exists(LegsPath))
            {
                string[] filesEntries = Directory.GetFiles(LegsPath);
                foreach (string File in filesEntries)
                {
                    _uploadedLegs.Add(File);
                }
                Legs_ComboBox.ItemsSource = _uploadedLegs;
            }
            else
            {
                // skip
            }
        }




        private void UploadHead_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;

            if (choofdlog.ShowDialog() == true)
            {
                string sFileName = choofdlog.FileName;
                string sFileName_Head = Path.GetFileNameWithoutExtension(sFileName);
                int fileCounter = 0;
                string multiplesCounter = "";
                bool uploading = true;
                while (uploading)
                    try
                    {

                        File.Copy(sFileName, Path.Combine(HeadsPath, sFileName_Head + multiplesCounter + ".png"));
                        _uploadedHeads.Add(sFileName);
                        uploading = false;
                    }
                    catch (IOException ex)
                    {
                        fileCounter++;
                        multiplesCounter = " (" + fileCounter + ")";
                    }
            } 
            Head_ComboBox.ItemsSource = _uploadedHeads;
        }

        private void DeleteHead_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void UploadBody_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;

            if (choofdlog.ShowDialog() == true)
            {
                string sFileName = choofdlog.FileName;
                string sFileName_Body = Path.GetFileNameWithoutExtension(sFileName);
                int fileCounter = 0;
                string multiplesCounter = "";
                bool uploading = true;
                while (uploading)
                    try
                    {

                        File.Copy(sFileName, Path.Combine(HeadsPath, sFileName_Body + multiplesCounter + ".png"));
                        _uploadedHeads.Add(sFileName);
                        uploading = false;
                    }
                    catch (IOException ex)
                    {
                        fileCounter++;
                        multiplesCounter = " (" + fileCounter + ")";
                    }
            }
            Head_ComboBox.ItemsSource = _uploadedHeads;
        }

        private void DeleteBody_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        private void UploadLegs_Click(object sender, RoutedEventArgs e)
        {
            //
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
