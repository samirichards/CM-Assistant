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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CM_Assistant_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Nav_MainNavView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in Nav_MainNavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home")
                {
                    Nav_MainNavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void Nav_MainNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                Frm_ContentFrame.Navigate(typeof(Pages.Settings));
            }
            switch (args.InvokedItem)
            {
                case "Home":
                    Frm_ContentFrame.Navigate(typeof(Pages.Home));
                    break;
                case "Times":
                    Frm_ContentFrame.Navigate(typeof(Pages.Calendar));
                    break;
                case "Clients":
                    Frm_ContentFrame.Navigate(typeof(Pages.Clients));
                    break;
                case "Children":
                    Frm_ContentFrame.Navigate(typeof(Pages.Register));
                    break;
                case "Income/Outgoings":
                    Frm_ContentFrame.Navigate(typeof(Pages.Proceeds));
                    break;
            }
        }

        private void Nav_MainNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                Frm_ContentFrame.Navigate(typeof(Pages.Settings));
            }
            NavigationViewItem item = (NavigationViewItem)args.SelectedItem;
            switch (item.Tag)
            {
                case "Home":
                    Frm_ContentFrame.Navigate(typeof(Pages.Home));
                    break;
                case "Times":
                    Frm_ContentFrame.Navigate(typeof(Pages.Calendar));
                    break;
                case "Clients":
                    Frm_ContentFrame.Navigate(typeof(Pages.Clients));
                    break;
                case "Children":
                    Frm_ContentFrame.Navigate(typeof(Pages.Register));
                    break;
                case "Proceeds":
                    Frm_ContentFrame.Navigate(typeof(Pages.Proceeds));
                    break;
            }
        }
    }
}
