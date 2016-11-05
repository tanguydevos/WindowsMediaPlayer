using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Controls;

namespace WindowsMediaPlayer.ViewModel
{
    class PictureViewModel : ViewModel
    {
        private Image image = new Image();
        private Model.PictureClip _pictureClip = null;

        public override void open(string fileName)
        {
            _pictureClip = new Model.PictureClip(fileName);
            image.Source = new BitmapImage(new Uri(@fileName));
        }

        public override void play()
        {
            if (!isPlaying && _pictureClip != null)
            {
                isPlaying = true;
            }
        }

        public override UIElement getMediaPlayer()
        {
            return image;
        }

        public override void pause()
        {
            if (isPlaying && _pictureClip != null)
            {
                isPlaying = false;
            }
        }

        public override void stop()
        {
            if (isPlaying && _pictureClip != null)
            {
                isPlaying = false;
            }
        }

        public override double getCurrentTime()
        {
            return 0;
        }

        public override double getMaximumTime()
        {
            return 1;
        }

        public override void setPositionPlayer(System.TimeSpan value)
        {
        }

    }
}
