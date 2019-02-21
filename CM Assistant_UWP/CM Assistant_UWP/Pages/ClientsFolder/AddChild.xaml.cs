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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// Page which handles the addition of a new child to the db which is associated with a parent
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
                if ((bool)Chk_FixedSessionModifier.IsChecked)
                {
                    ChildTemp.FixedRate = true;
                    ChildTemp.Rate = double.Parse(Txt_FixedRate.Text);
                }
                else
                {
                    ChildTemp.FixedRate = false;
                    ChildTemp.Rate = double.Parse(Txt_Rate.Text);
                    ChildTemp.AltRate = double.Parse(Txt_AltRate.Text);
                }
                ChildTemp.DateOfBirth = DateP_DOB_Input.SelectedDate.Value;
                ChildTemp.DietNotes = Txt_DietNotes.Text;
                ChildTemp.MedicalNotes = Txt_MedicalNotes.Text;
                ChildTemp.Notes = Txt_Notes.Text;
                ChildTemp.Age = Math.Floor(DateTimeOffset.Now.Subtract(ChildTemp.DateOfBirth).TotalDays / 365);

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
                conn.Insert(ChildTemp);
                conn.Commit();
                ((Button)sender).IsEnabled = false;
                ((Frame)Parent).Navigate(typeof(ViewClient), ClientID.ToString(), new DrillInNavigationTransitionInfo());
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = ("Successfully added " + ChildTemp.Name + " to database as a child of " + conn.Table<Classes.Models.Client>().Where(a => a.ID == ClientID).Single().Name),
                    CloseButtonText = "Okay"
                };
                dialog.ShowAsync();
                foreach (var item in Stk_Form.Children.Where(a => a.GetType() == typeof(TextBox)))
                {
                    ((TextBox)item).Text = string.Empty;
                }

            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "There are empty required fields",
                    CloseButtonText = "Okay"
                };
                dialog.ShowAsync();
            }
        }

        private async void Btn_EditPhoto_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                ChildTemp.Photo = await GetBytesAsync(file);
                UpdateContext();
            }
        }

        private bool IsFormValid()
        {
            if (!(bool)Chk_FixedSessionModifier.IsChecked)
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
            else
            {
                if (Txt_FirstNameInput.Text == string.Empty || Txt_LastNameInput.Text == string.Empty || Txt_FixedRate.Text == string.Empty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ClientID = int.Parse(e.Parameter.ToString());
            ChildTemp.ParentID = ClientID;
        }

        private void UpdateContext()
        {
            DataContext = ChildTemp;
            Img_ChildPhoto.Source = Classes.Utilities.ImageManipulation.ImageFromByteArray(ChildTemp.Photo);
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

        private void Chk_FixedSessionModifier_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((CheckBox)sender).IsChecked)
            {
                Txt_Rate.IsEnabled = false;
                Txt_AltRate.IsEnabled = false;
                Txt_FixedRate.IsEnabled = true;
            }
            else
            {
                Txt_Rate.IsEnabled = true;
                Txt_AltRate.IsEnabled = true;
                Txt_FixedRate.IsEnabled = false;
            }
        }
    }
}