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
    public sealed partial class Authentication : Page
    {
        public Authentication()
        {
            this.InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Utilities.Security.CheckPassword(Txt_Password.Password))
            {
                ((Frame)(Window.Current.Content)).Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "The password you entered was incorrect",
                    CloseButtonText = "Ok"
                };
                dialog.ShowAsync();
            }
        }

        private void Txt_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Password.Length >= 6)
            {
                Btn_Login.IsEnabled = true;
            }
            else
            {
                Btn_Login.IsEnabled = false;
            }
        }

        private void Btn_DropPasswords_Click(object sender, RoutedEventArgs e)
        {
            Classes.Utilities.Security.RemovePassword();
        }

        private void Btn_DropDatabase_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            Classes.Utilities.Security.RemovePassword();
            Classes.Utilities.StartUpChecks.ResetFirstBoot();
        }

        private void Btn_SetPassword_Click(object sender, RoutedEventArgs e)
        {
            Classes.Utilities.Security.SetPassword(Txt_NewPassword.Text);
        }
    }
}
