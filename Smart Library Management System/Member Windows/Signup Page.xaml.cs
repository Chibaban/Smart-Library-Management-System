using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
            MessageBox.Show("Take a Photo");
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
            if (tbPassword.Text == tbRepeatPassword.Text)
            {
                _SLMS.Prod_CreateAccount(tbUsername.Text, tbPassword.Text, tbFirstName.Text, tbLastName.Text, ConvertImageToByteArray(imagePicture));
            }
            else
            {
                MessageBox.Show("Password does not match!");
                tbRepeatPassword.Text = string.Empty;
            }
        }
        private byte[] ConvertImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
