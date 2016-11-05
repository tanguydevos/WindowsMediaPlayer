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
    partial class addClipToPlaylist : Window
    {
        private ViewModel.PlaylistViewModel _playlistController;

        public addClipToPlaylist(ViewModel.PlaylistViewModel playlistController)
        {
            InitializeComponent();
            _playlistController = playlistController;
            foreach (Model.Playlist item in _playlistController.Playlist)
                ComboBox1.Items.Add(item.Name);
            ComboBox1.SelectedIndex = 0;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            var result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                BrowseData.Content = openFileDialog1.FileName;
        }

         private void AddToPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists((string)BrowseData.Content))
            {
                _playlistController.addClipToPlaylist((string)BrowseData.Content, ComboBox1.SelectedValue.ToString());
                Close();
            }
            else
            {
                TextErrorInfo.Text = "The file you took doesn't exist";
                ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

         private void closeErrorInfo(object sender, RoutedEventArgs e)
         {
             ErrorInfo.Visibility = System.Windows.Visibility.Collapsed;
         }
    }
}
