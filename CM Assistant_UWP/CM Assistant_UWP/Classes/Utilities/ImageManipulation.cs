using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class ImageManipulation
    {
        public static BitmapImage ImageFromByteArray(byte[] array)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(array);
#pragma warning disable CS4014
                    writer.StoreAsync();
#pragma warning restore CS4014
                }
                BitmapImage image = new BitmapImage();
#pragma warning disable CS4014
                image.SetSourceAsync(stream);
#pragma warning restore CS4014
                return image;
            }
        }

        public static async Task<byte[]> GetBytesAsync(StorageFile file)
        {
            byte[] fileBytes = null;
            if (file == null) return null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }
    }
}
