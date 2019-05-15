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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Proceeds : Page
    {
        public Proceeds()
        {
            this.InitializeComponent();
        }

        private void Nav_Proceeds_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItemContainer.Tag.ToString())
            {
                case "Proceeds":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.Proceeds), null, new EntranceNavigationTransitionInfo());
                    break;

                case "Projections":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.ProjectionsPage), null, new EntranceNavigationTransitionInfo());
                    break;

                case "Taxes":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.Taxes), null, new EntranceNavigationTransitionInfo());
                    break;
            }
        }

        private void Nav_Proceeds_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (args.SelectedItem.ToString())
            {
                case "Proceeds":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.Proceeds), null, new EntranceNavigationTransitionInfo());
                    break;

                case "Projections":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.ProjectionsPage), null, new EntranceNavigationTransitionInfo());
                    break;

                case "Taxes":
                    Frm_Proceeds.Navigate(typeof(ProceedsPages.Taxes), null, new EntranceNavigationTransitionInfo());
                    break;
            }
        }
    }
}
