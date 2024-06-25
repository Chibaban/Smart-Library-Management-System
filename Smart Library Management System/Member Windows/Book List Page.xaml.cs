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
    /// Interaction logic for Book_List_Page.xaml
    /// </summary>
    public partial class Book_List_Page : Window
    {
        private string acc_ID = string.Empty;
        public Book_List_Page()
        {
            InitializeComponent();
            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();

            tbBookID.IsEnabled = false;
            tbTitle.IsEnabled = false;
            tbAuthor.IsEnabled = false;
            tbGenre.IsEnabled = false;
            tbPublishDate.IsEnabled = false;
            tbStatus.IsEnabled = false;
        }
        public Book_List_Page(string acc_id)
        {
            InitializeComponent();
            acc_ID = acc_id;
            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();

            tbBookID.IsEnabled = false;
            tbTitle.IsEnabled = false;
            tbAuthor.IsEnabled = false;
            tbGenre.IsEnabled = false;
            tbPublishDate.IsEnabled = false;
            tbStatus.IsEnabled = false;
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            User_Homepage UH = new User_Homepage(acc_ID);
            UH.Show();
            this.Close();
        }
        private void lbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBooks.SelectedIndex >= 0 && lbBooks.SelectedIndex < lbBooks.Items.Count)
            {
                var selectedBook = lbBooks.SelectedItem.ToString();
                var BookInfo = Connections._slms.Books.FirstOrDefault(o => o.Title == selectedBook);
                if (BookInfo != null)
                {
                    tbBookID.Text = BookInfo.Book_ID;
                    tbTitle.Text = BookInfo.Title;
                    tbAuthor.Text = BookInfo.Author;
                    tbGenre.Text = BookInfo.Genre;
                    tbPublishDate.Text = BookInfo.Publish_Year.ToString();
                    tbStatus.Text = BookInfo.Status;

                    if (BookInfo.Book_Image != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(BookInfo.Book_Image.ToArray()))
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
                    if (BookInfo.QR_Path != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(BookInfo.QR_Path.ToArray()))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                        }

                        imageQR.Source = bitmapImage;
                    }
                    //imagePicture.Source = null;
                    //imageQR.Source = null;
                }
            }
        }
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchEntry = tbSearchBar.Text;
            var existingBooks = Connections._slms.Books.ToList();
            lbBooks.ItemsSource = null;
            lbBooks.Items.Clear();

            if (searchEntry.Length > 0)
            {
                var filteredBooks = existingBooks.Where(b => b.Title.ToLower().Contains(searchEntry.ToLower())).ToList();

                foreach (var book in filteredBooks)
                {

                    lbBooks.Items.Add(book.Title);
                }

            }
            else
            {
                foreach (var book in existingBooks)
                {
                    lbBooks.Items.Add(book.Title);
                }
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var BooksData = from books in Connections._slms.Books
                              select books.Title;

            lbBooks.ItemsSource = BooksData.ToList();
        }
    }
}
