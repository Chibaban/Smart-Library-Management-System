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

namespace Smart_Library_Management_System.Admin_Windows
{
    /// <summary>
    /// Interaction logic for GenerateQRCode.xaml
    /// </summary>
    public partial class GenerateQRCode : Window
    {
        public GenerateQRCode()
        {
            InitializeComponent();
        }

        private void btnGenerateQR_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {
                imgQR.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }
    }
}
