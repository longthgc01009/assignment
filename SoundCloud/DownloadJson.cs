using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SoundCloud
{
    public class DownloadJson
    {
        public async Task<string> GetjsonStream(string url) //Function to read from given url (Dọc Json từ url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            HttpResponseMessage v = new HttpResponseMessage();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetJsonTop50(string cate)
        {
            var url = @"https://api-v2.soundcloud.com/charts?kind=top&genre=soundcloud:genres:" + cate + "&client_id=" + App.CLIENT_ID + "&limit=50&offset=0";
            string json = await GetjsonStream(url);
            return json;
        }

        public async Task<string> GetFullTrack(string urlTrack)
        {
            var url = urlTrack + "?client_id=" + App.CLIENT_ID;
            string json = await GetjsonStream(url);
            return json;
        }
        public async Task<string> GetFullInfoMe(string AccessToken)
        {
            var url = "http://api.soundcloud.com/me?oauth_token=" + AccessToken;
            string json = await GetjsonStream(url);
            return json;
        }
        public async Task<string> GetUserPlaylist(int userid)
        {
            var url = "http://api.soundcloud.com/users/" + userid + "/playlists?client_id=" + App.CLIENT_ID;
            string json = await GetjsonStream(url);
            return json;
        }

        public async Task<string> GetUserFavorites(int userid)
        {
            var url = "http://api.soundcloud.com/users/" + userid + "/favorites?client_id=" + App.CLIENT_ID;
            string json = await GetjsonStream(url);
            return json;
        }
    }
}
