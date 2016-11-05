using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Microsoft.Win32;


namespace WindowsMediaPlayer.View
{

    partial class MainWindow : Window
    {
        private ViewModel.ManagerViewModel _manager = new ViewModel.ManagerViewModel();

        bool fullscreen = false;
        public string toggleStr
        {
            get { return (string)GetValue(toggleStrProperty); }
            set { SetValue(toggleStrProperty, value); }
        }

        public static readonly DependencyProperty toggleStrProperty =
            DependencyProperty.Register("toggleStr", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public MainWindow()
        {
            InitializeComponent();
            _manager.onTimeChange += onTimeChange;
            _manager.onClipOpen += onClipOpen;
            _manager.onPlay += onPlay;
            _manager.onPause += onPause;
            _manager.onEnd += onEnd;
            SearchBar.TextChanged += searchBar_textChanged;
            sliProgress.Maximum = 1;
            this.DataContext = this;
            this.toggleStr = "Do not Reload Library";

        }

        private void openYoutubeClick(object sender, RoutedEventArgs e)
        {
	        setLibraryToHidden();
            YoutubeBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void youTubeClickOk(object sender, RoutedEventArgs e)
        {
            YoutubeBox.Visibility = System.Windows.Visibility.Hidden;
            String input = YoutubeTextBox.Text;
            if (input == "")
                TextErrorInfo.Text = "No name specified";
            if (input.StartsWith("https://www.youtube.com/watch?v=") == false)
                TextErrorInfo.Text = "Not a Youtube Video";
            else	
            {
                string embedurl = null;
                embedurl = GetEmbedUrlFromLink(input);

                if (embedurl == null)
                    TextErrorInfo.Text = "Error with the link";
                else
                {
                    _manager.stop();
                    if (_manager.getMedia() != null)
                        centralGrid.Children.Remove(_manager.getMedia());
                    Youtube.NavigateToString(embedurl);
                    Youtube.Visibility = Visibility.Visible;
                    ReturnToMainView.Visibility = Visibility.Visible;
                }

            }
            if (TextErrorInfo.Text != "")
            	ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            InputTextBox.Text = String.Empty;
        }

        private void fermerYouTubeClick(object sender, RoutedEventArgs e)
        {
            YoutubeBox.Visibility = System.Windows.Visibility.Collapsed;
            YoutubeTextBox.Text = String.Empty;
            ReturnToMainView.Visibility = Visibility.Hidden;
        }

        private void toggleLibraryReload(object sender, RoutedEventArgs e)
        {
            toggleStr = _manager.LibraryController.toggleReload();
        }

        private string GetEmbedUrlFromLink(string link)
        {
            try
            {
                const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
                const string replacement = "<html style='overflow:hidden;'><body scroll='no' style='background=black;'><iframe title='YouTube video player' src='http://www.youtube.com/embed/$1' width='480' height='390' frameborder='0' allowfullscreen='1'></iframe></body></html>";
                var rgx = new Regex(pattern);
                var result = rgx.Replace(link, replacement);
                return result;
            }
            catch
            {
                return null;
            }
        }

        private void refreshCurrentLibrary()
        {
            if (audioLibrary.Visibility == Visibility.Visible)
                audioLibrary.Items.Refresh();
            else if (pictureLibrary.Visibility == Visibility.Visible)
                pictureLibrary.Items.Refresh();
            else if (movieLibrary.Visibility == Visibility.Visible)
                movieLibrary.Items.Refresh();
        }

        private void SearchBar_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchBar.Text = "";
        }

        private void searchBar_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (SearchBar.Text == "Search a music..." || SearchBar.Text == "Search a movie..." || SearchBar.Text == "Search a picture...")
                return;
            if (audioLibrary.Visibility == Visibility.Visible)
                audioLibrary.ItemsSource = _manager.LibraryController.loadAudioClipsWithName(SearchBar.Text);
            if (movieLibrary.Visibility == Visibility.Visible)
                movieLibrary.ItemsSource = _manager.LibraryController.loadMovieClipsWithName(SearchBar.Text);
            if (pictureLibrary.Visibility == Visibility.Visible)
                pictureLibrary.ItemsSource = _manager.LibraryController.loadPictureClipsWithName(SearchBar.Text);
            refreshCurrentLibrary();
        }

        private void openCreatePlaylistClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void createPlaylistCloseClick(object sender, RoutedEventArgs e)
        {
            ErrorInfo.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void addClipPlaylistClick(object sender, RoutedEventArgs e)
        {
            if (_manager.PlaylistController.Playlist.Any())
            {
                addClipToPlaylist window = new addClipToPlaylist(_manager.PlaylistController);
                window.ShowDialog();
            }
            else
            {
                TextErrorInfo.Text = "There is no playlist created, please create one";
                ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void removeClipPlaylistClick(object sender, RoutedEventArgs e)
        {
            if (_manager.PlaylistController.Playlist.Any() && _manager.PlaylistController.hasClip())
            {
                removeClipFromPlaylist window = new removeClipFromPlaylist(_manager.PlaylistController);
                window.ShowDialog();
            }
            else
            {
                TextErrorInfo.Text = "There is no clip to remove, please add one";
                ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void deletePlaylistClick(object sender, RoutedEventArgs e)
        {
            if (_manager.PlaylistController.Playlist.Any())
            {
                deletePlaylist window = new deletePlaylist(_manager.PlaylistController);
                window.ShowDialog();
            }
            else
            {
                TextErrorInfo.Text = "There is no playlist to delete, please create one";
                ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void createPlaylistClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }


        private void createPlaylistClickOk(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Hidden;
            String input = InputTextBox.Text;
            if (input == "")
                TextErrorInfo.Text = "No name specified";
            else if (!Model.Playlist.create(input))
                TextErrorInfo.Text = "The playlist " + input + " already exist";
            else
            {
                TextErrorInfo.Text = "The playlist " + input + " has been created";
                _manager.PlaylistController.addPlaylist(input);
            }
            ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            InputTextBox.Text = String.Empty;
        }



        private void closePlaylistClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            InputTextBox.Text = String.Empty;
        }

        private void currentListClip_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Model.Clip clip = (Model.Clip)currentListClip.SelectedItem;

            if (clip != null)
            {
                _manager.PlaylistController.changeMusic(clip);
                onEnd();
            }
        }

        private void library_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Model.Clip clip = (Model.Clip)audioLibrary.SelectedItem;
            if (clip == null)
               clip = (Model.Clip)movieLibrary.SelectedItem;
            if (clip == null)
               clip = (Model.Clip)pictureLibrary.SelectedItem;
            if (_manager.getMedia() != null)
                centralGrid.Children.Remove(_manager.getMedia());
            _manager.open(clip.Path);
            Youtube.Visibility = Visibility.Hidden;
            Youtube.Source = null;
            if (_manager.getMedia() != null)
            {
                centralGrid.Children.Add(_manager.getMedia());
            }
            _manager.play();
            updateRightListView();
            setLibraryToHidden();
        }

        private void updateRightListView()
        {
            currentListClip.ItemsSource = _manager.PlaylistController.getCurrentClips();
            currentListClip.Items.Refresh();
        }

        private void openClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            var result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (_manager.getMedia() != null)
                    centralGrid.Children.Remove(_manager.getMedia());
                _manager.open(openFileDialog1.FileName);
                Youtube.Visibility = Visibility.Hidden;
                Youtube.Source = null;
                if (_manager.getMedia() != null)
                {
                    centralGrid.Children.Add(_manager.getMedia());
                    setLibraryToHidden();
                }
                _manager.play();
                updateRightListView();
            }

        }

        public void onEnd()
        {
            sliProgress.Value = 0;
            if (_manager.PlaylistController.CurrentPlaylist != null)
            {
                Youtube.Visibility = Visibility.Hidden;
                Youtube.Source = null;
              if (_manager.getMedia() != null)
                    centralGrid.Children.Remove(_manager.getMedia());
                if (_manager.open() != false)
                {
                    if (_manager.getMedia() != null)
                    centralGrid.Children.Add(_manager.getMedia());
                    _manager.play();
                }
            }
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if (_manager.ViewModelController == null)
                openClick(sender, e);
            else
            {
                if (MediaButton.Content == FindResource("Pause"))
                    _manager.pause();
                else
                    _manager.play();
            }
        }

        

        public void onPlay()
        {
           MediaButton.Content = FindResource("Pause");
        }

        public void onPause()
        {
            MediaButton.Content = FindResource("Play");
        }

        public void onTimeChange(double value)
        {
            sliProgress.Value = value;
        }

        public void onClipOpen(double value)
        {
            sliProgress.Minimum = 0;
            sliProgress.Maximum = value;
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            _manager.stop();
            MediaButton.Content = FindResource("Play");
            _manager.setPositionPlayer(TimeSpan.FromSeconds(0));
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _manager.setPositionPlayer(TimeSpan.FromSeconds(sliProgress.Value));
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void openPlaylistClick(object sender, RoutedEventArgs e)
        {
            if (_manager.PlaylistController.Playlist.Any() && _manager.PlaylistController.hasClip())
            {
                openPlaylist window = new openPlaylist(_manager.PlaylistController);
                window.ShowDialog();
                if (_manager.open())
                {
                    Youtube.Visibility = Visibility.Hidden;
                    Youtube.Source = null;
                    if (_manager.getMedia() != null)
                        centralGrid.Children.Remove(_manager.getMedia());
                    if (_manager.getMedia() != null)
                    {
                        centralGrid.Children.Add(_manager.getMedia());
                    }
                    _manager.play();
                    updateRightListView();
                }
            }
            else
            {
                TextErrorInfo.Text = "There is no playlist or music in to play";
                ErrorInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _manager.changeVolume(volumeSlider.Value);
            
        }

        private void PictureHandler(object sender, MouseButtonEventArgs e)
        {
            setLibraryToHidden();
            pictureLibrary.ItemsSource = _manager.LibraryController.loadPictureClips();
            pictureLibrary.Items.Refresh();
            pictureLibrary.Visibility = Visibility.Visible;
            SearchBar.Visibility = Visibility.Visible;
            SearchBar.Text = "Search a picture...";
            ReturnToMainView.Visibility = Visibility.Visible;
        }

        private void MovieHandler(object sender, MouseButtonEventArgs e)
        {
            setLibraryToHidden();
            movieLibrary.ItemsSource = _manager.LibraryController.loadMovieClips();
            movieLibrary.Items.Refresh();
            movieLibrary.Visibility = Visibility.Visible;
            SearchBar.Visibility = Visibility.Visible;
            SearchBar.Text = "Search a movie...";
            ReturnToMainView.Visibility = Visibility.Visible;
        }

        private void MusicHandler(object sender, MouseButtonEventArgs e)
        {
            setLibraryToHidden();
            audioLibrary.ItemsSource = _manager.LibraryController.loadAudioClips();
            audioLibrary.Items.Refresh();
            audioLibrary.Visibility = Visibility.Visible;
            SearchBar.Visibility = Visibility.Visible;
            SearchBar.Text = "Search a music...";
            ReturnToMainView.Visibility = Visibility.Visible;
        }

        private void setLibraryToHidden()
        {
            audioLibrary.Visibility = Visibility.Hidden;
            pictureLibrary.Visibility = Visibility.Hidden;
            movieLibrary.Visibility = Visibility.Hidden;
            audioLibrary.SelectedItem = null;
            pictureLibrary.SelectedItem = null;
            movieLibrary.SelectedItem = null;
            Youtube.Visibility = Visibility.Hidden;
            Youtube.Source = null;
            SearchBar.Visibility = Visibility.Hidden;
        }

        private void ReturnToMainViewHandler(object sender, MouseButtonEventArgs e)
        {
            setLibraryToHidden();
            Youtube.Visibility = Visibility.Hidden;
            ReturnToMainView.Visibility = Visibility.Hidden;
        }

      
        public void MediaPlayer_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
           
            if (!fullscreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                libraries.Visibility = System.Windows.Visibility.Hidden;
                currentListClip.Visibility = System.Windows.Visibility.Hidden;
                MenuBar.Visibility = System.Windows.Visibility.Hidden;
                centralGrid.Margin = new Thickness(0, 0, 0, 0); 
                Button_bottom.Opacity = 0;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                libraries.Visibility = System.Windows.Visibility.Visible;
                currentListClip.Visibility = System.Windows.Visibility.Visible;
                MenuBar.Visibility = System.Windows.Visibility.Visible;
                centralGrid.Margin = new Thickness(84, 20, 134, 57);
                Button_bottom.Opacity = 1;
            }

            fullscreen = !fullscreen;
        }

        public void Button_bottom_MouseEnter(object sender, System.EventArgs e)
        {
            if (fullscreen)
            {
                Button_bottom.Opacity = 1;

            }
        }

        public void Button_bottom_MouseLeave(object sender, System.EventArgs e)
        {
            if (fullscreen)
            {
                Button_bottom.Opacity = 0;
            }
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            onEnd();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_manager.PlaylistController.prevClip())
                onEnd();
            else
            {
                _manager.stop();
                if (_manager.open())
                {
                    Youtube.Visibility = Visibility.Hidden;
                    Youtube.Source = null;
                    if (_manager.getMedia() != null)
                        centralGrid.Children.Remove(_manager.getMedia());
                    if (_manager.getMedia() != null)
                    {
                        centralGrid.Children.Add(_manager.getMedia());
                    }
                    _manager.play();
                    updateRightListView();
                }
            }
        }
    }
}
