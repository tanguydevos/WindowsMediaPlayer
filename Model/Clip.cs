using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMediaPlayer.Model
{
    public abstract class Clip
    {
        protected string path;
        protected double resolution = 0;
        protected string type;
        protected string title = "Unknown";
        protected TimeSpan duration = TimeSpan.Zero;

        public string Path
        {
            get
            {
                return (path);
            }

            set
            {
                path = value;
            }
        }

        public string Title
        {
            get
            {
                return (title);
            }

            set
            {
                title = value;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return (duration);
            }

            set
            {
                duration = value;
            }
        }

        public string Type
        {
            get
            {
                return (type);
            }

            set
            {
                type = value;
            }
        }
    }
}
