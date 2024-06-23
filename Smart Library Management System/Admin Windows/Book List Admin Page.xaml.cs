﻿using Microsoft.Win32;
using Smart_Library_Management_System.Admin_Windows;
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
    /// Interaction logic for Book_List_Admin_Page.xaml
    /// </summary>
    public partial class Book_List_Admin_Page : Window
    {
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
                            select books.Title;

            lbBooksList.ItemsSource = BooksList.ToList();
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
            MessageBox.Show("Take A Photo");
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

                    //imagePicture.Source = null;
                    //imageQR.Source = null;
                }
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var existingBooks = Connections._slms.Books.FirstOrDefault(o => o.Book_ID == tbBookID.Text);

            if (existingBooks != null)
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
                        imagePic = BitmapSourceToByteArray(bitmapImage);
                        existingBooks.Book_Image = imagePic;
                    }
                    else
                    {
                        ifNoError = false;
                    }

                    if (tbTitle.Text == existingBooks.Title)
                    {
                        MessageBox.Show("You can't have the same book title.");
                        tbTitle.Text = null;
                    }
                    else
                    {
                        ifNoError = false;
                    }
                    if(imageQR.Source != null)
                    {
                        BitmapImage bitmapImage = imageQR.Source as BitmapImage;
                        imageQRPath = BitmapSourceToByteArray(bitmapImage);
                        existingBooks.Book_Image = imageQRPath;
                    }
                    else
                    {
                        ifNoError = false;
                    }

                }

                Connections._slms.Prod_AddBook(acc_id, tbBookID.Text, tbTitle.Text, tbAuthor.Text, tbGenre.Text, short.Parse(tbPublishDate.Text.ToString()), tbStatus.Text, imagePic, imageQRPath);
                Connections._slms.SubmitChanges();
                MessageBox.Show("GUMANA");
            }

        }
        private byte[] ConvertImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
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
            var BooksData = from books in Connections._slms.Books
                            select books.Title;

            lbBooksList.ItemsSource = BooksData.ToList();
        }
        private void btnGenerateQR_Click(object sender, RoutedEventArgs e)
        {
            GenerateQRCode generateQRCode = new GenerateQRCode();
            generateQRCode.Show();
            this.Close();
        }
    } 
}