﻿using System;
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
using AForge;
using AForge.Video;
using AForge.Imaging;
using AForge.Video.DirectShow;
using System.Drawing;
using ZXing;
using System.Windows.Threading;
using System.IO;

namespace Smart_Library_Management_System.Member_Windows
{
    /// <summary>
    /// Interaction logic for TakeAPhoto.xaml
    /// </summary>
    public partial class TakeAPhoto : Window
    {
        private string acc_Id = string.Empty;
        FilterInfoCollection fic = null;
        VideoCaptureDevice vcd = null;
        public TakeAPhoto()
        {
            InitializeComponent();
        }
        public TakeAPhoto(string acc_ID/*, Borrow_Book_Page window*/)
        {
            InitializeComponent();
            acc_Id = acc_ID;
            //this.bbpage = window;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            vcd = new VideoCaptureDevice(fic[cmbCamList.SelectedIndex].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();
        }
        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bmp = null;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgScan.Source));
                encoder.Save(stream);
                bmp = new Bitmap(stream);
            }

            MessageBox.Show("You successfully took a photo of the book!");
            Borrow_Book_Page borrow_Book_Page = new Borrow_Book_Page(acc_Id, bmp);
            borrow_Book_Page.Show();
            this.Close();
        }
        private void Vcd_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            this.Dispatcher.Invoke(() =>
            {
                imgScan.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ((Bitmap)eventArgs.Frame.Clone()).GetHbitmap(),
                        IntPtr.Zero,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight((int)imgScan.Width, (int)imgScan.Height));
            });
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Borrow_Book_Page b = new Borrow_Book_Page(acc_Id);
            b.Show();
            this.Close();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            fic = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in fic)
                cmbCamList.Items.Add(fi.Name);
            cmbCamList.SelectedIndex = 0;
            vcd = new VideoCaptureDevice();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vcd.IsRunning)
            {
                StopCameraWorking();
            }
        }
        private void StopCameraWorking()
        {
            vcd.SignalToStop();
            vcd = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
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

    }
}
