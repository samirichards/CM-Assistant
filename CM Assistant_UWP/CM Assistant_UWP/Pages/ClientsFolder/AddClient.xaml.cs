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
using Windows.UI.Xaml.Navigation;
using SQLite;
using SQLitePCL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddClient : Page
    {
        public AddClient()
        {
            this.InitializeComponent();
        }

        private void Txt_PhoneNumber_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
            //Disallow any characters that aren't classed as digits from being entered
        }

        private async void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
                //Create connection to the database
                Classes.Models.Client client = new Classes.Models.Client
                {
                    FirstName = Txt_FirstNameInput.Text,
                    LastName = Txt_LastNameInput.Text,
                    PhoneNumber = Txt_PhoneNumber.Text,
                    Postcode = Txt_Postcode.Text,
                    Address = Txt_Address.Text,
                    Notes = Txt_Notes.Text,
                    Deleted = false
                };
                //Create a new client from information entered in the form
                client.Name = client.FirstName + " " + client.LastName;
                conn.Insert(client);
                conn.Commit();
                conn.Close();
                //Insert the client to the database, then close the connection

                ((Button)sender).IsEnabled = false;
                ((Clients)((NavigationView)((Frame)Parent).Parent).Parent).RefreshClients();
                ((Frame)Parent).Content = null;
                //Disable the button to prevent multiple clicks being registered
                //Refresh the clients to show the new client in the database
                //Clear the frame holding this page
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "There are empty required fields",
                    CloseButtonText = "Okay"
                };
                await dialog.ShowAsync();
                //Inform the user that there is missing informaiton
            }
        }

        private bool IsFormValid()
        {
            bool temp = true;
            foreach (var item in Stk_Form.Children.Where(a => a.GetType() == typeof(TextBox) && ((TextBox)a).Name != "Txt_Notes"))
            {
                if (((TextBox)item).Text == string.Empty)
                {
                    temp = false;
                }
            }
            return temp;
            //Returns false if there are any empty textboxes (except for the notes box) to show that the form is invalid
        }
    }
}