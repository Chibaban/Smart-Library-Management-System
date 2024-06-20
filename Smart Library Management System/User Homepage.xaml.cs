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
    /// Interaction logic for User_Homepage.xaml
    /// </summary>
    public partial class User_Homepage : Window
    {
        string _Acc_Type = string.Empty;

        public User_Homepage()
        {
            InitializeComponent();
        }

        public User_Homepage(string AccType)
        {
            InitializeComponent();
            _Acc_Type = AccType;
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LP = new MainWindow();
            LP.Show();
            this.Close();
        }

        private void btProfile_Click(object sender, RoutedEventArgs e)
        {
            var query = from l in Connections._slms.Accounts
                        where l.Acc_Type == _Acc_Type
                        select l;

            string name = String.Empty;
            foreach (var username in query)
            {
                name = username.Username;
            }

            Admin_User_Profile_Page AUPP = new Admin_User_Profile_Page(name);
            AUPP.Show();
            this.Close();
        }

        private void btBorrowBook_Click(object sender, RoutedEventArgs e)
        {
            Borrow_Book_Page BBP = new Borrow_Book_Page();
            BBP.Show();
            this.Close();
        }

        private void btBookList_Click(object sender, RoutedEventArgs e)
        {
            Book_List_Page BLB = new Book_List_Page();
            BLB.Show();
            this.Close();
        }

        private void btReturnBook_Click(object sender, RoutedEventArgs e)
        {
            Return_Book_Page RBP = new Return_Book_Page();
            RBP.Show();
            this.Close();
        }
    }
}
