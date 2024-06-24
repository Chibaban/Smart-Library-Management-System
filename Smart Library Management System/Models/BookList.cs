using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Smart_Library_Management_System.Models
{
    public class BookList
    {
        public string bookID { get; set; }
        public string bookTitle { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int publishYear { get; set; }
        public string status { get; set; }
        public byte[] book_Image { get; set; }
        public byte[] qr_Image { get; set; }
    }
}
