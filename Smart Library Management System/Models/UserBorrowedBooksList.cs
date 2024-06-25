using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Library_Management_System.Models
{
    public class UserBorrowedBooksList
    {
        public string bID { get; set; }
        public string bTitle { get; set; }
        public string bAuthor { get; set; }
        public string bGenre { get; set; }
        public int bPublishYear { get; set; }
        public string bStatus { get; set; }
        public byte[] bBook_Image { get; set; }
        public byte[] bQr_Image { get; set; }
    }
}
