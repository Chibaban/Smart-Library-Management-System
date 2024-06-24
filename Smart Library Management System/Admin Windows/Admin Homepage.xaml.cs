﻿using System;
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
            Logs_Page LogsP = new Logs_Page(_acc_ID);
            LogsP.Show();
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
