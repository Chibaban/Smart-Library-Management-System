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
using System.Windows.Shapes;
using System.IO;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Admin_User_Profile_Page.xaml
    /// </summary>
    public partial class Admin_User_Profile_Page : Window
    {
        SLMSDataContext _SLMS = null;
        private string _username = "";

        public Admin_User_Profile_Page()
        {
            InitializeComponent();
            _SLMS = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);
        }

        public Admin_User_Profile_Page(string username)
        {
            InitializeComponent();

            var accountType = from a in Connections._slms.Accounts
                              where
                              a.Username == username
                              select a;
            
            foreach (var account in accountType)
            {
                if (account.Acc_Type == "Admin")
                {
                    if (username == account.Username)
                    {
                        tbAccountID.Text = User.Account_ID;
                        tbAccountType.Text = User.AccountType;
                        tbUsername.Text = User.AccountUsername;
                        tbPassword.Text = User.AccountPassword;
                        tbFirstName.Text = User.FirstName;
                        tbLastName.Text = User.LastName;

                        if (account.Acc_Image != null)
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            using (MemoryStream stream = new MemoryStream(account.Acc_Image.ToArray()))
                            {
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = stream;
                                bitmapImage.EndInit();
                            }

                            imagePicture.Source = bitmapImage;
                        }

                        else
                        {
                            // If no photo is available, clear the image control
                            imagePicture.Source = null;
                            MessageBox.Show("NULL");
                        }
                    }
                }

                else
                {
                    if (username == account.Username)
                    {
                        tbAccountID.Text = User.Account_ID;
                        tbAccountType.Text = User.AccountType;
                        tbUsername.Text = User.AccountUsername;
                        tbPassword.Text = User.AccountPassword;
                        tbFirstName.Text = User.FirstName;
                        tbLastName.Text = User.LastName;

                        if (account.Acc_Image != null)
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            using (MemoryStream stream = new MemoryStream(account.Acc_Image.ToArray()))
                            {
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = stream;
                                bitmapImage.EndInit();
                            }

                            imagePicture.Source = bitmapImage;
                        }

                        else
                        {
                            // If no photo is available, clear the image control
                            imagePicture.Source = null;
                            MessageBox.Show("NULL");
                        }
                    }
                }
            }
     
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var existingAccounts = Connections._slms.Accounts.FirstOrDefault(o => o.Acc_ID == tbAccountID.Text);

            if (existingAccounts != null)
            {
                tbPassword.Text = existingAccounts.Password;
                tbFirstName.Text = existingAccounts.First_Name;
                tbLastName.Text = existingAccounts.Last_Name;

                if (imagePicture.Source != null)
                {
                    byte[] imageData = ConvertImageToByteArray(imagePicture);
                    existingAccounts.Acc_Image = imageData;
                }
            }

            Connections._slms.Prod_UpdateAccount(tbAccountID.Text, tbAccountType.Text, tbUsername.Text, tbPassword.Text, tbFirstName.Text, tbLastName.Text, ConvertImageToByteArray(imagePicture));
            MessageBox.Show("Edited Successfully");
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

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage();
            AH.Show();
            this.Close();
        }

        private void btnTakeAPhoto_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Take A Photo");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var accountType = from a in Connections._slms.Accounts
                              where
                              a.Username == _username
                              select a;

            foreach (var login in accountType)
            {
                if (login != null)
                {
                    //User.Account_ID = login.Acc_ID;
                    //User.AccountType = login.Acc_Type;
                    //User.AccountUsername = login.Username;
                    //User.AccountPassword = login.Password;
                    //User.FirstName = login.First_Name;
                    //User.LastName = login.Last_Name;

                    login.Acc_ID = User.Account_ID;
                    login.Acc_Type = User.AccountType;
                    login.Username = User.AccountUsername;
                    login.Password = User.AccountPassword;
                    login.First_Name = User.FirstName;
                    login.Last_Name = User.LastName;


                    if (login.Acc_Image != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(login.Acc_Image.ToArray()))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                        }

                        imagePicture.Source = bitmapImage;
                    }

                    else
                    {
                        // If no photo is available, clear the image control
                        imagePicture.Source = null;
                    }
                }
            }
        }
    }
}
