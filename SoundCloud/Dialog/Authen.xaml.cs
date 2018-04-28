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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SoundCloud.Dialog
{
    public sealed partial class Authen : ContentDialog
    {
        public Authen()
        {
            this.InitializeComponent();
            this.BtnLogin_Click();
            this.Margin = new Thickness(0);
        }

        private void WebViewLogin_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (sender.Source.ToString().Contains("callback?code="))
            {
                string[] a = sender.Source.ToString().Replace("http://www.favesound.de/callback?", "").Split('&');
                foreach (string code in a)
                {
                    if (code.Contains("code="))
                    {
                        App.Code = code.Replace("code=", "");
                    }
                    else if (code.Contains("access_token="))
                    {
                        string[] b = code.Split('#');
                        App.AccessToken = b[1].Replace("access_token=", "");
                    }
                }
                App.Login = true;
                this.Hide();
            } else
            {

            }
        }

        private void BtnLogin_Click()
        {
            var url = App.ENPOINT_CONNECT + "?client_id=" + App.CLIENT_ID + "&display=popup&redirect_uri=" + App.REDIRECT_URL + "&response_type=code_and_token&scope=non-expiring&state=SoundCloud_Dialog_9ec50";
            WebViewLogin.Navigate(new Uri(url));
        }
    }
}
