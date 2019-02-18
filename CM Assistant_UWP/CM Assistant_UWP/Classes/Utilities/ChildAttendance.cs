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
        //Used to open a session with the start time custom, but usually is the current time
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

        //Used to close a session at a custom time, usually the current time but it can be different
        public void SetChildLeft(int _ChildID, DateTimeOffset time)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Single().End = time;
            conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Single().SessionOpen = false;
            conn.Commit();
        }

        //Used as a last resort when you want a particular session closed by ID
        public void TerminateSession(int SessionID)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            conn.Table<Session>().Where(a => a.SessionOpen == true && a.SessionID == SessionID).Single().End = DateTimeOffset.Now;
            conn.Table<Session>().Where(a => a.SessionOpen == true && a.SessionID == SessionID).Single().SessionOpen = false;
            conn.Commit();
        }
    }
}
