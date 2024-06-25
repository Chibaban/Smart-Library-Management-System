using Microsoft.Win32;
using Smart_Library_Management_System.Member_Windows;
using Smart_Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Borrow_Book_Page.xaml
    /// </summary>
    public partial class Borrow_Book_Page : Window
    {
        SLMSDataContext libContext = null;
        private string Acc_ID = "";
        private string decodedMessage = string.Empty;
        public Borrow_Book_Page()
        {
            InitializeComponent();

            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();
        }
        public Borrow_Book_Page(string acc_id)
        {
            InitializeComponent();
            Acc_ID = acc_id;
            var BooksList = from books in Connections._slms.Books
                            orderby books.Title
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();
            DisableFields();
        }
        public Borrow_Book_Page(byte[] convertedTitle, string acc_ID)
        {
            InitializeComponent();
            Acc_ID = acc_ID;
            decodedMessage = Encoding.UTF8.GetString(convertedTitle);

            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksList.ToList();
            foreach (string bookTitle in BooksList)
            {
                if (bookTitle == decodedMessage)
                {
                    lbBooks.SelectedItem = bookTitle;
                }
            }



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
            TakeAPhoto takeAPhoto = new TakeAPhoto();
            takeAPhoto.ShowDialog();

            if (TempImageStorer.image != null)
            {
                imagePicture.Source = ConvertBitmapToBitmapImage(TempImageStorer.image);
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (imagePicture.Source != null)
            {
                DateTime dt = DateTime.Now;
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

                BitmapImage bitImage = imagePicture.Source as BitmapImage;
                byte[] imageData = ConvertImageToByteArray(bitImage);

                Connections._slms.Prod_BorrowBook(book_id, User.Account_ID, imageData, dt);
                MessageBox.Show($"Borrowed book named {tbBookTitle.Text} successfully");
                Connections._slms.SubmitChanges();
            }
            else
            {
                MessageBox.Show("You must take a picture of the book!");
            }
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            User_Homepage UH = new User_Homepage(Acc_ID);
            UH.Show();
            this.Close();
        }
        private void lbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtons();
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
                    imagePicture.Source = null;

                    if(tbStatus.Text == "Borrowed")
                    {
                        DisableFields();
                    }
                } 
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var BooksData = from books in Connections._slms.Books
                            select books.Title;

            lbBooks.ItemsSource = BooksData.ToList();
        }
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbBooks.ItemsSource = null;

            string searchBook = tbSearchBar.Text;

            var query = from entry in Connections._slms.Books
                        where entry.Title.Contains(searchBook)
                        select entry;

            List<string> BookDescription = new List<string>();

            foreach (var entry in query)
            {
                BookDescription.Add(entry.Title);
            }

            lbBooks.ItemsSource = BookDescription;
        }
        private byte[] ConvertImageToByteArray(ImageSource imageSource)
        {
            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource == null)
            {
                throw new ArgumentException("ImageSource must be a BitmapSource");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        private byte[] BitmapSourceToByteArray(BitmapSource bitmapSource)
        {
            byte[] byteArray;
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                byteArray = ms.ToArray();
            }
            return byteArray;
        }
        public BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            TempImageStorer.memStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = TempImageStorer.memStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        private void DisableFields()
        {
            //Buttons
            btnUpload.IsEnabled = false;
            btnTakeAPhoto.IsEnabled = false;
            btnSubmit.IsEnabled = false;

            //TextBoxes
            tbBookTitle.IsEnabled = false;
            tbAuthor.IsEnabled = false;
            tbPublishDate.IsEnabled = false;
            tbGenre.IsEnabled = false;
            tbStatus.IsEnabled = false;
        }
        private void EnableButtons()
        {
            //Buttons
            btnUpload.IsEnabled = true;
            btnTakeAPhoto.IsEnabled = true;
            btnSubmit.IsEnabled = true;
        }
        private void btBookQRSearch_Click(object sender, RoutedEventArgs e)
        {
            FindBook fb = new FindBook(Acc_ID);
            fb.Show();
            this.Close();
        }
    }
}
