using Microsoft.Win32;
using Smart_Library_Management_System.Member_Windows;
using Smart_Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Signup_Page.xaml
    /// </summary>
    public partial class Signup_Page : Window
    {
        SLMSDataContext _SLMS = null;
        public Signup_Page()
        {
            InitializeComponent();
            _SLMS = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);
        }

        private void btnTakeAPhoto_Click(object sender, RoutedEventArgs e)
        {
            TakeAPhoto takeAPhoto = new TakeAPhoto();
            takeAPhoto.ShowDialog();

            if (TempImageStorer.image != null)
            {
                imagePicture.Source = ConvertBitmapToBitmapImage(TempImageStorer.image);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {
                imagePicture.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }

        private void btnAlreadyAMember_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LP = new MainWindow();
            LP.Show();
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) || string.IsNullOrEmpty(tbLastName.Text) || string.IsNullOrEmpty(tbUsername.Text)
                || string.IsNullOrEmpty(tbPassword.Text) || string.IsNullOrEmpty(tbRepeatPassword.Text) || imagePicture.Source == null)
            {
                MessageBox.Show("You must fill up all fields!");
            }
            else 
            {
                if (tbPassword.Text == tbRepeatPassword.Text)
                {
                    BitmapSource img = imagePicture.Source as BitmapSource;
                    _SLMS.Prod_CreateAccount(tbUsername.Text, tbPassword.Text, tbFirstName.Text, tbLastName.Text, BitmapSourceToByteArray(img));
                    _SLMS.SubmitChanges();
                    MessageBox.Show($"Your member account has been created successfully, {tbFirstName.Text} {tbLastName}.");
                }
                else
                {
                    MessageBox.Show("Password does not match!");
                    tbRepeatPassword.Text = string.Empty;
                }
            }
        }
        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            TempImageStorer.memStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = TempImageStorer.memStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private byte[] BitmapSourceToByteArray(BitmapSource bitmapSource)
        {
            byte[] byteArray;
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                byteArray = ms.ToArray();
            }
            return byteArray;
        }
    }
}
