using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Library_Management_System
{
    public static class Connections
    {
        public static SLMSDataContext _slms = new SLMSDataContext(Properties.Settings.Default.LibWonderConnectionString);
    }
}
