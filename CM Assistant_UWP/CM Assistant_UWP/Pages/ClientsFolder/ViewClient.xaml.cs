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
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewClient : Page
    {
        private int ClientID { get; set; }
        public ViewClient()
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
            //This method needs finishing
        }

        private void Lst_Children_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_EditClient_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Parent).Navigate(typeof(EditClient), ClientID, new DrillInNavigationTransitionInfo());
        }
    }
}