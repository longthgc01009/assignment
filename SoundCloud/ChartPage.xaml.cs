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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SoundCloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartPage : Page
    {
        private ObservableCollection<Models.TopTrack> ListTrack { get; set; }

        private DownloadJson clientDownload { get; set; }
        private ObservableCollection<Models.Category> ListCate { get; set; }
        public ChartPage()
        {
            this.InitializeComponent();
            ListTrack = new ObservableCollection<Models.TopTrack>();
            ListCate = new ObservableCollection<Models.Category>();
            clientDownload = new DownloadJson();
            this.GetTrack();
            this.UpdateCate();
        }

        private void UpdateCate()
        {
            ListCate.Add(new Models.Category() { tag = "danceedm", title = "Dance - EDM" });
            ListCate.Add(new Models.Category() { tag = "dubstep", title = "DubStep" });
            ListCate.Add(new Models.Category() { tag = "deephouse", title = "DeepHouse" });
        }

        public async void GetTrack()
        {
            var json = await clientDownload.GetJsonTop50("danceedm");
            var TopTrack = JsonConvert.DeserializeObject<Models.Charts>(json);
            TopTrack.collection.ForEach(item => ListTrack.Add(item.track));
        }
        private void btnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            App.ListTopTrack = ListTrack.ToList();
            App.NumberTrack = ListTrack.Count;
            App.IsPlayTop = true;
            MainPage.Current.PlayMediaList();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var Track = e.ClickedItem as Models.TopTrack;
            MainPage.Current.PlayTrack(Track);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
        }
    }
}
