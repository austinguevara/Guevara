using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace ImgUp.Common
{
    //abstract private 
    public static class ImageHandler
    {

        public static Windows.Storage.StorageFile File { get; set; }

        public static void Initialize(Windows.Storage.StorageFile file)
        {
            File = file;
        }

        public async static Task<Windows.UI.Xaml.Media.Imaging.BitmapImage> ImageToBitmap()
        {
            // Open a stream for the selected file.
            Windows.Storage.Streams.IRandomAccessStream fileStream =
                await File.OpenAsync(Windows.Storage.FileAccessMode.Read);

            // Set the image source to the selected bitmap.
            Windows.UI.Xaml.Media.Imaging.BitmapImage newBitmapImage =
                new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            newBitmapImage.SetSource(fileStream);

            //Return the newly created bitmap image.
            return newBitmapImage;
        }

        public static void AddImageToFAL()
        {
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(File);
        }

        public async static Task<Windows.UI.Xaml.Media.Imaging.BitmapImage> getImage(String falToken)
        {
            //Assign the file of first token to "retrievedFile" & create new stream
            StorageFile retrievedFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(falToken);
            Windows.Storage.Streams.IRandomAccessStream stream = await retrievedFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

            //this.DataContext = retrievedFile; >> Is this necessary?

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

                //Return the newly created bitmap image.
                return bitmapImage;
            }
            else
            {
                return null;
            }
        }
    }
}
