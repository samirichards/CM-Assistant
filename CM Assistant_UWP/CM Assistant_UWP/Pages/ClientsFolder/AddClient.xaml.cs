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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ClientsFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddClient : Page
    {
        public AddClient()
        {
            this.InitializeComponent();
        }

        private void Txt_PhoneNumber_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = IsFormValid().ToString();
        }

        private bool IsFormValid()
        {
            foreach (var item in Stk_Form.Children.Where(a => a.GetType() == typeof(TextBox)))
            {
                if (((TextBox)item).Text == string.Empty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}