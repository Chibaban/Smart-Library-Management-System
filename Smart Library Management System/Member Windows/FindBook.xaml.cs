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
using AForge;
using AForge.Video;
using AForge.Imaging;
using AForge.Video.DirectShow;
using System.Drawing;
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Threading;
using System.IO;

namespace Smart_Library_Management_System.Member_Windows
{
    /// <summary>
    /// Interaction logic for FindBook.xaml
    /// </summary>
    public partial class FindBook : Window
    {
        private string acc_Id = string.Empty;
        private byte[] _scannedBook = null;
        FilterInfoCollection fic = null;
        VideoCaptureDevice vcd = null;
        DispatcherTimer timer1 = null;
        public FindBook()
        {
            InitializeComponent();
        }

        public FindBook(string acc_ID)
        {
            InitializeComponent();
        }


        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            StartOrChangeCamera();
            vcd = new VideoCaptureDevice();
            timer1 = new DispatcherTimer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer1.Start();
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vcd.IsRunning)
            {
                StopCameraWorking();
            }
        }

        private void StartOrChangeCamera()
        {
            vcd = new VideoCaptureDevice(fic[cmbCamList.SelectedIndex].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();
        }

        private void cmbCamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vcd = new VideoCaptureDevice(fic[cmbCamList.SelectedIndex].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader Reader = new BarcodeReader();
            Reader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };
            Reader.Options.TryHarder = true; 

            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgScan.Source));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }

            Result result = Reader.Decode(bitmap);
            if (result != null)
            {
                string decoded = result.Text.Trim();
                if (!string.IsNullOrEmpty(decoded))
                {
                    StopCameraWorking();
                    MessageBox.Show(decoded);
                }
            }
            else
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        private void StopCameraWorking()
        {
            vcd.SignalToStop();
            vcd = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
