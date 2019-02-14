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
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
            RefreshClients();
        }

        private void Nav_RegList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Frm_ChildContent.Navigate(typeof(RegisterFolder.ChildRegister), args.InvokedItemContainer.Tag, new DrillInNavigationTransitionInfo());
        }

        private void Nav_RegList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frm_ChildContent.Navigate(typeof(RegisterFolder.ChildRegister), ((NavigationViewItem)args.SelectedItem).Tag, new DrillInNavigationTransitionInfo());
        }

        private void NavItem_Refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RefreshClients();
        }

        public void RefreshClients()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            Nav_RegList.MenuItems.Clear();
            List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();

            if (conn.Table<Classes.Models.Session>().Select(a => a.SessionOpen).Contains(true))
            {
                NavigationViewItemHeader header = new NavigationViewItemHeader
                {
                    Content = "Open sessions"
                };
                Nav_RegList.MenuItems.Add(header);
                foreach (Classes.Models.Session item in conn.Table<Classes.Models.Session>().Where(a=> a.SessionOpen == true))
                {
                    Nav_RegList.MenuItems.Add(conn.Table<Classes.Models.Child>().Where(a => a.ID == item.ChildID).Single());
                }
            }

            SQLiteCommand command = new SQLiteCommand(conn);
            command.CommandText = "SELECT MAX(ParentID) FROM Child";
            for (int i = 1; i <= int.Parse(command.ExecuteScalar<string>()); i++)
            {
                NavigationViewItemHeader header = new NavigationViewItemHeader
                {
                    Content = conn.Table<Classes.Models.Client>().Where(a => a.ID == i).Single().Name
                };
                Nav_RegList.MenuItems.Add(header);
                foreach (Classes.Models.Child item in conn.Table<Classes.Models.Child>().Where(a=> a.ParentID == i))
                {
                    NavigationViewItem navigationViewItem = new NavigationViewItem
                    {
                        Content = item.Name,
                        Icon = new SymbolIcon(Symbol.Account),
                        Tag = item.ID
                    };
                    Nav_RegList.MenuItems.Add(navigationViewItem);
                }
            }
            conn.Close();
        }
    }
}