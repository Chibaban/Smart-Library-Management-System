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

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Admin_Homepage.xaml
    /// </summary>
    public partial class Admin_Homepage : Window
    {
        public Admin_Homepage()
        {
            InitializeComponent();
        }
        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LP = new MainWindow();
            LP.Show();
            this.Close();
        }

        private void btProfile_Click(object sender, RoutedEventArgs e)
        {
            Admin_User_Profile_Page AUPP = new Admin_User_Profile_Page();
            AUPP.Show();
            this.Close();
        }

        private void btBookList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Book List");
        }

        private void btBookDocumentation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Book Documentation");
        }

        private void btLogs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logs");
        }

        private void btAccountList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Accounts");
        }
    }
}
