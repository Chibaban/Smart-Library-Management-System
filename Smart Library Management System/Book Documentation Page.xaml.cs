using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Book_Documentation_Page.xaml
    /// </summary>
    public partial class Book_Documentation_Page : Window
    {
        SLMSDataContext _SLMS = null;

        public Book_Documentation_Page()
        {
            InitializeComponent();
            _SLMS = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);

            var BooksList = from books in _SLMS.Book_Documentations
                               select books.Doc_ID;

            lbBooks.ItemsSource = BooksList.ToList();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage();
            AH.Show();
            this.Close();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbBooks.ItemsSource = null;

            string searchAccount = tbSearchBar.Text;

            var query = from entry in _SLMS.Book_Documentations
                        where entry.Book_ID.Contains(searchAccount)
                        select entry;

            List<string> AccountDescription = new List<string>();

            foreach (var entry in query)
            {
                AccountDescription.Add(entry.Book_ID);
            }

            lbBooks.ItemsSource = AccountDescription;
        }

        private void lbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBooks.SelectedIndex >= 0 && lbBooks.SelectedIndex < lbBooks.Items.Count)
            {
                var selectedBook = lbBooks.SelectedItem.ToString();
                var BookInfo = _SLMS.Book_Documentations.FirstOrDefault(o => o.Doc_ID == selectedBook);
                if (BookInfo != null)
                {
                    tbDocumentationID.Text = BookInfo.Doc_ID;
                    tbBookID.Text = BookInfo.Book_ID;
                    tbAccountID.Text = BookInfo.Acc_ID;
                    tbBorrowDate.Text = BookInfo.Borrow_Date.ToString();
                    tbReturnDate.Text = BookInfo.Return_Date.ToString();

                    if (BookInfo.Borrow_Image != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(BookInfo.Borrow_Image.ToArray()))
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

                    if (BookInfo.Return_Image != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(BookInfo.Return_Image.ToArray()))
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
    }
}
