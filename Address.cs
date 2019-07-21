using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Address_Test_App
{
    //Address Class is used by SQLite Database to create the table and store data
    class Address
    {
        [PrimaryKey, AutoIncrement]
        public int id
        {
            get;
            set;
        }

        public string address
        {
            get;
            set;
        }
    }
}