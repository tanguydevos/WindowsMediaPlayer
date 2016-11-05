using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsMediaPlayer.Model
{
    class PictureClip : Clip
    {
        private DateTime? dateTaken = null;
        private uint? rated = null;
        private int width = 0;
        private int height = 0;
        private string imageDesc = "";

        public string ImageDesc { get { return imageDesc; } set { imageDesc = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int Width { get { return width; } set { width = value; } }
        public DateTime? DateTaken { get { return dateTaken; } set { dateTaken = value; } }

        public PictureClip(string fileName)
        {
            path = fileName;
            type = "Picture";
            title = System.IO.Path.GetFileName(fileName);
            try
            {
                TagLib.File tagFile = TagLib.File.Create(fileName);
                var imageDetails = tagFile as TagLib.Image.File;
                if (imageDetails.ImageTag.DateTime != null)
                    dateTaken = imageDetails.ImageTag.DateTime ?? null;
                if (imageDetails.ImageTag.Rating != null)
                    rated = imageDetails.ImageTag.Rating ?? null;
                width = imageDetails.Properties.PhotoWidth;
                height = imageDetails.Properties.PhotoHeight;
                if (imageDetails.Tag.Title != null)
                    imageDesc = imageDetails.Tag.Title;
                Console.WriteLine(dateTaken + " " + rated + " " + width + " " + height + " " + imageDesc);
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
        }
    }
}