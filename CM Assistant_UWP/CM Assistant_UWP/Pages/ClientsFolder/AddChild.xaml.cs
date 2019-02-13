using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddChild : Page
    {
        public int ClientID { get; set; }
        private Classes.Models.Child ChildTemp = new Classes.Models.Child();
        public AddChild()
        {
            InitializeComponent();
            DateP_DOB_Input.MaxYear = DateTimeOffset.Now;
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                ChildTemp.FirstName = Txt_FirstNameInput.Text;
                ChildTemp.LastName = Txt_LastNameInput.Text;
                ChildTemp.Name = ChildTemp.FirstName + " " + ChildTemp.LastName;
                ChildTemp.Rate = int.Parse(Txt_Rate.Text);
                ChildTemp.AltRate = int.Parse(Txt_AltRate.Text);
                ChildTemp.DateOfBirth = DateP_DOB_Input.SelectedDate.Value;
                ChildTemp.DietNotes = Txt_DietNotes.Text;
                ChildTemp.MedicalNotes = Txt_MedicalNotes.Text;
                ChildTemp.Notes = Txt_Notes.Text;
                ChildTemp.Age = DateTimeOffset.Now.Subtract(ChildTemp.DateOfBirth).TotalDays / 365;

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
                conn.Insert(ChildTemp);
                conn.Commit();
                ContentDialog dialog = new ContentDialog();
                dialog.Title = "Success";
                dialog.Content = ("Successfully added {0} to database as {1}'s child", ChildTemp.Name, conn.Table<Classes.Models.Client>().Where(a => a.ID == ClientID).Single().Name);
                dialog.CloseButtonText = "Okay";
                dialog.ShowAsync();
                foreach (var item in Stk_Form.Children.Where(a => a.GetType() == typeof(TextBox)))
                {
                    ((TextBox)item).Text = string.Empty;
                }

            }
            else
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = "Error";
                dialog.Content = "There are empty required fields";
                dialog.CloseButtonText = "Okay";
                dialog.ShowAsync();
            }
        }

        private async void Btn_EditPhoto_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                ChildTemp.Photo = await GetBytesAsync(file);
                await UpdateContextAsync();
            }
        }

        private bool IsFormValid()
        {
            if (Txt_FirstNameInput.Text == string.Empty || Txt_LastNameInput.Text == string.Empty || Txt_Rate.Text == string.Empty || Txt_AltRate.Text == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ClientID = int.Parse(e.Parameter.ToString());
            ChildTemp.ParentID = ClientID;
        }

        private async Task UpdateContextAsync()
        {
            DataContext = ChildTemp;
            BitmapImage biSource = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(ChildTemp.Photo.AsBuffer());
                stream.Seek(0);
                await biSource.SetSourceAsync(stream);
            }

            Img_ChildPhoto.Source = biSource;
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
