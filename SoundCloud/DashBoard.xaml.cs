using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SoundCloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoard : Page
    {
        public ObservableCollection<Models.SoundCloudPlaylist> listPlaylist { get; set; }
        public ObservableCollection<Models.SoundCloudTrack> FavoritesTrack { get; set; }
        public DownloadJson clientDownload { get; set; }
        public DashBoard()
        {
            this.InitializeComponent();
            listPlaylist = new ObservableCollection<Models.SoundCloudPlaylist>();
            FavoritesTrack = new ObservableCollection<Models.SoundCloudTrack>();
            clientDownload = new DownloadJson();
        }

        public async void GetData(int id)
        {
            var json = await clientDownload.GetUserPlaylist(id);
            Models.SoundCloudPlaylist[] Data = JsonConvert.DeserializeObject<Models.SoundCloudPlaylist[]>(json);
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].artwork_url == null)
                {
                    Data[i].artwork_url = "https://i1.sndcdn.com/avatars-000333197764-zard19-large.jpg";
                }

                listPlaylist.Add(Data[i]);
            }
        }

        public async void GetFavorites(int id)
        {
            var json = await clientDownload.GetUserFavorites(id);
            Models.SoundCloudTrack[] Data = JsonConvert.DeserializeObject<Models.SoundCloudTrack[]>(json);
            for (int i = 0; i < Data.Length; i++)
            {
                FavoritesTrack.Add(Data[i]);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Models.SoundCloudMe Me = e.Parameter as Models.SoundCloudMe;
            this.GetData(Me.id);
            this.GetFavorites(Me.id);
            var avatarBrush = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Me.avatar_url)) };
            AvatarE.Fill = avatarBrush;
            tbUserName.Text = Me.username;
            tbUserLink.Text = Me.permalink;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.SoundCloudTrack Track = e.ClickedItem as Models.SoundCloudTrack;
            MainPage.Current.PlayTrackUser(Track);
        }

        private void PlayAllLikes_Click(object sender, RoutedEventArgs e)
        {
            App.ListTrack = FavoritesTrack.ToList();
            App.NumberTrack = FavoritesTrack.Count;
            App.NowPlay = 0;
            App.IsPlayTop = false;
            MainPage.Current.PlayMediaListUser();
        }
    }
}
