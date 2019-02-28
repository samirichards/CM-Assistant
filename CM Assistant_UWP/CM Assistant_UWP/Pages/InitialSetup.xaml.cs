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
    public sealed partial class InitialSetup : Page
    {
        public InitialSetup()
        {
            this.InitializeComponent();
        }

        private void Txt_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Password.Length < 6)
            {
                Ico_Alert.Visibility = Visibility.Visible;
                Btn_Conf.IsEnabled = false;
            }
            if (((PasswordBox)sender).Password == Txt_ConfPassword.Password && ((PasswordBox)sender).Password.Length >= 6)
            {
                Ico_Alert.Visibility = Visibility.Collapsed;
                Btn_Conf.IsEnabled = true;
            }
            else
            {
                Ico_Alert.Visibility = Visibility.Visible;
                Btn_Conf.IsEnabled = false;
            }
        }

        private void Txt_ConfPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Password.Length < 6)
            {
                Ico_Alert.Visibility = Visibility.Visible;
                Btn_Conf.IsEnabled = false;
            }
            if (((PasswordBox)sender).Password == Txt_Password.Password && ((PasswordBox)sender).Password.Length >= 6)
            {
                Ico_Alert.Visibility = Visibility.Collapsed;
                Btn_Conf.IsEnabled = true;
            }
            else
            {
                Ico_Alert.Visibility = Visibility.Visible;
                Btn_Conf.IsEnabled = false;
            }
        }

        private void Btn_Conf_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Utilities.Security.SetPassword(Txt_Password.Password))
            {
                Classes.Utilities.StartUpChecks.SetFirstBoot();
                ((Frame)Window.Current.Content).Navigate(typeof(Authentication), null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "error",
                    Content = "There has been an error",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }
    }
}
