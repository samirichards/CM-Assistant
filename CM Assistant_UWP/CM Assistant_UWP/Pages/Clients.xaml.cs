﻿using System;
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
using System.Threading.Tasks;

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

        public Task RefreshClients()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            Nav_ClientList.MenuItems.Clear();
            //Create a connection to the database, also clear the menu items before adding any new ones
            if (conn.Table<Classes.Models.Client>().Where(a => a.Deleted != true).Count() > 0)
            {
                List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();
                foreach (Classes.Models.Client item in conn.Table<Classes.Models.Client>().Where(a => a.Deleted != true))
                {
                    NavigationViewItem navigationViewItem = new NavigationViewItem
                    {
                        Content = item.Name,
                        Icon = new SymbolIcon(Symbol.Account),
                        Tag = item.ID
                    };
                    navigationViewItems.Add(navigationViewItem);
                    //Add a menu item for each client in the database that isn't deleted with a default account icon to a list
                }
                Nav_ClientList.MenuItemsSource = navigationViewItems;
                //Set the item source to the list created earlier
            }
            else
            {
                NavigationViewItemHeader header = new NavigationViewItemHeader
                {
                    Content = "No Clients to show"
                };
                Nav_ClientList.MenuItems.Add(header);
                Nav_ClientList.SelectedItem = null;
                //By default inform the user that there are no clients and ensure that nothing is selected
            }

            conn.Close();
            GC.Collect();
            return Task.FromResult(0);
            //Close the connection and call the garbage collector
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.AddClient), null, new EntranceNavigationTransitionInfo());
        }

        private void Nav_ClientList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag != null)
            {
                Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), args.InvokedItemContainer.Tag, new EntranceNavigationTransitionInfo());
            }
        }

        private void Nav_ClientList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), ((NavigationViewItem)args.SelectedItem).Tag, new EntranceNavigationTransitionInfo());
            }
        }

        private void NavItem_Refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RefreshClients();
        }
    }
}