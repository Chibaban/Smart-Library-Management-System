using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Smart_Library_Management_System
{
    public static class User
    {
        public static string Account_ID {get; set;}
        public static string AccountType { get; set;}

        public static string AccountUsername { get; set;}
        public  static string AccountPassword { get; set;}
        public static string FirstName { get; set;}
        public static string LastName { get; set;} 
        public static BitmapImage UserProfilePic { get; set;}
    }
}
