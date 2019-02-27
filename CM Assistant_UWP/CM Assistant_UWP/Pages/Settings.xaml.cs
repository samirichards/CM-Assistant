using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            RefreshDeletedClients();
            RefreshDeletedChildren();
        }

        private void Rdo_Theme_ButtonChecked(object sender, RoutedEventArgs e)
        {

        }

        public void RefreshDeletedClients()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            Lst_DeletedClients.ItemsSource = null;
            List<Classes.Viewmodels.DeletedClient> deletedClients = new List<Classes.Viewmodels.DeletedClient>();

            foreach (var item in conn.Table<Classes.Models.Client>().Where(a=> a.Deleted == true))
            {
                Classes.Viewmodels.DeletedClient deletedClient = new Classes.Viewmodels.DeletedClient
                {
                    ParentName = item.Name,
                    ParentID = item.ID
                };
                foreach (var item2 in conn.Table<Classes.Models.Child>().Where(a=> a.ParentID == item.ID))
                {
                    deletedClient.Children += item2.Name + Environment.NewLine;
                }
                deletedClients.Add(deletedClient);
                Lst_DeletedClients.ItemsSource = deletedClients;
                Lst_DeletedClients.DataContext = deletedClients;
            }
            conn.Close();
            GC.Collect();
        }

        public void RefreshDeletedChildren()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");
            Lst_DeletedClients.ItemsSource = null;
            List<Classes.Viewmodels.DeletedChild> deletedChildren = new List<Classes.Viewmodels.DeletedChild>();

            foreach (var item in conn.Table<Classes.Models.Child>().Where(a => a.Deleted == true))
            {
                Classes.Viewmodels.DeletedChild deletedChild = new Classes.Viewmodels.DeletedChild
                {
                    ParentName = conn.Table<Classes.Models.Client>().Where(a=> a.ID == item.ParentID).Single().Name,
                    ChildID = item.ID,
                    Name = item.Name
                };
                deletedChildren.Add(deletedChild);
            }
            Lst_DeletedChildren.ItemsSource = deletedChildren;
            conn.Close();
            GC.Collect();
        }

        private void Btn_RecoverDeletedClients_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            foreach (Classes.Viewmodels.DeletedClient item in Lst_DeletedClients.SelectedItems)
            {
                var temp = conn.Table<Classes.Models.Client>().Where(a => a.ID == item.ParentID).Single();
                temp.Deleted = false;
                conn.Update(temp);
                conn.Commit();
            }
            conn.Close();
            GC.Collect();
            RefreshDeletedClients();
        }

        private void Btn_ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Utilities.Security.CheckPassword(Txt_OldPassword.Password))
            {
                if (!(Txt_NewPassword.Password.Length < 6))
                {
                    if (Txt_Conf_NewPassword.Password == Txt_NewPassword.Password)
                    {
                        Classes.Utilities.Security.SetPassword(Txt_NewPassword.Password);
                        Txt_OldPassword.Password = string.Empty;
                        Txt_NewPassword.Password = string.Empty;
                        Txt_Conf_NewPassword.Password = string.Empty;
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "Success",
                            Content = "Successfully set new password, you may now log in with the new password",
                            CloseButtonText = "Okay"
                        };
                        dialog.ShowAsync();
                    }
                    else
                    {
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "New passwords did not match, please re-enter them correctly",
                            CloseButtonText = "Okay"
                        };
                        dialog.ShowAsync();
                    }
                }
                else
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "You new password must be at least 6 characters long",
                        CloseButtonText = "Okay"
                    };
                    dialog.ShowAsync();
                }
                
            }
            else
            {
                Txt_OldPassword.Password = string.Empty;
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Incorrect password, Please enter your current password in the current password box",
                    CloseButtonText = "Okay"
                };
                dialog.ShowAsync();
            }
        }

        private void Btn_RecoverDeletedChildren_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            foreach (Classes.Viewmodels.DeletedChild item in Lst_DeletedChildren.SelectedItems)
            {
                var temp = conn.Table<Classes.Models.Child>().Where(a => a.ID == item.ChildID).Single();
                temp.Deleted = false;
                conn.Update(temp);
                conn.Commit();
            }
            conn.Close();
            GC.Collect();
            RefreshDeletedChildren();
        }
    }
}
