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
    abstract class ViewModel
    {
        protected bool isPlaying = false;
        
        public abstract void play();
        public abstract void pause();
        public abstract void stop();
        public abstract void open(string fileName);
        public abstract double getCurrentTime();
        public abstract double getMaximumTime();
        public abstract void setPositionPlayer(System.TimeSpan value);
        public abstract UIElement getMediaPlayer();
        public virtual void changeVolume(double value) {}
    }
}
