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
            Lst_ClientList.Items.Add(temp);
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationViewItem temp = new NavigationViewItem
            {
                Content = "Test",
                Icon = new SymbolIcon(Symbol.Account)
            };
            Lst_ClientList.Items.Add(temp);
        }

        private void Lst_ClientList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frm_ClientContent.Navigate(typeof(ClientsFolder.ViewClient), null, new DrillInNavigationTransitionInfo());
        }
    }
}