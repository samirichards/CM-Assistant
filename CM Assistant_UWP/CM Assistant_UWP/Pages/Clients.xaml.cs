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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Clients : Page
    {
        public List<NavigationViewItem> ClientList = new List<NavigationViewItem>();

        public Clients()
        {
            this.InitializeComponent();
            NavigationViewItem temp = new NavigationViewItem
            {
                Content = "Test",
                Icon = new SymbolIcon(Symbol.Account)
            };
            ClientList.Add(temp);
            NavView_ClientsList.DataContext = ClientList;
            NavView_ClientsList.MenuItemsSource = ClientList;
        }

        private void NavView_ClientsList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

        }

        private void NavView_ClientsList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        private void Btn_AddClient_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationViewItem temp = new NavigationViewItem
            {
                Content = "Test",
                Icon = new SymbolIcon(Symbol.Account)
            };
            NavView_ClientsList.MenuItemsSource = ClientList;
            ContentDialog content = new ContentDialog
            {
                Content = "Test",
                CloseButtonText = "Alright then famalam"
            };
        }
    }
}
