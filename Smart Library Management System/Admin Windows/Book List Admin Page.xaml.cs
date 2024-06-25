using Microsoft.Win32;
using Smart_Library_Management_System.Member_Windows;
using Smart_Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ZXing;

namespace Smart_Library_Management_System
{
    /// <summary>
    /// Interaction logic for Book_List_Admin_Page.xaml
    /// </summary>
    public partial class Book_List_Admin_Page : Window
    {
        List<BookList> books = new List<BookList>();
        List<string> bookTitles = new List<string>();
        private string acc_id = "";
        public Book_List_Admin_Page()
        {
            InitializeComponent();
            var BooksList = from books in Connections._slms.Books
                            select books.Title;

            lbBooksList.ItemsSource = BooksList.ToList();
        }
        public Book_List_Admin_Page(string acc_ID)
        {
            InitializeComponent();
            acc_id = acc_ID;
            var BooksList = from books in Connections._slms.Books
                            orderby books.Title
                            select books.Title;
            lbBooksList.ItemsSource = BooksList.ToList();

            var forBookClass = from books in Connections._slms.Books
                               select books;

            foreach (var book in forBookClass)
            {
                books.Add(new BookList
                {
                    bookID = book.Book_ID,
                    bookTitle = book.Title,
                    author = book.Author,
                    genre = book.Genre,
                    publishYear = (int)book.Publish_Year,
                    status = book.Status,
                    book_Image = book.Book_Image.ToArray(),
                    qr_Image = book.QR_Path.ToArray()
                });
            }

            foreach (BookList book in books)
            {
                bookTitles.Add(book.bookTitle);
            }

            lbBooksList.ItemsSource = bookTitles;
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Admin_Homepage AH = new Admin_Homepage(acc_id);
            AH.Show();
            this.Close();
        }
        private void btnBookImageUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {
                imagePicture.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }
        private void btnBookImageTakeAPhoto_Click(object sender, RoutedEventArgs e)
        {
            TakeAPhoto takeAPhoto = new TakeAPhoto();
            takeAPhoto.ShowDialog();

            if (TempImageStorer.image != null)
            {
                BitmapImage bmpImg = ConvertBitmapToBitmapImage(TempImageStorer.image);

                int cropWidth = 320; // Adjust as needed
                int cropHeight = 300; // Adjust as needed
                int cropX = (bmpImg.PixelWidth - cropWidth) / 2;
                int cropY = (bmpImg.PixelHeight - cropHeight) / 2;

                // Create a cropped version of the image
                CroppedBitmap croppedImage = new CroppedBitmap(
                    bmpImg,
                    new Int32Rect(cropX, cropY, cropWidth, cropHeight)
                );
                imagePicture.Source = croppedImage;
            }
        }
        private void lbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBooksList.SelectedIndex >= 0 && lbBooksList.SelectedIndex < lbBooksList.Items.Count)
            {
                var selectedBook = lbBooksList.SelectedItem.ToString();
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
                    else
                    {
                        imageQR.Source = null;
                    }
                }
            }
        }
        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            TempImageStorer.memStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = TempImageStorer.memStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var existingBooks = Connections._slms.Books.FirstOrDefault(o => o.Title == tbTitle.Text);

            if (existingBooks != null)
            {
                if (string.IsNullOrEmpty(tbTitle.Text) || string.IsNullOrEmpty(tbAuthor.Text) || string.IsNullOrEmpty(tbGenre.Text)
                    || string.IsNullOrEmpty(tbPublishDate.Text) || string.IsNullOrEmpty(tbStatus.Text)
                    || imagePicture.Source == null || imageQR.Source == null)
                {
                    MessageBox.Show("Please fill out all possible fields to fill.");
                }
                else
                {
                    existingBooks.Title = tbTitle.Text;
                    existingBooks.Author = tbAuthor.Text;
                    existingBooks.Genre = tbGenre.Text;

                    int publishYear = int.Parse(tbPublishDate.Text);
                    existingBooks.Publish_Year = (short?)publishYear;

                    existingBooks.Status = tbStatus.Text;

                    bool ifNoError = true;
                    byte[] imagePic = null;
                    byte[] imageQRPath = null;

                    if (ifNoError)
                    {
                        if (imagePicture.Source != null)
                        {
                            BitmapImage bitmapImage = imagePicture.Source as BitmapImage;
                            imagePic = BitmapImageToByteArray(bitmapImage);
                            existingBooks.Book_Image = imagePic;
                        }
                        else
                        {
                            MessageBox.Show("Book image must not be empty!");
                            ifNoError = false;
                        }
                        if (imageQR.Source != null)
                        {
                            BitmapSource bitmapImage = imageQR.Source as BitmapSource;
                            imageQRPath = BitmapImageToByteArray(bitmapImage);
                            existingBooks.QR_Path = imageQRPath;
                        }
                        else
                        {
                            MessageBox.Show("Generate a QR Code first!");
                            ifNoError = false;
                        }


                        Connections._slms.Prod_UpdateBookDetails(acc_id, tbBookID.Text, tbTitle.Text, tbAuthor.Text, tbGenre.Text, short.Parse(tbPublishDate.Text.ToString()), tbStatus.Text, imagePic, imageQRPath);
                        Connections._slms.SubmitChanges();
                        MessageBox.Show($"Updated {tbTitle.Text} successfully!");

                        foreach (BookList book in books)
                        {
                            if (book.bookTitle == tbTitle.Text)
                            {
                                book.bookID = tbBookID.Text;
                                book.bookTitle = tbTitle.Text;
                                book.author = tbAuthor.Text;
                                book.genre = tbGenre.Text;
                                book.status = tbStatus.Text;
                                book.book_Image = imagePic;
                                book.qr_Image = imageQRPath;
                            }
                        }
                        EmptyFields();
                    }
                    else
                    {
                        ResetInputObjects();
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tbTitle.Text) || string.IsNullOrEmpty(tbAuthor.Text) || string.IsNullOrEmpty(tbGenre.Text)
                    || string.IsNullOrEmpty(tbPublishDate.Text) || string.IsNullOrEmpty(tbStatus.Text)
                    || imagePicture.Source == null || imageQR.Source == null)
                {
                    MessageBox.Show("Please fill out all possible fields to fill.");
                }
                else
                {
                    //existingBooks.Title = tbTitle.Text;
                    //existingBooks.Author = tbAuthor.Text;
                    //existingBooks.Genre = tbGenre.Text;

                    //int publishYear = int.Parse(tbPublishDate.Text);
                    //existingBooks.Publish_Year = (short?)publishYear;

                    //existingBooks.Status = tbStatus.Text;

                    bool ifNoError = true;
                    byte[] imagePic = null;
                    byte[] imageQRPath = null;

                    if (ifNoError)
                    {
                        if (imagePicture.Source != null)
                        {
                            BitmapImage bitmapImage = imagePicture.Source as BitmapImage;
                            imagePic = BitmapImageToByteArray(bitmapImage);
                        }
                        else
                        {
                            MessageBox.Show("Book image must not be empty!");
                            ifNoError = false;
                        }
                        if (imageQR.Source != null)
                        {
                            BitmapSource bitmapImage = imageQR.Source as BitmapSource;
                            imageQRPath = BitmapImageToByteArray(bitmapImage);
                        }
                        else
                        {
                            MessageBox.Show("Generate a QR Code first!");
                            ifNoError = false;
                        }

                        foreach (string title in bookTitles)
                        {
                            if (tbTitle.Text == title)
                            {
                                MessageBox.Show("You can't have the same book title.");
                                tbTitle.Text = null;
                                ifNoError = false;
                                break;
                            }
                        }


                        Connections._slms.Prod_AddBook(acc_id, tbTitle.Text, tbAuthor.Text, tbGenre.Text, short.Parse(tbPublishDate.Text.ToString()), tbStatus.Text, imagePic, imageQRPath);
                        Connections._slms.SubmitChanges();
                        MessageBox.Show($"Added {tbTitle.Text} successfully!");

                        SLMSDataContext newLibContext = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);

                        //Checks for the new book added and adds it to the list
                        var Lookforbook = newLibContext.Books.First(b => b.Book_ID == $"BK{books.Count()+1}");
                        books.Add(new BookList
                        {
                            bookID = tbBookID.Text,
                            bookTitle = tbTitle.Text,
                            author = tbAuthor.Text,
                            genre = tbGenre.Text,
                            publishYear = int.Parse(tbPublishDate.Text),
                            status = tbStatus.Text,
                            book_Image = imagePic,
                            qr_Image = imageQRPath
                        });

                        //bookTitles.Clear();
                        //foreach (BookList book in books)
                        //{
                        //    bookTitles.Add(book.bookTitle);
                        //}

                        //Reload contents of existing window
                        this.Content = null;
                        Book_List_Admin_Page blap = new Book_List_Admin_Page(acc_id);
                        blap.Show();
                        this.Close();

                        //lbBooksList.ItemsSource = bookTitles;
                    }
                    else
                    {
                        ResetInputObjects();
                    }
                }
            }
        }
        private byte[] BitmapImageToByteArray(BitmapSource bitmapSource)
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
        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchEntry = tbSearchBar.Text;
            var existingBooks = Connections._slms.Books.ToList();
            lbBooksList.ItemsSource = null;
            lbBooksList.Items.Clear();

            if (searchEntry.Length > 0)
            {
                var filteredBooks = existingBooks.Where(b => b.Title.ToLower().Contains(searchEntry.ToLower())).ToList();

                foreach (var book in filteredBooks)
                {

                    lbBooksList.Items.Add(book.Title);
                }

            }
            else
            {
                foreach (var book in existingBooks)
                {
                    lbBooksList.Items.Add(book.Title);
                }
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lbBooksList.ItemsSource = null;
            lbBooksList.Items.Clear();

            var BooksData = from books in Connections._slms.Books
                            select books.Title;

            lbBooksList.ItemsSource = BooksData.ToList();
            EmptyFields();
        }
        private void btnGenerateQR_Click(object sender, RoutedEventArgs e)
        {

            if (tbTitle.Text != "")
            {
                string QRCodeText = tbTitle.Text;
                BarcodeWriter barcode = new BarcodeWriter();
                barcode.Format = BarcodeFormat.QR_CODE;
                barcode.Options.Height = 150;
                barcode.Options.Width = 160;

                Bitmap QRImage = barcode.Write(QRCodeText);
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                   QRImage.GetHbitmap(),
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions()
                   );

                imageQR.Source = bitmapSource;
            }
            else
            {
                MessageBox.Show("Make sure to have a book title first!");
            }
        }
        private void ResetInputObjects()
        {
            tbTitle.Text = string.Empty;
            tbTitle.Text = string.Empty;
            tbAuthor.Text = string.Empty;
            tbGenre.Text = string.Empty;
            tbPublishDate.Text = string.Empty;
            tbStatus.Text = string.Empty;
            imagePicture.Source = null;
            imageQR.Source = null;
        }
        private void btnUploadQR_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {
                imageQR.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }
        private void tbPublishDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPublishDate.Text.Length > 4)
            {
                MessageBox.Show("A year is only 4 characters long!");
                tbPublishDate.Text = string.Empty;
            }
            else
            {
                foreach (char c in tbPublishDate.Text)
                {
                    if (!char.IsDigit(c))
                    {
                        MessageBox.Show("You can't have a year with letters in it!");
                        tbPublishDate.Text = string.Empty;
                        break;
                    }
                }
            }
        }
        private void EmptyFields()
        {
            tbBookID.Text = string.Empty;
            tbTitle.Text = string.Empty;
            tbAuthor.Text = string.Empty;
            tbGenre.Text = string.Empty;
            tbPublishDate.Text = string.Empty;
            tbStatus.Text = string.Empty;
            imagePicture.Source = null;
            imageQR.Source = null;
            tbSearchBar.Text = string.Empty;
        }
    }
}
