using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CM_Assistant_UWP.Classes.Models;
using CM_Assistant_UWP;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class ChildAttendance
    {
        /// <summary>
        /// Used to open a session with the start time custom, advised to use the current time
        /// </summary>
        /// <param name="_ChildID">ID of the child a new session is to be opened with</param>
        /// <param name="time">The start time of the session</param>
        /// <returns></returns>
        public static int SetChildPresent(int _ChildID, DateTimeOffset time)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            Session session = new Session
            {
                ChildID = _ChildID,
                SessionOpen = true,
                Start = time,
                //Leave end null so that it can be set later
            };
            conn.Insert(session);
            conn.Commit();
            SQLiteCommand command = new SQLiteCommand(conn)
            {
                CommandText = "SELECT last_insert_rowid()"
            };
            return command.ExecuteScalar<int>();
            //Return ID of session
        }

        /// <summary>
        /// Used to close a session at a custom time, although it is adivsed to just use the current time
        /// </summary>
        /// <param name="_ChildID">ID of the child whos session is open</param>
        /// <param name="time">The time which the session end should have assigned to it</param>
        /// <returns></returns>
        public static bool SetChildLeft(int _ChildID, DateTimeOffset time)
        {
            SQLiteConnection conn = Database.GetConnection();
            if (conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Count() > 0)
            {
                Session session = conn.Table<Session>().Where(a => a.ChildID == _ChildID && a.SessionOpen == true).Single();
                session.End = time;
                session.SessionOpen = false;
                conn.Update(session);
                RecordSession(session.SessionID);
                conn.Close();
                //Record amount earned after closing the session

                return true;
                //Return true if session was closed
            }
            else
            {
                return false;
                //Return false if there was no session for that child open
            }
        }

        /// <summary>
        /// Used as a last resort when you want a particular session closed by ID
        /// </summary>
        /// <param name="SessionID">ID of session to be closed</param>
        public static void TerminateSession(int SessionID)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            conn.Table<Session>().Where(a => a.SessionOpen == true && a.SessionID == SessionID).Single().End = DateTimeOffset.Now;
            conn.Table<Session>().Where(a => a.SessionOpen == true && a.SessionID == SessionID).Single().SessionOpen = false;
            conn.Commit();
            RecordSession(SessionID);
            //Record the amount earned after closing the session
        }

        /// <summary>
        /// Creates or updates the income transaction for the given session
        /// </summary>
        /// <param name="SessionID">ID of the session to have the transaction generated/updated</param>
        public static void RecordSession(int SessionID)
        {
            double IncomeAmount;
            SQLiteConnection conn = Database.GetConnection();
            if (conn.Table<Transaction>().Where(a=> a.InSourceID == SessionID).Count() > 0)
            {
                int temp = conn.Table<Session>().Where(b => b.SessionID == SessionID).Single().ChildID;
                Child child = conn.Table<Child>().Where(a => a.ID == temp).Single();
                Session session = conn.Table<Session>().Where(a => a.SessionID == SessionID).Single();
                if (child.FixedRate)
                {
                    IncomeAmount = child.Rate;
                }
                else
                {
                    IncomeAmount = child.Rate * session.End.Value.Subtract(session.Start.Value).TotalHours;
                }
                //Set the amount earnt based on either time spent with the user or the fixed amount per session
                Transaction transaction = conn.Table<Transaction>().Where(a => a.InSourceID == SessionID).Single();

                transaction.TimeStamp = session.End;
                transaction.Amount = IncomeAmount;
                transaction.Reason = "Income from " + conn.Table<Client>().Where(a => a.ID == child.ParentID).Single().Name + " for care of " + child.Name;
                //Get transaction from the database and update its values

                conn.CreateTable<Transaction>();
                conn.Update(transaction);
                conn.Commit();
                conn.Close();
            }
            else
            {
                int temp = conn.Table<Session>().Where(b => b.SessionID == SessionID).Single().ChildID;
                Child child = conn.Table<Child>().Where(a => a.ID == temp).Single();
                Session session = conn.Table<Session>().Where(a => a.SessionID == SessionID).Single();
                if (child.FixedRate)
                {
                    IncomeAmount = child.Rate;
                }
                else
                {
                    IncomeAmount = child.Rate * session.End.Value.Subtract(session.Start.Value).TotalHours;
                }
                //Set the amount earnt based on either time spent with the user or the fixed amount per session
                Transaction transaction = new Transaction
                {
                    InSourceID = SessionID,
                    TimeStamp = session.End,
                    Amount = IncomeAmount,
                    Reason = "Income from " + conn.Table<Client>().Where(a=>a.ID == child.ParentID).Single().Name + " for care of " + child.Name
                };
                //Create new transaction object

                conn.CreateTable<Transaction>();
                conn.Insert(transaction);
                conn.Commit();
                conn.Close();
            }
        }
    }
}
