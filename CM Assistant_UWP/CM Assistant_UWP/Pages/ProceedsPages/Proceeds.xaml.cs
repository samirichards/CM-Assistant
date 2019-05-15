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
using CM_Assistant_UWP.Classes;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ProceedsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Proceeds : Page
    {
        public Proceeds()
        {
            this.InitializeComponent();
            UpdatePage();
        }

        public void UpdatePage()
        {
            SQLiteConnection conn = Classes.Utilities.Database.GetConnection();
            Lst_AllTransactions.ItemsSource = conn.Table<Classes.Models.Transaction>();
        }

        private void Btn_AddExpense_Click(object sender, RoutedEventArgs e)
        {
            Frm_ProceedsDetail.Navigate(typeof(ProceedsPages.EditProceed), null, new EntranceNavigationTransitionInfo());
            //Naviate to EditProceeds without any parameters (will cause it to allow the user to add a new expense)
        }

        private void Lst_AllTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frm_ProceedsDetail.Navigate(typeof(ProceedsPages.EditProceed), (sender as Classes.Models.Transaction).ID, new EntranceNavigationTransitionInfo());
            //Naviate to the EditTransaction page and allow the user to view or change the details of the transaction
        }
    }
}
