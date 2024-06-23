using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Account_List.xaml
    /// </summary>
    public partial class Account_List : Window
    {
        private string acc_id = string.Empty;
        public Account_List()
        {
            InitializeComponent();

            var AccountsList = from accounts in Connections._slms.Accounts
                               select accounts.Username;

            lbAccounts.ItemsSource = AccountsList.ToList();
        }
        public Account_List(string acc_ID)
        {
            InitializeComponent();
            acc_id = acc_ID;

            var AccountsList = from accounts in Connections._slms.Accounts
                               select accounts.Username;

            lbAccounts.ItemsSource = AccountsList.ToList();
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage();
            AH.Show();
            this.Close();
        }
        private void lbAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAccounts.SelectedIndex >= 0 && lbAccounts.SelectedIndex < lbAccounts.Items.Count)
            {
                var selectedAccount = lbAccounts.SelectedItem.ToString();
                var AccountInfo = Connections._slms.Accounts.FirstOrDefault(o => o.Username == selectedAccount);
                if (AccountInfo != null)
                {
                    tbAccountID.Text = AccountInfo.Acc_ID;
                    tbAccountType.Text = AccountInfo.Acc_Type;
                    tbUsername.Text = AccountInfo.Username;
                    tbPassword.Text = AccountInfo.Password;
                    tbFirstName.Text = AccountInfo.First_Name;
                    tbLastName.Text = AccountInfo.Last_Name;

                    if (AccountInfo.Acc_Image != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(AccountInfo.Acc_Image.ToArray()))
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
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbAccounts.ItemsSource = null;

            string searchAccount = tbSearchBar.Text;

            var query = from entry in Connections._slms.Accounts
                        where entry.Username.Contains(searchAccount)
                        select entry;

            List<string> AccountDescription = new List<string>();

            foreach ( var entry in query ) 
            {
                AccountDescription.Add(entry.Username);
            }

            lbAccounts.ItemsSource = AccountDescription;
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var AccountData = from accounts in Connections._slms.Accounts
                              select accounts.Username;

            lbAccounts.ItemsSource = AccountData.ToList();
        }
    }
}
