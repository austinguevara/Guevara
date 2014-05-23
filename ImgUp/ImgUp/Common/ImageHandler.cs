using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgUp.Common
{
    //abstract private 
    public static class ImageHandler
    {
        //TODO: Declare class variables/etc here

        public async static Task<Windows.UI.Xaml.Media.Imaging.BitmapImage> imageToBitmap(Windows.Storage.StorageFile addFile)
        {
            // Open a stream for the selected file.
            Windows.Storage.Streams.IRandomAccessStream fileStream =
                await addFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

            // Set the image source to the selected bitmap.
            Windows.UI.Xaml.Media.Imaging.BitmapImage newBitmapImage = 
                new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            newBitmapImage.SetSource(fileStream);

            //Return the newly created bitmap image
            return newBitmapImage;
        }

        public static void addImageToFAL(Windows.Storage.StorageFile addFile)
        {
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(addFile);
        }

    }
}
