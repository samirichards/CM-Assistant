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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditClient : Page
    {
        string childList;
        public int ClientID { get; set; }
        public EditClient()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ClientID = int.Parse(e.Parameter.ToString());
            UpdatePage();
        }

        public void UpdatePage()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            DataContext = conn.Table<Classes.Models.Client>().Where(a => a.ID == ClientID).Single();
            Lst_Children.ItemsSource = conn.Table<Classes.Models.Child>().Where(a => a.ParentID == ClientID);
        }

        private void Btn_AddChild_Click(object sender, RoutedEventArgs e)
        {
            Grd_AddChildOverlay.Visibility = Visibility.Visible;
            Frm_AddChildUI.Navigate(typeof(AddChild), ClientID, new DrillInNavigationTransitionInfo());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frm_AddChildUI.Content = null;
            Grd_AddChildOverlay.Visibility = Visibility.Collapsed;

        }

        private void Lst_Children_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            Classes.Models.Client client = conn.Table<Classes.Models.Client>().Where(a => a.ID == ClientID).Single();
            client.FirstName = Txt_FirstName.Text;
            client.LastName = Txt_LastName.Text;
            client.Name = client.FirstName + " " + client.LastName;
            client.PhoneNumber = Txt_PhoneNumber.Text;
            client.Postcode = Txt_Postcode.Text;
            client.Notes = Txt_Notes.Text;

            conn.Update(client);
            conn.Commit();
            conn.Close();

            ((Frame)Parent).Navigate(typeof(ViewClient), ClientID, new DrillInNavigationTransitionInfo());
        }

        private void Btn_DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Parent).GoBack();
        }

        private void Btn_DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            Classes.Models.Client client = conn.Table<Classes.Models.Client>().Where(a => a.ID == ClientID).Single();

            ContentDialog dialog = new ContentDialog
            {
                Title = "Confirm"
            };
            childList = null;
            if (conn.Table<Classes.Models.Child>().Where(a => a.ParentID == ClientID).Count() > 0)
            {
                foreach (var item in conn.Table<Classes.Models.Child>().Where(a => a.ParentID == ClientID && a.Deleted != true))
                {
                    childList = childList + item.Name + Environment.NewLine;
                }
                dialog.Content = "Are you sure you want to delete " + client.Name + "?" + Environment.NewLine + "The following children will also be removed:" + Environment.NewLine + childList;
            }
            else
            {
                dialog.Content = "Are you sure you want to delete " + client.Name + "?";
            }
            dialog.PrimaryButtonText = "Yes";
            dialog.IsPrimaryButtonEnabled = true;
            dialog.PrimaryButtonClick += (ContentDialog, args) =>
            {
                client.Deleted = true;
                conn.Update(client);
                conn.Commit();
                ((Clients)((NavigationView)((Frame)Parent).Parent).Parent).RefreshClients();
                ((Frame)Parent).Content = null;
            };
            dialog.CloseButtonText = "No";
            dialog.ShowAsync();
        }

        private void Btn_EditChild_Click(object sender, RoutedEventArgs e)
        {
            if (Lst_Children.SelectedItem != null)
            {
                ((Frame)Parent).Navigate(typeof(EditChild), (Lst_Children.SelectedItem as Classes.Models.Child).ID, new DrillInNavigationTransitionInfo());
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select a child to edit",
                    CloseButtonText = "Okay"
                };
                dialog.ShowAsync();
            }
        }
    }
}