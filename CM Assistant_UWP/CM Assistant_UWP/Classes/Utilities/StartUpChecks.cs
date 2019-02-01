using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class StartUpChecks
    {
        public static bool IsFirstboot()
        {
            if (ApplicationData.Current.LocalSettings.Values["Loaded"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void SetFirstBoot()
        {
            ApplicationData.Current.LocalSettings.Values["Loaded"] = true;
        }

        public static void ResetFirstBoot()
        {
            ApplicationData.Current.LocalSettings.Values["Loaded"] = null;
        }

        public static bool IsPasswordSet()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            try
            {
                if (vault.FindAllByUserName("user").Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
