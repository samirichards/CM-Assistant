using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class Database
    {
        public static SQLiteConnection GetConnection()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            return new SQLiteConnection(localFolder.Path + "\\data.db");
        }
    }
}
