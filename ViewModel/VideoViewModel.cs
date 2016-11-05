using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;

namespace WindowsMediaPlayer.ViewModel
{
    class VideoViewModel : ViewModel
    {
        private MediaElement mediaPlayer = new MediaElement();
        private Model.VideoClip _videoClip = null;

        public override void open(string fileName)
        {
            _videoClip = new Model.VideoClip(fileName);
            mediaPlayer.LoadedBehavior = MediaState.Manual;
            mediaPlayer.Source = new Uri(fileName);
        }

        public override void play()
        {
            if (!isPlaying && _videoClip != null)
            {
                mediaPlayer.Play();
                isPlaying = true;
            }
        }

        public override UIElement getMediaPlayer()
        {
            return mediaPlayer;
        }

        public override void pause()
        {
            if (isPlaying && _videoClip != null)
            {
                mediaPlayer.Pause();
                isPlaying = false;
            }
        }

        public override void stop()
        {
            if (isPlaying && _videoClip != null)
            {
                mediaPlayer.Stop();
                isPlaying = false;
            }
        }

        public override double getCurrentTime()
        {
            return mediaPlayer.Position.TotalSeconds;
        }

        public override double getMaximumTime()
        {
            return mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        public override void setPositionPlayer(System.TimeSpan value)
        {
            mediaPlayer.Position = value;
        }

        public override void changeVolume(double value)
        {
            mediaPlayer.Volume = value;
        }
    }
}
