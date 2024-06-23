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
using static System.Net.Mime.MediaTypeNames;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Logs_Page.xaml
    /// </summary>
    public partial class Logs_Page : Window
    {
        private string accId = "";
        public Logs_Page()
        {
            InitializeComponent();
            var LogList = from Logs in Connections._slms.Logs
                            select Logs.Log_ID;
            lbLogs.ItemsSource = LogList;
        }
        public Logs_Page(string acc_ID)
        {
            InitializeComponent();
            var LogList = from Logs in Connections._slms.Logs
                          select Logs.Log_ID;
            lbLogs.ItemsSource = LogList;
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage(accId);
            AH.Show();
            this.Close();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLogs.SelectedIndex >= 0 && lbLogs.SelectedIndex < lbLogs.Items.Count)
            {
                var selectedLog = lbLogs.SelectedItem.ToString();
                var logInfo = Connections._slms.Logs.FirstOrDefault(o => o.Log_ID  == selectedLog);
                if (selectedLog != null)
                {
                    tbLogID.Text = logInfo.Log_ID;
                    tbAccountID.Text = logInfo.Acc_ID;
                    tbTimeStamp.Text = logInfo.TimeStamp.ToString();
                    tbLogActivity.Text = logInfo.Log_Activity;
                }
            }
        }
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchEntry = tbSearchBar.Text;
            var existingLogs = Connections._slms.Logs.ToList();
            lbLogs.ItemsSource = null;
            lbLogs.Items.Clear();

            if (searchEntry.Length > 0)
            {
                var filteredLogs = existingLogs.Where(b => b.Log_ID.ToLower().Contains(searchEntry.ToLower())).ToList();

                foreach (var logs in filteredLogs)
                {

                    lbLogs.Items.Add(logs.Log_ID);
                }

            }
            else
            {
                foreach (var logs in existingLogs)
                {
                    lbLogs.Items.Add(logs.Log_ID);
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e) 
        {
            var BooksData = from books in Connections._slms.Logs
                            select books.Log_ID;

            lbLogs.ItemsSource = BooksData.ToList();
        }
    }
}
