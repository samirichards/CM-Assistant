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
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
                Classes.Models.Client client = new Classes.Models.Client
                {
                    FirstName = Txt_FirstNameInput.Text,
                    LastName = Txt_LastNameInput.Text,
                    PhoneNumber = Txt_PhoneNumber.Text,
                    Postcode = Txt_Postcode.Text,
                    Address = Txt_Address.Text,
                    Notes = Txt_Notes.Text,
                };
                client.Name = client.FirstName + " " + client.LastName;
                conn.Insert(client);
                conn.Commit();
                conn.Close();

                ((Button)sender).IsEnabled = false;
                ((Clients)((NavigationView)((Frame)Parent).Parent).Parent).RefreshClients();
                ((Frame)Parent).Content = null;
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
        }
    }
}