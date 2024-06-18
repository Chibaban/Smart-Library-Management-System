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
        public User_Homepage()
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
            MessageBox.Show("Profile");
        }

        private void btBorrowBook_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Borrow Book");
        }

        private void btBookDocumentation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Book Documentation");
        }

        private void btBookList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Book List");
        }
    }
}
