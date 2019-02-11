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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using SQLite;
using SQLitePCL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Clients : Page
    {
        public Clients()
        {
            this.InitializeComponent();
            RefreshClients();
        }

        public void RefreshClients()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();
            foreach (Classes.Models.Client item in conn.Table<Classes.Models.Client>())
            {
                NavigationViewItem navigationViewItem = new NavigationViewItem
                {
                    Content = item.Name,
                    Icon = new SymbolIcon(Symbol.Account),
                    Tag = item.ID
                };
                navigationViewItems.Add(navigationViewItem);
            }
            conn.Close();
            Nav_ClientList.MenuItemsSource = navigationViewItems;
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.AddClient), null, new DrillInNavigationTransitionInfo());
        }

        private void Nav_ClientList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), args.InvokedItemContainer.Tag, new DrillInNavigationTransitionInfo());
        }

        private void Nav_ClientList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), ((NavigationViewItem)args.SelectedItem).Tag, new DrillInNavigationTransitionInfo());
        }

        private void NavItem_Refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RefreshClients();
        }
    }
}