﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.HomeFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeQuickAccessPage : Page
    {
        public int ChildID { get; set; }
        public HomeQuickAccessPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ChildID = int.Parse(e.Parameter.ToString());

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            SQLiteConnection conn = new SQLiteConnection(localFolder.Path + "\\data.db");

            var child = conn.Table<Classes.Models.Child>().Where(a => a.ID == ChildID).Single();

            if (child.Photo == null)
            {
                Img_Icon.ImageSource = new BitmapImage(new Uri("ms-appx:///CM Assistant_UWP/Assets/image.jpg"));
            }
            else
            {
                Img_Icon.ImageSource = Classes.Utilities.ImageManipulation.ImageFromByteArray(child.Photo);
            }
            DataContext = child;
        }
    }
}
