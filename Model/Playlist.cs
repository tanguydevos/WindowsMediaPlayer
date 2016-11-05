using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WindowsMediaPlayer.Model
{
    public class Playlist
    {
        private string _name = "basic";
        private List<Clip> _clipList = new List<Clip>();

        public Playlist(string name)
        {
            _name = name;
            load();
        }

        public Playlist()
        {
        }

        public List<Clip> ClipList { 
            get
            {
                return (_clipList);
            }

            set
            {
                _clipList = value;
            }
        }

        public string Name
        {
            get
            {
                return (_name);
            }

            set
            {
                _name = value;
            }
        }

        // delete an element of the playlist according to the type and the name
        public void deleteClip(Clip clip)
        {
            XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
            xmlFile.Element("WindowsMediaPlayer").Elements("Playlist")
                    .First(c => (string)c.Attribute("name") == _name).Elements("Element")
                    .Where(d => (string)d.Element("name").Value == clip.Path).Where(e => (string)e.Element("type").Value == clip.Type).Remove();
            xmlFile.Save(Helper.xmlFolder + "/playlist.xml");
            foreach (Clip clipp in _clipList)
            {
                if (clipp.Path == clip.Path && clipp.Type == clip.Type)
                {
                    _clipList.Remove(clipp);
                    break;
                }
            }
        }

        // delete the current playlist from the xml
        public void delete()
        {
            XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
            xmlFile.Element("WindowsMediaPlayer").Elements("Playlist")
                    .First(c => (string)c.Attribute("name") == _name).Remove();
            xmlFile.Save(Helper.xmlFolder + "/playlist.xml");
        }

        // Loading the playlist and adding every clip in the list _clipList
        public void load()
        {
            if (_name != null)
            {
                XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
                var result = from playlist in xmlFile.Descendants("Playlist")
                             where (string)playlist.Attribute("name") == _name
                             from song in playlist.Descendants("Element")
                             select new
                             {
                                 type = song.Element("type").Value,
                                 name = song.Element("name").Value
                             };
                foreach (var item in result)
                {
                    if (item.type == "Audio")
                        _clipList.Add(new AudioClip(item.name));
                    else if (item.type == "Video")
                        _clipList.Add(new VideoClip(item.name));
                    else if (item.type == "Picture")
                        _clipList.Add(new PictureClip(item.name));
                }
            }
        }

        // add elem to the playlist
        public bool addClip(Clip clip)
        {
            XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
            // Le code commenté est pour la vérification de si le clip existe deja ds le xml
      //      var result = from playlist in xmlFile.Descendants("Playlist")
         //                where (string)playlist.Attribute("name") == _name
          //               from song in playlist.Descendants("Element")
          //               where (string)song.Element("type") == clip.Type && (string)song.Element("name") == clip.Name
          //               select playlist;
         //   if (result.ToList().Any() == false)
        //    {
                xmlFile.Element("WindowsMediaPlayer").Elements("Playlist")
                    .First(c => (string)c.Attribute("name") == _name).Add(new XElement("Element",
                        new XElement("type", clip.Type),
                        new XElement("name", clip.Path)));
                xmlFile.Save(Helper.xmlFolder + "/playlist.xml");
                _clipList.Add(clip);
                return true;
         //   }
         //   return false;
        }

        public void addClipToList(Clip clip)
        {
            _clipList.Add(clip);
        }

        // static function to create a new playlist with a name
        static public bool create(string _name)
        {
            XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
            XElement character = xmlFile.Descendants("WindowsMediaPlayer").FirstOrDefault();
            var result = from playlist in xmlFile.Descendants("Playlist")
                         where (string)playlist.Attribute("name") == _name
                         select playlist;
            if (character != null && !result.Any())
            {
                character.Add(new XElement("Playlist",
                       new XAttribute("name", _name)
                     ));
                xmlFile.Save(Helper.xmlFolder + "/playlist.xml");
                return true;
            }
            return false;
        }

        static public List<string> loadAllPlaylistName()
        {
            List<string> listPlaylistName = new List<string>();

            XDocument xmlFile = XDocument.Load(Helper.xmlFolder + "/playlist.xml");
            var result = from playlist in xmlFile.Descendants("Playlist")
                         select new
                         {
                             name = playlist.Attribute("name").Value
                         };
            foreach (var item in result)
            {
                listPlaylistName.Add(item.name);
            }
            return listPlaylistName;
        }

    }
}
