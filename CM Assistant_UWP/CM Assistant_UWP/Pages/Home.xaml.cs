using SQLite;
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
        public object LastClicked { get; set; }
        public Home()
        {
            this.InitializeComponent();

            RefreshChildList();
        }

        public void RefreshChildList()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            List<Classes.Viewmodels.ChildQuickAccess> childQuickAccesses = new List<Classes.Viewmodels.ChildQuickAccess>();

            bool ChildrenPresent = false;
            foreach (var client in conn.Table<Classes.Models.Client>().Where(a=> a.Deleted != true))
            {
                foreach (var child in conn.Table<Classes.Models.Child>().Where(a=> a.ParentID == client.ID && a.Deleted != true))
                {
                    Classes.Viewmodels.ChildQuickAccess quickAccess = new Classes.Viewmodels.ChildQuickAccess
                    {
                        ChildID = child.ID,
                        Name = child.Name
                    };
                    if (child.Photo == null)
                    {
                        quickAccess.Icon = new BitmapImage(new Uri(BaseUri, "/Assets/acc.png"));
                    }
                    else
                    {
                        quickAccess.Icon = Classes.Utilities.ImageManipulation.ImageFromByteArray(child.Photo);
                    }

                    childQuickAccesses.Add(quickAccess);
                    ChildrenPresent = true;
                }
            }

            if (ChildrenPresent)
            {
                GrdView_Test.ItemsSource = childQuickAccesses;
                GrdView_Test.DataContext = childQuickAccesses;
                GC.Collect();
            }
            else
            {
                Grd_NoChildren.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExitStoryboard.Begin();
            ExitStoryboard.Completed += ExitStoryboard_Completed;
        }

        private void ExitStoryboard_Completed(object sender, object e)
        {
            Overlay.Visibility = Visibility.Collapsed;
            Frm_QuickAccessFrame.Content = null;
            GC.Collect();
        }

        private void GrdView_Test_ItemClick(object sender, ItemClickEventArgs e)
        {
            Overlay.Visibility = Visibility.Visible;
            LastClicked = e.ClickedItem;
            GrdView_Test.PrepareConnectedAnimation("child", e.ClickedItem, "Stk_ChildPanel");
            EnterStoryboard.Begin();
            ConnectedAnimation anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("child");
            Frm_QuickAccessFrame.Navigate(typeof(HomeFolder.HomeQuickAccessPage), ((Classes.Viewmodels.ChildQuickAccess)e.ClickedItem).ChildID, new SuppressNavigationTransitionInfo());
            anim.Configuration = new DirectConnectedAnimationConfiguration();
            anim.TryStart(Grd_ChildDetails);
        }
    }
}
