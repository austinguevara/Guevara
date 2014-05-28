using ImgUp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace ImgUp
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class Gallery : Page
    {
        int entryCount = 0;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public Gallery()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private async void populateGallery(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Test");
            // if (e.PageState != null)
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]

            AccessListEntryView entries = StorageApplicationPermissions.FutureAccessList.Entries;
            if (entries.Count > 0)
            {
                foreach (AccessListEntry entry in entries)
                {
                    //New image created
                    Image newImg = new Image();
                    

                    //Use ImageHandler to retrieve image at entryCount and assign image source to bitmap
                    try
                    {
                        newImg.Source = await ImageHandler.getImage(StorageApplicationPermissions.FutureAccessList.Entries[entryCount].Token);
                        newImg.Width = 380;
                        newImg.Height = 220;
                        newImg.VerticalAlignment = VerticalAlignment.Center;
                        newImg.HorizontalAlignment = HorizontalAlignment.Center;
                        newImg.Stretch = Stretch.UniformToFill;
                        newImg.Tapped += initiatePopout;

                        //Item added to grid
                        itemGridView.Items.Add(newImg);
                    }
                    catch
                    {
                        break;
                    }

                    //Increment entry count
                    entryCount += 1;
                }
            }
        }

        private void initiatePopout(object sender, RoutedEventArgs e)
        {
            transparentBackdrop.Visibility = Visibility.Visible;
            Canvas.SetZIndex(transparentBackdrop, 1);

            //DoubleAnimation opacityAnimation = new DoubleAnimation();
            //opacityAnimation.From = 0.0;
            //opacityAnimation.To = 0.9;
            //opacityAnimation.Duration = TimeSpan.FromSeconds(0.5);
            //opacityAnimation.AutoReverse = true;
            //Storyboard.SetTargetName(opacityAnimation, "white75");

            Image srcImg = (Image)sender;
            nullImg.Source = srcImg.Source;
            this.DataContext = srcImg;

            nullImgPanel.Visibility = Visibility.Visible;
        }

        private void hidePopout(object sender, RoutedEventArgs e)
        {
            transparentBackdrop.Visibility = Visibility.Collapsed;
            Canvas.SetZIndex(transparentBackdrop, -2);
            nullImgPanel.Visibility = Visibility.Collapsed;
        }


    }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    navigationHelper.OnNavigatedTo(e);
        //}

        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    navigationHelper.OnNavigatedFrom(e);
        //}

        #endregion

}

