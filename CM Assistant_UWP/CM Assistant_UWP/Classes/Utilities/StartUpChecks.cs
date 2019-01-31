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
        public bool IsFirstboot()
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

        public void SetFirstBoot()
        {
            ApplicationData.Current.LocalSettings.Values["Loaded"] = true;
        }

        public void ResetFirstBoot()
        {
            ApplicationData.Current.LocalSettings.Values["Loaded"] = null;
        }
    }
}
