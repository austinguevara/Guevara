using Facebook;
using Facebook.Client;
using FaceMap.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FaceMap.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        public LandingPage()
        {
            this.InitializeComponent();
            LoadUserInfo();
        }

        private async void LoadUserInfo()
        {
            FacebookClient fb = new FacebookClient(App.AccessToken);

            dynamic parameters = new ExpandoObject();
            parameters.access_token = App.AccessToken;
            parameters.fields = "name";

            dynamic result = await fb.GetTaskAsync("me", parameters);

            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", App.FacebookId, "large", fb.AccessToken);

            this.MyImage.Source = new BitmapImage(new Uri(profilePictureUrl));
            this.MyName.Text = result.name;
        }

        async private void selectFriendsTextBox_Tapped(object sender, TappedRoutedEventArgs evtArgs)
        {
            System.Diagnostics.Debug.WriteLine("Select friends textbox has been tapped.");
            FacebookClient fb = new FacebookClient(App.AccessToken);

            dynamic friendsTaskResult = await fb.GetTaskAsync("/me/friends");
            var result = (IDictionary<string, object>)friendsTaskResult;

            //Write out resultant result variable
            System.Diagnostics.Debug.WriteLine("Result: " + result);

            var data = (IEnumerable<object>)result["data"];

            //Write out resultant data variable
            System.Diagnostics.Debug.WriteLine("Data: " + data);

            var newCount = 50;
            while (newCount > 0)
            {
                foreach (var item in data)
                {
                    var friend = (IDictionary<string, object>)item;

                    FacebookData.Friends.Add(new Friend { Name = (string)friend["name"], id = (string)friend["id"], PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", (string)friend["id"], "square", App.AccessToken)) });
                    System.Diagnostics.Debug.WriteLine(FacebookData.Friends);
                }

                newCount -= 1;
                System.Diagnostics.Debug.WriteLine(newCount);
            }

            Frame.Navigate(typeof(FriendSelector));
        }
    }
}
