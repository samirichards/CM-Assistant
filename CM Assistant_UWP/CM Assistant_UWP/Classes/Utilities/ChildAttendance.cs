using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CM_Assistant_UWP.Classes.Models;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class ChildAttendance
    {
        public int SetChildPresent(int _ChildID, DateTimeOffset time)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            Session session = new Session
            {
                ChildID = _ChildID,
                SessionOpen = true,
                Start = time,
                End = null
            };
            conn.Insert(session);
            conn.Commit();
            SQLiteCommand command = new SQLiteCommand(conn);
            command.CommandText = "SELECT last_insert_rowid()";
            return command.ExecuteScalar<int>();
        }

        public void SetChildLeft(int _ChildID, DateTimeOffset time)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Single().End = time;
            conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Single().SessionOpen = false;
            conn.Commit();
        }
    }
}
