using System;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using Smart_Library_Management_System.Models;

namespace Smart_Library_Management_System.Member_Windows
{
    /// <summary>
    /// Interaction logic for TakeAPhoto.xaml
    /// </summary>
    public partial class TakeAPhoto : Window
    {
        private IDisposable iD = null;
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
            TempImageStorer.memStream?.Dispose(); 
            TempImageStorer.memStream = new MemoryStream();

            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgScan.Source));
            encoder.Save(TempImageStorer.memStream);
            bmp = new Bitmap(TempImageStorer.memStream);

            TempImageStorer.image = bmp;

            vcd.SignalToStop();
            vcd.WaitForStop();
            vcd = null;

            MessageBox.Show("Image captured.");

            GC.WaitForPendingFinalizers();
            GC.Collect();



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
            if (vcd.IsRunning)
            {
                vcd.SignalToStop();
                vcd = null;

                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
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
