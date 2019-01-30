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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();

            List<Classes.Viewmodels.ChildQuickAccess> childQuickAccesses = new List<Classes.Viewmodels.ChildQuickAccess>();
            for (int i = 0; i < 10; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(BaseUri, "/Assets/acc.png"));
                childQuickAccesses.Add(new Classes.Viewmodels.ChildQuickAccess { Name = "Child " + i.ToString(), Icon = img });
            }

            TestListView.ItemsSource = childQuickAccesses;
            TestListView.DataContext = childQuickAccesses;
        }

        private void TestListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Overlay.Visibility = Visibility.Visible;
            EnterStoryboard.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExitStoryboard.Begin();
            ExitStoryboard.Completed += ExitStoryboard_Completed;
        }

        private void ExitStoryboard_Completed(object sender, object e)
        {
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}
