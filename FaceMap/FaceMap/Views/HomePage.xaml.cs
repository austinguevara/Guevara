using Facebook.Client;
using FaceMap.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FaceMap.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private FacebookSession session;

        public HomePage()
        {
            this.InitializeComponent();
        }

        private async Task Authenticate()
        {
            string message = String.Empty;
            var newException = ""; 
            var loginSuccess = false;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_friends,read_stream");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;

                Frame.Navigate(typeof(LandingPage));
                loginSuccess = true;
            }
            catch (InvalidOperationException e)
            {
                newException = e.Message;
                loginSuccess = false;
            }
            if (!loginSuccess)
            {
                message = "Login failed! Exception details: " + newException;
                MessageDialog dialog = new MessageDialog(message);
                await dialog.ShowAsync();
            }
        }

        async private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }
    }
}
