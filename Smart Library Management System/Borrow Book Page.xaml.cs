using Microsoft.Win32;
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
    /// Interaction logic for Borrow_Book_Page.xaml
    /// </summary>
    public partial class Borrow_Book_Page : Window
    {
        public Borrow_Book_Page()
        {
            InitializeComponent();

            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {
                imagePicture.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }

        private void btnTakeAPhoto_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Take A Photo");
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string book_id = string.Empty;
            var getBooks = from books in Connections._slms.Books
                            select books;

            foreach (var title in getBooks)
            {
                if (title.Title == tbBookTitle.Text)
                {
                    book_id = title.Book_ID;
                }
            }

            Connections._slms.p
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            User_Homepage UH = new User_Homepage();
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
                    tbBookTitle.Text = BookInfo.Title;
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
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var BooksData = from books in _SLMS.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksData.ToList();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbBooks.ItemsSource = null;

            string searchBook = tbSearchBar.Text;

            var query = from entry in _SLMS.Books
                        where entry.Title.Contains(searchBook)
                        select entry;

            List<string> BookDescription = new List<string>();

            foreach (var entry in query)
            {
                BookDescription.Add(entry.Title);
            }

            lbBooks.ItemsSource = BookDescription;
        }
    }
}
