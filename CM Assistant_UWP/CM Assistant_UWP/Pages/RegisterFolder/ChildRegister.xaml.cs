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
using CM_Assistant_UWP.Classes.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.RegisterFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChildRegister : Page
    {
        public int ChildID { get; set; }
        public ChildRegister()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ChildID = (int)e.Parameter;
            UpdatePage();
        }

        public void UpdatePage()
        {
            SQLiteConnection conn = Classes.Utilities.Database.GetConnection();
            DataContext = conn.Table<Child>().Where(a => a.ID == ChildID).Single();
            if (conn.Table<Session>().Where(a=>a.SessionOpen == true && a.ChildID == ChildID).Count() > 0)
            {
                Btn_SignIn.IsEnabled = false;
                Btn_SignOut.IsEnabled = true;
                Txt_SessionTime.Text = "Currrent Session time: " + (DateTimeOffset.Now - conn.Table<Session>().Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours.ToString() + " Hours";
                Txt_SessionPay.Text = "Current projected pay: £" + (DateTimeOffset.Now - conn.Table<Session>().Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours * conn.Table<Child>().Where(a => a.ID == ChildID).Single().Rate;
                TimePick_SignInTime.Time = conn.Table<Session>().Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value.TimeOfDay;
            }
        }

        private void Btn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (TimePick_SignInTime.SelectedTime != null)
            {
                Classes.Utilities.ChildAttendance.SetChildPresent(ChildID, new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignInTime.SelectedTime.GetValueOrDefault()), TimePick_SignInTime.SelectedTime.GetValueOrDefault()));
                //session.Start = new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignInTime.SelectedTime.Value), TimePick_SignInTime.SelectedTime.Value);
            }
            else
            {
                Classes.Utilities.ChildAttendance.SetChildPresent(ChildID, DateTimeOffset.Now);
            }
            UpdatePage();
        }

        private void Btn_SignOut_Click(object sender, RoutedEventArgs e)
        {
            if (TimePick_SignOutTime.SelectedTime != null)
            {
                Classes.Utilities.ChildAttendance.SetChildLeft(ChildID, new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignOutTime.SelectedTime.GetValueOrDefault()), TimePick_SignOutTime.SelectedTime.GetValueOrDefault()));
            }
            else
            {
                Classes.Utilities.ChildAttendance.SetChildLeft(ChildID, DateTimeOffset.Now);
            }
            UpdatePage();
        }
    }
}
