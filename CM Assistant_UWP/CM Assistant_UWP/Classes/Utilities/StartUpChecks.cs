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
        /// <summary>
        /// Checks if the bool value in LocalSettings exists (if it does then it's considered true)
        /// </summary>

        public static void SetFirstBoot() => ApplicationData.Current.LocalSettings.Values["Loaded"] = true;
        /// <summary>
        /// Sets a bool value in LocalSettings to true to show that the application has been previously launched
        /// </summary>

        public static void ResetFirstBoot() => ApplicationData.Current.LocalSettings.Values["Loaded"] = null;
        /// <summary>
        /// Resets First boot status of the application
        /// </summary>
        /// <returns></returns>

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
        /// <summary>
        /// Checks if there is a password set already
        /// </summary>
    }
}
