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

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Admin_User_Profile_Page.xaml
    /// </summary>
    public partial class Admin_User_Profile_Page : Window
    {
        public Admin_User_Profile_Page()
        {
            InitializeComponent();
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
                        imagePicture.Source = ;
                    }
                }
            }
     
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edited Successfully");
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
    }
}
