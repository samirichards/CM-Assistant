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

            NavigationViewItem temp = new NavigationViewItem
            {
                Content = "Test",
                Icon = new SymbolIcon(Symbol.Account)
            };
            Nav_ClientList.MenuItems.Add(temp);
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.AddClient), null, new DrillInNavigationTransitionInfo());
        }

        private void Nav_ClientList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), null, new DrillInNavigationTransitionInfo());
        }

        private void Nav_ClientList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), null, new DrillInNavigationTransitionInfo());
        }
    }
}