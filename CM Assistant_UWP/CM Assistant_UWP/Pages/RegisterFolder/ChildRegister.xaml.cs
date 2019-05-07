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
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            //Call update page once every second
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
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
                Txt_SessionTime.Text = "Currrent Session time: " + Math.Round((DateTimeOffset.Now - conn.Table<Session>()
                    .Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours, 2).ToString() + " Hours";
                Txt_SessionPay.Text = "Current projected pay: £" + Math.Round((DateTimeOffset.Now - conn.Table<Session>()
                    .Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours * conn.Table<Child>().Where(a => a.ID == ChildID).Single().Rate, 2);
                TimePick_SignInTime.Time = conn.Table<Session>().Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value.TimeOfDay;
            }
            conn.Close();
            //If there is a session open disable the sign in button and display the current running time and amount earned
        }

        private void Btn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (TimePick_SignInTime.SelectedTime != null)
            {
                Classes.Utilities.ChildAttendance.SetChildPresent(ChildID, new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignInTime.SelectedTime.GetValueOrDefault()), 
                    TimePick_SignInTime.SelectedTime.GetValueOrDefault()));
                //session.Start = new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignInTime.SelectedTime.Value), TimePick_SignInTime.SelectedTime.Value);
                (((Parent as Frame).Parent as NavigationView).Parent as Register).RefreshClients();
            }
            else
            {
                Classes.Utilities.ChildAttendance.SetChildPresent(ChildID, DateTimeOffset.Now);
                (((Parent as Frame).Parent as NavigationView).Parent as Register).RefreshClients();
            }
            (Parent as Frame).Navigate(typeof(ChildRegister), ChildID);
            //Set the start time of the session to either a custom time or the current time and then refresh the page
        }

        private void Btn_SignOut_Click(object sender, RoutedEventArgs e)
        {
            if (TimePick_SignOutTime.SelectedTime != null)
            {
                Classes.Utilities.ChildAttendance.SetChildLeft(ChildID, new DateTimeOffset(DateTime.Now.Subtract(TimePick_SignOutTime.SelectedTime.GetValueOrDefault()), 
                    TimePick_SignOutTime.SelectedTime.GetValueOrDefault()));
                (((Parent as Frame).Parent as NavigationView).Parent as Register).RefreshClients();
            }
            else
            {
                Classes.Utilities.ChildAttendance.SetChildLeft(ChildID, DateTimeOffset.Now);
                (((Parent as Frame).Parent as NavigationView).Parent as Register).RefreshClients();
            }
            (Parent as Frame).Navigate(typeof(ChildRegister), ChildID);
            //Set the end time of the session to either a custom time or the current time and then refresh the page

        }
    }
}
