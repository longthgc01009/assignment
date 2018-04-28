using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SoundCloud
{
    public sealed partial class PlaylistControl : UserControl
    {
        public Models.SoundCloudPlaylist Playlists
        {
            get { return this.DataContext as Models.SoundCloudPlaylist; }
        }

        public PlaylistControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void PlayAllPlaylist_Click(object sender, RoutedEventArgs e)
        {
            App.ListTrack = Playlists.tracks;
            App.NumberTrack = Playlists.tracks.Count;
            App.NowPlay = 0;
            App.IsPlayTop = false;
            MainPage.Current.PlayMediaListUser();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.SoundCloudTrack Track = e.ClickedItem as Models.SoundCloudTrack;
            MainPage.Current.PlayTrackUser(Track);
        }
    }
}
