using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var password = new Windows.Security.Credentials.PasswordCredential();
                    password.UserName = "user";
                    password.Password = input;
                    password.Resource = "CM-Assistant";

                    vault.Add(password);
                    return true;
                }
                catch (Exception)
                {
                    vault.Remove(vault.FindAllByUserName("user").FirstOrDefault());
                    var password = new Windows.Security.Credentials.PasswordCredential();
                    password.UserName = "user";
                    password.Resource = "CM-Assistant";
                    password.Password = input;
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
            if (vault.FindAllByUserName("user").FirstOrDefault().Password == input)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
