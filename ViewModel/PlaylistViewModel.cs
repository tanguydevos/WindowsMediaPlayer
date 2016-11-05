using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMediaPlayer.ViewModel
{
    public class PlaylistViewModel
    {
        private List<Model.Playlist> _playlist = new List<Model.Playlist>();
        private Model.Playlist currentPlaylist = new Model.Playlist();
        private int currentClip;

        public Model.Playlist CurrentPlaylist { get { return currentPlaylist; } set { currentPlaylist = value; } }

        public int CurrentClip { get { return currentClip; } set { currentClip = value; } }

        public PlaylistViewModel()
        {
            List<string> playlistName = Model.Playlist.loadAllPlaylistName();
            currentClip = 0;
            foreach (string bla in playlistName)
            {
                _playlist.Add(new Model.Playlist(bla));
            }
        }

        public void changeMusic(Model.Clip clip)
        {
            int count = 0;
            foreach (Model.Clip clipp in currentPlaylist.ClipList)
            {
                if (clip == clipp)
                {
                    currentClip = count;
                    break;
                }
                count++;
            }
        }

        public void openPlaylist(string name)
        {
            foreach (Model.Playlist item in _playlist)
            {
                if (item.Name == name)
                {
                    currentPlaylist = item;
                    currentClip = 0;
                    break;
                } 
            }
        }

        public string getNextClip()
        {
            int count = 0;
            if (currentClip < 0)
                currentClip = 0;
            foreach (Model.Clip clip in currentPlaylist.ClipList)
            {
                if (count == currentClip)
                {
                    Console.WriteLine("musique trouve ds playlist:" + clip.Path);
                    return clip.Path;
                }
                count++;
            }
            return null;
        }

        public List<Model.Playlist> Playlist {
            get
            {
                return _playlist;
            } 
            set
            {
                _playlist = value;
            }
        }
        public void addPlaylist(string name)
        {
            _playlist.Add(new Model.Playlist(name));
        }

        public Model.Playlist getPlaylist(string name)
        {
            foreach(Model.Playlist item in _playlist)
            {
                if (item.Name == name)
                    return item;
            }
            return null;
        }

        public void addClipToPlaylist(string @filePath, string playlistName)
        {
            foreach (Model.Playlist playlist in _playlist)
            {
                if (playlist.Name == playlistName)
                {
                    string ext = System.IO.Path.GetExtension(@filePath);
                    if (ext == ".mp3")
                        playlist.addClip(new Model.AudioClip(@filePath));
                    else if (ext == ".avi" || ext == ".asf" || ext == ".wmv" || ext == ".mov" || 
                        ext == ".3gp" || ext == ".3g2" || ext == ".flv" || ext == ".rm" || 
                       ext == ".ogg" || ext == ".oga" || ext == ".ogv" || ext == ".ogx" || 
                       ext == ".ogm" || ext == ".ts" || ext == ".webm" || ext == ".weba" || 
                       ext == ".mxf" || ext == ".asx" || ext == ".nut" || ext == ".mp4" || 
                       ext == ".mkv" || ext == ".mka" || ext == ".mks" || ext == "mpg" || ext == "mpeg")
                        playlist.addClip(new Model.VideoClip(@filePath));
                    else if (ext == ".jpg")
                        playlist.addClip(new Model.PictureClip(@filePath));
                    break;
                }
            }
        }

        public void addClipToCurrentPlaylist(string @filePath)
        {
            currentPlaylist.ClipList.Clear();
            CurrentClip = 0;
                    string ext = System.IO.Path.GetExtension(@filePath);
                    if (ext == ".mp3")
                        currentPlaylist.addClipToList(new Model.AudioClip(@filePath));
                    else if (ext == ".avi" || ext == ".asf" || ext == ".wmv" || ext == ".mov" ||
                        ext == ".3gp" || ext == ".3g2" || ext == ".flv" || ext == ".rm" ||
                       ext == ".ogg" || ext == ".oga" || ext == ".ogv" || ext == ".ogx" ||
                       ext == ".ogm" || ext == ".ts" || ext == ".webm" || ext == ".weba" ||
                       ext == ".mxf" || ext == ".asx" || ext == ".nut" || ext == ".mp4" ||
                       ext == ".mkv" || ext == ".mka" || ext == ".mks" || ext == "mpg" || ext == "mpeg")
                        currentPlaylist.addClipToList(new Model.VideoClip(@filePath));
                    else if (ext == ".jpg")
                        currentPlaylist.addClipToList(new Model.PictureClip(@filePath));
        }

        public void deletePlaylist(string name)
        {
            foreach (Model.Playlist item in _playlist)
            {
                if (item.Name == name)
                {
                    item.delete();
                    _playlist.Remove(item);
                    break;
                }
            }
        }

        public List<Model.Clip> getCurrentClips()
        {
            return currentPlaylist.ClipList;
        }

        public bool hasClip()
        {
            foreach (Model.Playlist playlist in _playlist)
            {
                if (playlist.ClipList.Any())
                    return true;
            }
            return false;
        }

        public void removeClipFromPlaylist(string @filePath, string playlistName)
        {
            foreach (Model.Playlist playlist in _playlist)
            {
                if (playlist.Name == playlistName)
                {
                    string ext = System.IO.Path.GetExtension(@filePath);
                    if (ext == ".mp3")
                        playlist.deleteClip(new Model.AudioClip(@filePath));
                    else if (ext == ".avi" || ext == ".asf" || ext == ".wmv" || ext == ".mov" ||
                        ext == ".3gp" || ext == ".3g2" || ext == ".flv" || ext == ".rm" ||
                       ext == ".ogg" || ext == ".oga" || ext == ".ogv" || ext == ".ogx" ||
                       ext == ".ogm" || ext == ".ts" || ext == ".webm" || ext == ".weba" ||
                       ext == ".mxf" || ext == ".asx" || ext == ".nut" || ext == ".mp4" ||
                       ext == ".mkv" || ext == ".mka" || ext == ".mks" || ext == "mpg" || ext == "mpeg")
                        playlist.deleteClip(new Model.VideoClip(@filePath));
                    else if (ext == ".jpg")
                        playlist.deleteClip(new Model.PictureClip(@filePath));
                    break;
                }
            }
        }

        public bool prevClip()
        {
            if (currentClip > 1)
            {
                currentClip -= 2;
                return true;
            }
            currentClip = 0;
            return false;
        }
    }
}
