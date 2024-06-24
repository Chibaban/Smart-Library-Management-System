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
            acc_Id = acc_ID;
        }


        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            vcd = new VideoCaptureDevice(fic[cmbCamList.SelectedIndex].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();

            timer1 = new DispatcherTimer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = new TimeSpan(0, 0, 0, 1, 0);
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
            vcd = new VideoCaptureDevice();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vcd.IsRunning)
            {
                StopCameraWorking();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            SLMSDataContext sLMSDataContext = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);
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
                MessageBox.Show(decoded);
                if (!string.IsNullOrEmpty(decoded))
                {
                    var lookTitle = from title in sLMSDataContext.Books
                                    where title.Title == decoded    
                                    select title;
                    foreach (var book in lookTitle)
                    {
                        if (book.Title == decoded)
                        {
                            byte[] convDecoded = Encoding.UTF8.GetBytes(decoded);
                            Borrow_Book_Page brp = new Borrow_Book_Page(convDecoded, acc_Id);
                            brp.Show();
                        }
                        else
                        {
                            MessageBox.Show("Book not found in the database.");
                            Borrow_Book_Page brp = new Borrow_Book_Page(acc_Id);
                            brp.Show();
                        }
                        break;
                    }
                    this.Close();
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
