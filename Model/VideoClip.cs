using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsMediaPlayer.Model
{
    class VideoClip : Clip
    {
        public VideoClip(string fileName)
        {
            path = fileName;
            type = "Video";
            title = System.IO.Path.GetFileName(fileName);
            try
            {
                TagLib.File tagFile = TagLib.File.Create(fileName);
                if (tagFile.Properties.Duration != null)
                    duration = tagFile.Properties.Duration;
            }
            catch (Exception e)
            {
            }
        }
    }
}
