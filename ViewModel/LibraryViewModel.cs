using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using System.Text;
using System.Threading.Tasks;
using WindowsMediaPlayer.Model;
using System.ComponentModel;

namespace WindowsMediaPlayer.ViewModel
{
    class LibraryViewModel
    {
        private List<AudioClip> _audioClips = new List<AudioClip>();
        private List<VideoClip> _movieClips = new List<VideoClip>();
        private List<PictureClip> _pictureClips = new List<PictureClip>();
        private string musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private string movieFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        private string pictureFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        private bool reloadLib = true;
        public  string _toggleStr;

        public LibraryViewModel()
        {
        }

        public string toggleReload()
        {
            if (reloadLib)
            {
                _toggleStr = "Reload library";
            }
            else
            {
                _toggleStr = "Do not Reload library";
            }
            reloadLib = !reloadLib;
            return _toggleStr;
        }

        public List<AudioClip> loadAudioClips()
        {
            if (!_audioClips.Any() || reloadLib == true)
            {
                _audioClips.Clear();
                var filePaths = Directory.EnumerateFiles(musicFolder, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".mp3"));
                foreach (string value in filePaths)
                {
                    _audioClips.Add(new AudioClip(@value));
                }
                if (_audioClips.Any())
                    return _audioClips;
                return null;
            }
            else
                return _audioClips;
            }

        public List<PictureClip> loadPictureClips()
        {
            if (!_pictureClips.Any() || reloadLib == true)
            {
                _pictureClips.Clear();
                var filePaths = Directory.EnumerateFiles(pictureFolder, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".jpg"));
                foreach (string value in filePaths)
                {
                    _pictureClips.Add(new PictureClip(@value));
                }
                if (_pictureClips.Any())
                    return _pictureClips;
                return null;
            }
            else
                return _pictureClips;
        }

        public List<VideoClip> loadMovieClips()
        {
            if (!_movieClips.Any() || reloadLib == true)
            {
                _movieClips.Clear();
                var filePaths = Directory.EnumerateFiles(movieFolder, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".avi"));
                foreach (string value in filePaths)
                {
                    _movieClips.Add(new VideoClip(@value));
                }
                if (_movieClips.Any())
                    return _movieClips;
                return null;
            }
            else
                return _movieClips;
        }


        internal IEnumerable<PictureClip> loadPictureClipsWithName(string p)
        {
            IEnumerable<PictureClip> results = from picture in _pictureClips
                                               where picture.Title.ToLower().Contains(p.ToLower()) || picture.ImageDesc.ToLower().Contains(p.ToLower()) || picture.DateTaken.ToString().ToLower().Contains(p.ToLower())
                                             select picture;
            return results; 
        }

        internal IEnumerable<VideoClip> loadMovieClipsWithName(string p)
        {
            IEnumerable<VideoClip> results = from video in _movieClips
                                             where video.Title.ToLower().Contains(p.ToLower())
                                             select video;
            return results; 
        }

        internal IEnumerable<AudioClip> loadAudioClipsWithName(string p)
        {
            IEnumerable<AudioClip> results = from audio in _audioClips
                                             where audio.Title.ToLower().Contains(p.ToLower()) || audio.Album.ToLower().Contains(p.ToLower()) || audio.Artist.ToLower().Contains(p.ToLower())
                                             select audio;
            return results; 
        }
    }
}
