using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Library_Management_System.Models
{
    internal class AccountList
    {
        public string acc_id {  get; set; }
        public string acc_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public byte[] acc_image { get; set; }
    }
}
