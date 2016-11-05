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

namespace WindowsMediaPlayer.View
{
    public partial class removeClipFromPlaylist : Window
    {
        private ViewModel.PlaylistViewModel _playlistController;

        public removeClipFromPlaylist(ViewModel.PlaylistViewModel playlistController)
        {
            InitializeComponent();
            _playlistController = playlistController;
            ComboPlaylist.SelectionChanged += ComboBox1_SelectedIndexChanged;
            foreach (Model.Playlist item in _playlistController.Playlist)
            {
                if (item.ClipList.Any())
                    ComboPlaylist.Items.Add(item.Name);
            }
            ComboPlaylist.SelectedIndex = 0;
        }

         private void AddToPlaylist_Click(object sender, RoutedEventArgs e)
        {
          _playlistController.removeClipFromPlaylist(ComboClip.SelectedValue.ToString(), ComboPlaylist.SelectedValue.ToString());
          Close();
         }

         private void closeErrorInfo(object sender, RoutedEventArgs e)
         {
             ErrorInfo.Visibility = System.Windows.Visibility.Collapsed;
         }

        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
         {
            ComboClip.Items.Clear();  
            foreach (Model.Clip clip in _playlistController.getPlaylist(ComboPlaylist.SelectedValue.ToString()).ClipList)
            {
                ComboClip.Items.Add(clip.Path);
                ComboClip.SelectedIndex = 0;
            }
         }    

        }
}
