using Microsoft.Win32;
using Smart_Library_Management_System.Member_Windows;
using Smart_Library_Management_System.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
                BitmapImage bmpImg = ConvertBitmapToBitmapImage(TempImageStorer.image);

                int cropWidth = 320; // Adjust as needed
                int cropHeight = 300; // Adjust as needed
                int cropX = (bmpImg.PixelWidth - cropWidth) / 2;
                int cropY = (bmpImg.PixelHeight - cropHeight) / 2;

                // Create a cropped version of the image
                CroppedBitmap croppedImage = new CroppedBitmap(
                    bmpImg,
                    new Int32Rect(cropX, cropY, cropWidth, cropHeight)
                );
                imagePicture.Source = croppedImage;
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
                bool shouldAddAcc = true;
                if (tbPassword.Text == tbRepeatPassword.Text)
                {
                    BitmapSource img = imagePicture.Source as BitmapSource;
                    var query = from a in _SLMS.Accounts
                                select a;

                    foreach (var acc in query)
                    {
                        if (acc.Username == tbUsername.Text || acc.First_Name == tbFirstName.Text)
                        {
                            MessageBox.Show("Your username and/or password already exists in the database.");
                            shouldAddAcc = false;
                            break;
                        }
                    }
                    if (shouldAddAcc)
                    {
                        _SLMS.Prod_CreateAccount(tbUsername.Text, tbPassword.Text, tbFirstName.Text, tbLastName.Text, BitmapSourceToByteArray(img));
                        _SLMS.SubmitChanges();
                        MessageBox.Show($"Your member account has been created successfully, {tbFirstName.Text} {tbLastName.Text}.");
                    }
                }
                else
                {
                    MessageBox.Show("Password does not match!");
                    tbRepeatPassword.Text = string.Empty;
                }
            }
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
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

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbUsername.Text.Length > 8)
            {
                MessageBox.Show("Maximum of 8 characters only is allowed.");
                tbUsername.Text = string.Empty;
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassword.Text.Length > 8) 
            {
                MessageBox.Show("Maximum of 8 characters only is allowed.");
                tbPassword.Text = string.Empty;
            }
        }

        private void tbRepeatPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbRepeatPassword.Text.Length > 8)
            {
                MessageBox.Show("Maximum of 8 characters only is allowed.");
                tbRepeatPassword.Text = string.Empty;
            }
        }
    }
}
