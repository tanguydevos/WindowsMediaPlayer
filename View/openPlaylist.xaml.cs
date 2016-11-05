using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowsMediaPlayer.View
{

    public partial class openPlaylist : Window
    {
        private ViewModel.PlaylistViewModel _playlistController;

        public openPlaylist(ViewModel.PlaylistViewModel playlistController)
        {
            InitializeComponent();
            _playlistController = playlistController;
            foreach (Model.Playlist item in _playlistController.Playlist)
            {
                ComboPlaylist.Items.Add(item.Name);
            }
            ComboPlaylist.SelectedIndex = 0;

        }

        private void OpenPlaylist_Click(object sender, RoutedEventArgs e)
        {
            _playlistController.openPlaylist(ComboPlaylist.SelectedValue.ToString());
            Close();
        }


    }
}
