using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WindowsMediaPlayer.Model
{
    class AudioClip : Clip
    {
        private string album = "Unknown";
        private string artist = "Unknown";


        public string Album { get { return album; } set { album = value; } }

        public string Artist { get { return artist; } set { artist = value; } }

        public AudioClip(string fileName)
        {
            path = fileName;
            type = "Audio";
            try
            {
                title = System.IO.Path.GetFileName(fileName);
                TagLib.File tagFile = TagLib.File.Create(fileName);
                duration = tagFile.Properties.Duration;
                if (tagFile.Tag.Album != null)
                    album = tagFile.Tag.Album;
                if (tagFile.Tag.Performers.Any())
                    artist = tagFile.Tag.Performers.FirstOrDefault();
                if (tagFile.Tag.Title != null)
                    title = tagFile.Tag.Title;
                Console.WriteLine(album + " " + artist + " " + duration.Minutes + " " + title);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        public AudioClip()
        {}
    }
}
