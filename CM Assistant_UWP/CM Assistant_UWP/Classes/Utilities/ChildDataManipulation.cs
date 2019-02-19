using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class ChildDataManipulation
    {
        public static void UpdateAges()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            foreach (var item in conn.Table<Models.Child>())
            {
                item.Age = Math.Floor(DateTimeOffset.Now.Subtract(item.DateOfBirth).TotalDays / 365);
            }
            conn.Commit();
        }
    }
}
