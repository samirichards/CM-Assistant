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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CM_Assistant_UWP.Classes.Utilities;
using CM_Assistant_UWP.Classes.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.HomeFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeQuickAccessPage : Page
    {
        public int ChildID { get; set; }
        public HomeQuickAccessPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ChildID = int.Parse(e.Parameter.ToString());
            UpdatePage();
        }

        public void UpdatePage()
        {
            SQLiteConnection conn = Database.GetConnection();
            var child = conn.Table<Classes.Models.Child>().Where(a => a.ID == ChildID).Single();
            if (child.Photo == null)
            {
                Img_Icon.ImageSource = new BitmapImage(new Uri("ms-appx:///CM Assistant_UWP/Assets/image.jpg"));
            }
            else
            {
                Img_Icon.ImageSource = ImageManipulation.ImageFromByteArray(child.Photo);
            }
            DataContext = child;

            if (conn.Table<Session>().Where(a=> a.ChildID == child.ID && a.SessionOpen == true).Count() > 0)
            {
                Session session = conn.Table<Session>().Where(a => a.ChildID == child.ID && a.SessionOpen == true).Single();
                Txt_SessionStartTime.Text = "In session since: " + session.Start.Value.ToLocalTime().ToString("HH:mm");
                Txt_SessionTime.Text = "Currrent Session time: " + Math.Round((DateTimeOffset.Now - conn.Table<Session>()
                   .Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours, 2).ToString() + " Hours";
                if (child.FixedRate)
                {
                    Txt_SessionIncome.Text = "Current projected pay: £" + child.Rate;
                }
                else
                {
                    Txt_SessionIncome.Text = "Current projected pay: £" + Math.Round((DateTimeOffset.Now - conn.Table<Session>()
                        .Where(a => a.SessionOpen == true && a.ChildID == ChildID).Single().Start.Value).TotalHours * conn.Table<Child>().Where(a => a.ID == ChildID).Single().Rate, 2);
                }
                Btn_SignInOrOut.Content = "Sign Out";
                Btn_SignInOrOut.Tag = "SignOut";
            }
            else
            {
                if (!child.FixedRate)
                {
                    Txt_SessionStartTime.Text = "Child hourly rate: £" + child.Rate;
                }
                else
                {
                    Txt_SessionStartTime.Text = "Child daily rate: £" + child.Rate;
                }

                Txt_SessionIncome.Text = string.Empty;
                Txt_SessionTime.Text = string.Empty;
                Btn_SignInOrOut.Content = "Sign In";
                Btn_SignInOrOut.Tag = "SignIn";
            }
        }

        private void Btn_SignInOrOut_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Tag.ToString() == "SignIn")
            {
                ChildAttendance.SetChildPresent(ChildID, DateTimeOffset.Now);
                UpdatePage();            }
            else
            {
                ChildAttendance.SetChildLeft(ChildID, DateTimeOffset.Now);
                UpdatePage();            }
        }
    }
}
