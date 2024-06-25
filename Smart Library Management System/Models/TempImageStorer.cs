using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Smart_Library_Management_System.Models
{
    public static class TempImageStorer
    {
        public static Bitmap image { get; set; }
        public static MemoryStream memStream { get; set; }
    }
}
