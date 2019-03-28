using SQLite;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewChild : Page
    {
        private Classes.Models.Child ChildTemp { get; set; }
        public ViewChild()
        {
            this.InitializeComponent();
        }

        public void RefreshChild()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            DataContext = ChildTemp;
            if (ChildTemp.Photo != null)
            {
                Img_ChildIcon.ImageSource = Classes.Utilities.ImageManipulation.ImageFromByteArray(ChildTemp.Photo);
            }
            //Set the photo to the one stored in the table
            //If there isn't a photo then leave it as the default value (blank)
            Txt_ParentName.Text = "Child of: " + conn.Table<Classes.Models.Client>().Where(a => a.ID == ChildTemp.ParentID).Single().Name;

            if (ChildTemp.FixedRate == true)
            {
                Txt_Rate.Text = "N/A";
                Txt_AltRate.Text = "N/A";
                Txt_SessionRate.Text = "Session Rate: " + ChildTemp.Rate;
                Chk_FixedRate.IsChecked = true;
            }
            //Special case to override the binding in the event that the pay from this child is calculated at a fixed rate per day
            Dtp_ChildDateOfBirth.SelectedDate = ChildTemp.DateOfBirth;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            int childID = int.Parse(e.Parameter.ToString());

            ChildTemp = conn.Table<Classes.Models.Child>().Where(a => a.ID == childID).Single();
            conn.Close();
            RefreshChild();
        }

        private void Btn_CloseViewChild_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Parent).GoBack();
        }
    }
}