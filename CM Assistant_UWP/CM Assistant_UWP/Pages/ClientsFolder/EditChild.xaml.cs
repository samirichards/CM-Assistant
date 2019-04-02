using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditChild : Page
    {
        private Classes.Models.Child ChildTemp { get; set; }
        public int ChildID { get; set; }
        public EditChild()
        {
            this.InitializeComponent();
        }

        private void UpdatePage()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            ChildTemp = conn.Table<Classes.Models.Child>().Where(a => a.ID == ChildID).Single();
            DataContext = ChildTemp;
            if (ChildTemp.Photo != null)
            {
                Img_ChildIcon.ImageSource = Classes.Utilities.ImageManipulation.ImageFromByteArray(ChildTemp.Photo);
            }
            if (ChildTemp.FixedRate)
            {
                Txt_Rate.IsEnabled = false;
                Txt_AltRate.IsEnabled = false;
                Chk_FixedRate.IsChecked = true;
                Txt_Rate.Text = "N/A";
                Txt_AltRate.Text = "N/A";
                Txt_SessionRate.Text = ChildTemp.Rate.ToString();
            }
            else
            {
                Txt_Rate.IsEnabled = true;
                Txt_AltRate.IsEnabled = true;
                Chk_FixedRate.IsChecked = false;
                Txt_Rate.Text = ChildTemp.Rate.ToString();
                Txt_AltRate.Text = ChildTemp.AltRate.ToString();
                Txt_SessionRate.Text = "N/A";
            }
            Dtp_ChildDateOfBirth.SelectedDate = ChildTemp.DateOfBirth;

            conn.Close();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ChildID = int.Parse(e.Parameter.ToString());
            UpdatePage();
        }

        private async void Btn_EditPhoto_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                ChildTemp.Photo = await Classes.Utilities.ImageManipulation.GetBytesAsync(file);
                Img_ChildIcon.ImageSource = Classes.Utilities.ImageManipulation.ImageFromByteArray(ChildTemp.Photo);
            }
        }

        private void Btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ChildTemp.FirstName = Txt_FirstName.Text;
            ChildTemp.LastName = Txt_LastName.Text;
            ChildTemp.Name = ChildTemp.FirstName + " " + ChildTemp.LastName;
            if ((bool)Chk_FixedRate.IsChecked)
            {
                ChildTemp.FixedRate = true;
                ChildTemp.Rate = double.Parse(Txt_SessionRate.Text);
            }
            else
            {
                ChildTemp.FixedRate = false;
                ChildTemp.Rate = double.Parse(Txt_Rate.Text);
                ChildTemp.AltRate = double.Parse(Txt_AltRate.Text);
            }
            //Set rate based on if FixedRate is checked
            ChildTemp.DateOfBirth = (DateTimeOffset)Dtp_ChildDateOfBirth.SelectedDate;
            ChildTemp.Notes = Txt_Notes.Text;
            ChildTemp.MedicalNotes = Txt_MedicalNotes.Text;
            ChildTemp.DietNotes = Txt_DietNotes.Text;
            //Set notes for child

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            //Establish db connection

            conn.Update(ChildTemp);
            conn.Commit();
            conn.Close();
            (Parent as Frame).GoBack();
            //Update record in the database and then navigate back
        }

        private void Btn_DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Frame).GoBack();
        }

        private async void Btn_DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Confirm",
                Content = "Are you sure you wish to delete " + ChildTemp.Name + "?",
                CloseButtonText = "No",
                PrimaryButtonText = "Yes",
            };
            dialog.PrimaryButtonClick += (ContentDialog, args) =>
            {
                ChildTemp.Deleted = true;
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

                conn.Update(ChildTemp);
                conn.Commit();
                conn.Close();
                (Parent as Frame).GoBack();
            };
            await dialog.ShowAsync();
        }

        private void UpdateName(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            Txt_FullName.Text = Txt_FirstName.Text + " " + Txt_LastName.Text;
        }
    }
}