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
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WindowsMediaPlayer.ViewModel
{

    class ManagerViewModel
    {
        private ViewModel _currentController = null;
        private PlaylistViewModel _playlistController;
        private LibraryViewModel _libraryController;

        public event Helper.DoubleDelegate onTimeChange;
        public event Helper.DoubleDelegate onClipOpen;
        public event Helper.VoidDelegate onPlay;
        public event Helper.VoidDelegate onPause;
        public event Helper.VoidDelegate onEnd;

        private DispatcherTimer timer = new DispatcherTimer();

        public PlaylistViewModel PlaylistController { get { return _playlistController; } set { _playlistController = value; } }

        public LibraryViewModel LibraryController { get { return _libraryController; } set { _libraryController = value; } }

        public ViewModel ViewModelController { get { return _currentController; } set { _currentController = value; } }

        private void updateTime(object sender, EventArgs e)
        {
            if (onTimeChange != null)
                onTimeChange(_currentController.getCurrentTime());
        }

        private void ME_Opened(object sender, RoutedEventArgs e)
        {
         if (((MediaElement)_currentController.getMediaPlayer()).NaturalDuration.HasTimeSpan)
            if (onClipOpen != null)
                onClipOpen(_currentController.getMaximumTime());
        }

        private void ME_Ended(object sender, RoutedEventArgs e)
        {
            stop();
            if (onEnd != null)
                onEnd();
        }

        private void createXmlFile()
        {
            Directory.CreateDirectory(Helper.xmlFolder);
            XDocument xmlFile = new XDocument(
            new XComment("XML File for storing playlists for the WindowsMediaPlayer"));
            xmlFile.Add(new XElement("WindowsMediaPlayer"));
            xmlFile.Save(Helper.xmlFolder + "/playlist.xml");
        }

        public ManagerViewModel()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += updateTime;
            if (!File.Exists(Helper.xmlFolder + "/playlist.xml"))
                createXmlFile();
            _playlistController = new PlaylistViewModel();
            _libraryController = new LibraryViewModel();
        }

        public UIElement getMedia()
        {
            if (_currentController != null)
                return _currentController.getMediaPlayer();
            else
                return null;
        }

        public void setPositionPlayer(System.TimeSpan value)
        {
            if (_currentController != null)
            {
                _currentController.setPositionPlayer(value);
                if (onTimeChange != null)
                    onTimeChange(_currentController.getCurrentTime());
            }
        }

        public void open(string file)
        {
            if (setRightController(file))
            {

                Console.WriteLine("Setrightcontroller for open(string file) ok");
                _playlistController.addClipToCurrentPlaylist(file);
                openNextClip();
            }

        }

        public bool open()
        {
            if (setRightController(_playlistController.getNextClip()))
            {
                Console.WriteLine("Setrightcontroller for open() ok");
                if (openNextClip())
                    return true;
            }
            return false;

        }

        private bool setRightController(string file)
        {
            string ext = System.IO.Path.GetExtension(file);
            if (ext == ".mp3")
            {
                _currentController = new AudioViewModel();
                ((MediaElement)_currentController.getMediaPlayer()).MediaOpened += ME_Opened;
                ((MediaElement)_currentController.getMediaPlayer()).MediaEnded += ME_Ended;
                return true;
            }
            else if (ext == ".avi" || ext == ".asf" || ext == ".wmv" || ext == ".mov" ||
                ext == ".3gp" || ext == ".3g2" || ext == ".flv" || ext == ".rm" ||
                ext == ".ogg" || ext == ".oga" || ext == ".ogv" || ext == ".ogx" ||
                ext == ".ogm" || ext == ".ts" || ext == ".webm" || ext == ".weba" ||
                ext == ".mxf" || ext == ".asx" || ext == ".nut" || ext == ".mp4" ||
                ext == ".mkv" || ext == ".mka" || ext == ".mks" || ext == "mpg" || ext == "mpeg")
            {
                _currentController = new VideoViewModel();
                ((MediaElement)_currentController.getMediaPlayer()).MediaOpened += ME_Opened;
                ((MediaElement)_currentController.getMediaPlayer()).MediaEnded += ME_Ended;
                return true;
            }
            else if (ext == ".jpg" || ext == ".gif" || ext == ".png" || ext == ".tif" ||
                     ext == ".jpeg")
            {
                _currentController = new PictureViewModel();
                return true;
            }
            return false;
        }

        public bool openNextClip()
        {
            string file;
            Console.WriteLine("openextclic begin");
            if ((file = _playlistController.getNextClip()) != null)
            {
                Console.WriteLine("dans le if du opennextclip (donc ya un clip de dispo)");

                _currentController.open(file);
                _playlistController.CurrentClip += 1;
                return true;
            }
            return false;
        }

        public void play()
        {
            if (_currentController != null)
            {
                timer.Start();
                _currentController.play();
                if (onPlay != null)
                    onPlay();
            }
        }

        public void pause()
        {
            if (_currentController != null)
            {
                timer.Stop();
                _currentController.pause();
                if (onPause != null)
                    onPause();
            }
        }

        public void stop()
        {
            if (_currentController != null)
            {
                timer.Stop();
                _currentController.stop();
                if (onPause != null)
                    onPause();
            }
        }

        public void changeVolume(double value)
        {
            if (_currentController != null)
                _currentController.changeVolume(value);
        }
        
    }
}
