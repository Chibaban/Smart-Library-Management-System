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
        private string _AccId = "";

        public Admin_User_Profile_Page()
        {
            InitializeComponent();
        }

        public Admin_User_Profile_Page(string Acc_ID)
        {
            InitializeComponent();
            _AccId = Acc_ID;

            var accountType = from a in Connections._slms.Accounts
                              where
                              a.Acc_ID == Acc_ID
                              select a;

            foreach (var account in accountType)
            {
                if (account.Acc_Type == "Admin")
                {
                    if (_AccId == account.Acc_ID)
                    {
                        tbAccountID.Text = account.Acc_ID;
                        tbAccountType.Text = account.Acc_Type;
                        tbUsername.Text = account.Username;
                        tbPassword.Text = account.Password;
                        tbFirstName.Text = account.First_Name;
                        tbLastName.Text = account.Last_Name;

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
                            MessageBox.Show("No image was found");
                        }
                    }
                }

                else
                {
                    if (_AccId == account.Acc_ID)
                    {
                        tbAccountID.Text = account.Acc_ID;
                        tbAccountType.Text = account.Acc_Type;
                        tbUsername.Text = account.Username;
                        tbPassword.Text = account.Password;
                        tbFirstName.Text = account.First_Name;
                        tbLastName.Text = account.Last_Name;

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
                            MessageBox.Show("No image was found");
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
                    byte[] imageData = ConvertImageToByteArray(imagePicture.Source);
                    existingAccounts.Acc_Image = imageData;
                }
            }

            Connections._slms.Prod_UpdateAccount(tbAccountID.Text, tbAccountType.Text, tbUsername.Text, tbPassword.Text, tbFirstName.Text, tbLastName.Text, ConvertImageToByteArray(imagePicture.Source));
            MessageBox.Show("Edited Successfully");
        }

        private byte[] ConvertImageToByteArray(ImageSource imageSource)
        {
            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource == null)
            {
                throw new ArgumentException("ImageSource must be a BitmapSource");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                return ms.ToArray();
            }
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
            var accountType = from a in Connections._slms.Accounts
                              where
                              a.Acc_ID == _AccId
                              select a;
            foreach (var account in accountType)
            {
                if (account.Acc_ID == _AccId)
                {
                    if (account.Acc_Type == "Admin")
                    {
                        Admin_Homepage AH = new Admin_Homepage(_AccId);
                        AH.Show();
                        this.Close();
                    }
                    else
                    {
                        User_Homepage UH = new User_Homepage(_AccId);
                        UH.Show();
                        this.Close();
                    }
                }
            }


        }
        private void btnTakeAPhoto_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Take A Photo");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var accountType = from a in Connections._slms.Accounts
                              where
                              a.Acc_ID == _AccId
                              select a;

            foreach (var login in accountType)
            {
                if (login != null)
                {
                    if (login.Acc_ID == _AccId)
                    {
                        login.Acc_ID = tbAccountID.Text;
                        login.Acc_Type = tbAccountType.Text;
                        login.Username = tbUsername.Text;
                        login.Password = tbPassword.Text;
                        login.First_Name = tbFirstName.Text;
                        login.Last_Name = tbLastName.Text;

                        User.Account_ID = tbAccountID.Text;
                        User.AccountType = tbAccountType.Text;
                        User.AccountUsername = tbUsername.Text;
                        User.AccountPassword = tbPassword.Text;
                        User.FirstName = tbFirstName.Text;
                        User.LastName = tbLastName.Text;

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
                            login.Acc_Image = BitmapSourceToByteArray(bitmapImage);
                            User.UserProfilePic = bitmapImage;
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
}
