using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsMediaPlayer
{
    public static class Helper
    {
        public delegate void DoubleDelegate(double value);
        public delegate void VoidDelegate();
        public static string xmlFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowsMediaPlayer");
    }
}
