using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class Security
    {
        public static bool SetPassword(string input)
        {
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                try
                {
                    var password = new Windows.Security.Credentials.PasswordCredential
                    {
                        UserName = "user",
                        Password = input,
                        Resource = "CM-Assistant"
                    };

                    vault.Add(password);
                    return true;
                }
                catch (Exception)
                {
                    vault.Remove(vault.FindAllByUserName("user").Single());
                    var password = new Windows.Security.Credentials.PasswordCredential
                    {
                        UserName = "user",
                        Resource = "CM-Assistant",
                        Password = input
                    };
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckPassword(string input)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            if (vault.FindAllByUserName("user").Single() != null)
            {
                Windows.Security.Credentials.PasswordCredential  temp = vault.FindAllByUserName("user").Single();
                temp.RetrievePassword();
                if (temp.Password == input)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void RemovePassword()
        {
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Remove(vault.FindAllByUserName("user").First());
            }
            catch (Exception e)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Error removing password" + Environment.NewLine + e.Message,
                    CloseButtonText = "Okay"
                };
            }
        }
    }
}
