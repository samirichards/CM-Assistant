﻿using System;
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
using Windows.UI.Xaml.Navigation;
using SQLite;
using System.Xml;
using System.Xml.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CM_Assistant_UWP.Pages.ProceedsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Taxes : Page
    {
        public Taxes()
        {
            this.InitializeComponent();
            UpdatePage();
        }

        public void UpdatePage()
        {
            double amountToPay;
            SQLiteConnection conn = Classes.Utilities.Database.GetConnection();
            Txt_TotalRevenue.Text = "Total Revenue: £" + Math.Round(conn.Table<Classes.Models.Transaction>().Sum(a => a.Amount), 2, MidpointRounding.AwayFromZero);

            XDocument document = XDocument.Load(@"Data\\TaxRates.xml");
            foreach (var item in document.Root.Nodes().Where(a=> a.ToString() == "TaxYear"))
            {

            }
        }
    }
}
