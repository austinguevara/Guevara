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
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // if (e.PageState != null)
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            AccessListEntryView entries = StorageApplicationPermissions.MostRecentlyUsedList.Entries;
            if (entries.Count > 0)
            {
                foreach (AccessListEntry entry in entries)
                {
                    //Get first token in most recently used items list, assign it to "mruFirstToken"
                    String mruFirstToken = StorageApplicationPermissions.MostRecentlyUsedList.Entries[entryCount].Token;

                    //Increment entry count
                    entryCount += 1;

                    //Assign the file of first token to "retrievedFile" & create new stream
                    StorageFile retrievedFile = await StorageApplicationPermissions.MostRecentlyUsedList.GetFileAsync(mruFirstToken);
                    Windows.Storage.Streams.IRandomAccessStream stream = await retrievedFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    this.DataContext = retrievedFile;

                    //Check that file is correct filetype
                    if (retrievedFile.FileType == ".bmp" ||
                        retrievedFile.FileType == ".png" ||
                        retrievedFile.FileType == ".jpeg" ||
                        retrievedFile.FileType == ".jpg" ||
                        retrievedFile.FileType == ".gif")
                    {
                        //Create new "blank" bitmap
                        Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                                new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                        //Set bitmap's source to newly created steam
                        bitmapImage.SetSource(stream);

                        //New image created and source assigned to bitmap
                        Image newImg = new Image();
                        newImg.Source = bitmapImage;
                        newImg.Height = 200;

                        //Item added to grid
                        itemGridView.Items.Add(newImg);
                    }
                }
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
