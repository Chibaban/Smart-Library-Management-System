using System;
using System.Windows;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Admin_Homepage.xaml
    /// </summary>
    public partial class Admin_Homepage : Window
    {
        string _acc_ID = String.Empty;
        public Admin_Homepage()
        {
            InitializeComponent();
        }
        public Admin_Homepage(string accId)
        {
            InitializeComponent();
            _acc_ID = accId;
        }
        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LP = new MainWindow();
            LP.Show();
            this.Close();
        }
        private void btProfile_Click(object sender, RoutedEventArgs e)
        {
            //var query = from l in  Connections._slms.Accounts
            //            where l.Acc_ID == _acc_ID
            //            select l;

            //string name = String.Empty;
            //foreach (var username in query)
            //{
            //    name = username.Username;
            //}
            Admin_User_Profile_Page AUPP = new Admin_User_Profile_Page(_acc_ID);
            AUPP.Show();
            this.Close();
        }
        private void btBookList_Click(object sender, RoutedEventArgs e)
        {
            Book_List_Admin_Page BLAP = new Book_List_Admin_Page(_acc_ID);
            BLAP.Show();
            this.Close();
        }
        private void btBookDocumentation_Click(object sender, RoutedEventArgs e)
        {
            Book_Documentation_Page BDP = new Book_Documentation_Page(_acc_ID);
            BDP.Show();
            this.Close();
        }
        private void btLogs_Click(object sender, RoutedEventArgs e)
        {
            Logs_Page logs_Page = new Logs_Page(_acc_ID);
            logs_Page.Show();
            this.Close();
        }
        private void btAccountList_Click(object sender, RoutedEventArgs e)
        {
            Account_List ALP = new Account_List(_acc_ID);
            ALP.Show();
            this.Close();
        }
    }
}
