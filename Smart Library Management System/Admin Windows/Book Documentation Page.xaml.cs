using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Book_Documentation_Page.xaml
    /// </summary>
    public partial class Book_Documentation_Page : Window
    {
        private string acc_Id = "";
        private string selectedBookID = "";
        public Book_Documentation_Page()
        {
            InitializeComponent();
            var BooksList = from books in Connections._slms.Book_Documentations
                               select books.Doc_ID;

            lbBooks.ItemsSource = BooksList.ToList();
        }
        public Book_Documentation_Page(string acc_ID)
        {
            InitializeComponent();
            acc_Id = acc_ID;
            var BooksList = from books in Connections._slms.Book_Documentations
                            orderby books.Doc_ID
                            select books.Doc_ID;

            lbBooks.ItemsSource = BooksList.ToList();
            btnChangeStatus.IsEnabled = false;
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage(acc_Id);
            AH.Show();
            this.Close();
        }
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbBooks.ItemsSource = null;

            string searchBook = tbSearchBar.Text;

            var query = from entry in Connections._slms.Book_Documentations
                        where entry.Book_ID.Contains(searchBook)
                        select entry;

            List<string> BookDescription = new List<string>();

            foreach (var entry in query)
            {
                BookDescription.Add(entry.Book_ID);
            }

            lbBooks.ItemsSource = BookDescription;
        }
        private void lbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnChangeStatus.IsEnabled = false;
            if (lbBooks.SelectedIndex >= 0 && lbBooks.SelectedIndex < lbBooks.Items.Count)
            {
                string selectedBook = lbBooks.SelectedItem.ToString();
                var BookInfo = Connections._slms.Book_Documentations.FirstOrDefault(o => o.Doc_ID == selectedBook);
                if (BookInfo != null)
                {
                    lbDocumentationID.Content = BookInfo.Doc_ID;
                    lbBookID.Content = BookInfo.Book_ID;
                    selectedBookID = BookInfo.Book_ID;
                    lbAccountID.Content = BookInfo.Acc_ID;

                    DateTime borrowDate = new DateTime();
                    borrowDate = (DateTime)BookInfo.Borrow_Date;
                    lbBorrowDate.Content = borrowDate.ToShortDateString();

                    if(BookInfo.Return_Date != null)
                    {
                        DateTime returnDate = new DateTime();
                        returnDate = (DateTime)BookInfo.Return_Date;
                        lbReturnDate.Content = borrowDate.ToShortDateString();
                        btnChangeStatus.IsEnabled = true;
                    }
                    else
                    {
                        lbReturnDate.Content = string.Empty;
                    }

                    if (BookInfo.Borrow_Image != null)
                    {
                        byte[] borrowImage = BookInfo.Borrow_Image.ToArray();
                        
                        using (MemoryStream stream = new MemoryStream(borrowImage))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            imageBorrow.Source = bitmapImage;
                        }
                    }
                    else
                    {
                        // If no photo is available, clear the image control
                        imageBorrow.Source = null;
                    }

                    if (BookInfo.Return_Image != null)
                    {
                        byte[] returnImage = BookInfo.Return_Image.ToArray();
                       
                        using (MemoryStream stream = new MemoryStream(returnImage))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            imageReturn.Source = bitmapImage;
                        }
                    }
                    else
                    {
                        imageReturn.Source = null;
                    }
                }
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var BookDocumentationData = from books in Connections._slms.Book_Documentations
                                        orderby books.Doc_ID
                              select books.Doc_ID;
            tbSearchBar.Text = string.Empty;
            lbBooks.ItemsSource = BookDocumentationData.ToList();
        }
        private void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            SLMSDataContext dataContext = new SLMSDataContext();
            dataContext.Prod_UpdateBookStatus(selectedBookID);
            dataContext.SubmitChanges();

            var query = from theBook in dataContext.Books
                        select theBook;
            foreach (var book in query) 
            { 
                if(book.Book_ID == selectedBookID)
                {
                    MessageBox.Show($"Book entitled {book.Title} has been made available!");
                    break;
                }
            }
        }
    }
}
