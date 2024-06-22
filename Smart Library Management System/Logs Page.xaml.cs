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
    /// Interaction logic for Logs_Page.xaml
    /// </summary>
    public partial class Logs_Page : Window
    {
        SLMSDataContext _SLMS = null;
        public Logs_Page()
        {
            InitializeComponent();
            _SLMS = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);

            var Logs = from logs in _SLMS.Logs
                            select logs.Log_ID;

            lbLogs.ItemsSource = Logs.ToList();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage();
            AH.Show();
            this.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLogs.SelectedIndex >= 0 && lbLogs.SelectedIndex < lbLogs.Items.Count)
            {
                var selectedLog = lbLogs.SelectedItem.ToString();
                var LogInfo = _SLMS.Logs.FirstOrDefault(o => o.Log_ID == selectedLog);
                if (LogInfo != null)
                {
                    tbLogID.Text = LogInfo.Log_ID;
                    tbAccountID.Text = LogInfo.Acc_ID;
                    tbTimeStamp.Text = LogInfo.TimeStamp.ToString();
                    tbLogActivity.Text = LogInfo.Log_Activity;
                }
            }
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbLogs.ItemsSource = null;

            string searchLogs = tbSearchBar.Text;

            var query = from entry in _SLMS.Logs
                        where entry.Log_ID.Contains(searchLogs)
                        select entry;

            List<string> AccountDescription = new List<string>();

            foreach (var entry in query)
            {
                AccountDescription.Add(entry.Log_ID);
            }

            lbLogs.ItemsSource = AccountDescription;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var LogsData = from books in _SLMS.Logs
                            select books.Log_ID;

            lbLogs.ItemsSource = LogsData.ToList();
        }
    }
}
