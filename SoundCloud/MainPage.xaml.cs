using Newtonsoft.Json;
using SoundCloud.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundCloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static String Code { get; set; }
        public static String AccessToken { get; set; }
        private ObservableCollection<Models.Menu> ListMenu { get; set; }
        private string PLAY = "";
        private string PAUSE = "";
        public static MainPage Current { get; set; }
        public DownloadJson clientDownload { get; set; }
        public Models.SoundCloudMe meInfo { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += this.PageLoaded;
            ListMenu = new ObservableCollection<Models.Menu>();
            Player.MediaOpened += this.HandleMediaOpened;
            Player.MediaEnded += this.HandleMediaEnded;
            Player.BufferingProgressChanged += this.HandleProgressChanged;
            this.SetupMenu();
            clientDownload = new DownloadJson();
            MainFrame.Navigate(typeof(ChartPage));

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Current = this;
        }

        private async void BtnLogin_Click()
        {
            var dialog = new Authen();
            dialog.Closing += this.HandleAuthen;
            await dialog.ShowAsync();
        }

        private async void HandleAuthen(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            this.SetupMenu();
            UserInfo.Visibility = Visibility.Visible;
            meInfo = await this.GetInfoMe();
            var avatarBrush = new ImageBrush() { ImageSource = new BitmapImage(new Uri(meInfo.avatar_url))};
            btnUserInfo.Content = meInfo.username;
            AvatarE.Fill = avatarBrush;
            MainFrame.Navigate(typeof(DashBoard), meInfo);
        }

        private async Task<Models.SoundCloudMe> GetInfoMe()
        {
            var json = await clientDownload.GetFullInfoMe(App.AccessToken);
            var Me = JsonConvert.DeserializeObject<Models.SoundCloudMe>(json);
            return Me;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var MenuClick = e.ClickedItem as Models.Menu;
            switch (MenuClick.Tag)
            {
                case "LOGIN":
                    this.BtnLogin_Click();
                    break;
                case "TOP":
                    MainFrame.Navigate(typeof(ChartPage));
                    break;
            }
        }

        public void SetupMenu()
        {
            if (!App.Login) {
                ListMenu.Clear();
                ListMenu.Add(new Models.Menu()
                {
                    Tag = "LOGIN",
                    Title = "Login",
                    Icon = ""
                });
                ListMenu.Add(new Models.Menu()
                {
                    Tag = "TOP",
                    Title = "TOP 50",
                    Icon = ""
                });
            } else
            {
                ListMenu.Clear();
                ListMenu.Add(new Models.Menu()
                {
                    Tag = "TOP",
                    Title = "TOP 50",
                    Icon = ""
                });
            }
        }
        // Media Player Control
        private void HandleMediaOpened(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += this.HandleProgressChanged;
            btnPlayerPlayPause.Content = this.PAUSE;
            ProgressMusic.Minimum = 0;
            ProgressMusic.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
            txtTrackPosition.Text = "0:00";
            txtTrackDuration.Text = " /  " + Player.NaturalDuration.TimeSpan.Minutes + ":" + Player.NaturalDuration.TimeSpan.Seconds;
            timer.Start();
        }

        private void HandleProgressChanged(object sender, object e)
        {
            ProgressMusic.Value = Player.Position.TotalSeconds;
            txtTrackPosition.Text = Player.Position.Seconds < 10 ? Player.Position.Minutes + ":0" + Player.Position.Seconds : Player.Position.Minutes + ":" + Player.Position.Seconds;
        }

        private void HandleMediaEnded(object sender, RoutedEventArgs e)
        {
            App.NowPlay = App.NowPlay < App.NumberTrack ? App.NowPlay + 1 : 0;
            btnPlayerPlayPause.Content = this.PLAY;
            this.PlayMediaList();
        }

        public void PlayMediaList()
        {
            if (App.ListTopTrack.Count > 0)
            {
                this.PlayTrack(App.ListTopTrack.ElementAt(App.NowPlay));
            }
        }

        public void PlayMediaListUser()
        {
            if (App.ListTrack.Count > 0)
            {
                this.PlayTrackUser(App.ListTrack.ElementAt(App.NowPlay));
            }
        }

        public void PlayTrackUser(Models.SoundCloudTrack Track) {
            Player.Stop();
            var uri = new Uri("https://api.soundcloud.com/tracks/" + Track.id + "/stream" + "?client_id=" + App.CLIENT_ID);
            Player.Source = uri;
            txtTrackName.Text = Track.title;
            txtTrackUser.Text = Track.user.username;
            if (Track.artwork_url != null)
            {
                ImageTrack.Source = new BitmapImage(new Uri(Track.artwork_url));
            }
            Player.Play();
        }

        public void PlayTrack(Models.TopTrack Track)
        {
            Player.Stop();
            var uri = new Uri("https://api.soundcloud.com/tracks/" + Track.id + "/stream" + "?client_id=" + App.CLIENT_ID);
            Player.Source = uri;
            txtTrackName.Text = Track.title;
            txtTrackUser.Text = Track.user.username;
            ImageTrack.Source = new BitmapImage(new Uri(Track.artwork_url));
            Player.Play();
        }

        private void ProgressMusic_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var a = sender as UIElement;
            var value = (e.GetPosition(a).X / ProgressMusic.RenderSize.Width) * ProgressMusic.Maximum;
            ProgressMusic.Value = value;
            Player.Position = TimeSpan.FromSeconds(value);
        }

        private void btnPlayerPrev_Click(object sender, RoutedEventArgs e)
        {
            App.NowPlay = App.NowPlay > 0 ? App.NowPlay - 1 : 0;
            if (App.IsPlayTop)
            {
                this.PlayMediaList();
            } else
            {
                this.PlayMediaListUser();
            }
        }

        private void btnPlayerRewind_Click(object sender, RoutedEventArgs e)
        {
            Player.Position = TimeSpan.FromSeconds(Player.Position.TotalSeconds - 2);
        }

        private void btnPlayerPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (Player.CurrentState.ToString().Equals("Playing"))
            {
                Player.Pause();
                btnPlayerPlayPause.Content = this.PLAY;
            }
            else
            {
                Player.Play();
                btnPlayerPlayPause.Content = this.PAUSE;
            }
        }

        private void btnPlayerForward_Click(object sender, RoutedEventArgs e)
        {
            Player.Position = TimeSpan.FromSeconds(Player.Position.TotalSeconds + 2);
        }

        private void btnPlayerNext_Click(object sender, RoutedEventArgs e)
        {
            App.NowPlay = App.NowPlay < App.NumberTrack-1 ? App.NowPlay + 1 : 0;
            if (App.IsPlayTop)
            {
                this.PlayMediaList();
            }
            else
            {
                this.PlayMediaListUser();
            }
        }

        private void btnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DashBoard), meInfo);
        }
    }
}
