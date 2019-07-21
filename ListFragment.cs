using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace Address_Test_App
{
    //This Fragment Contains a listView Which will contain the names of the Locations we have selected from the Second Screen
    public class ListFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Inflate the listfragment layout and return the View
            View view = inflater.Inflate(Resource.Layout.listfragment, container, false);
            //Get the Reference of listView from the Inflated Layout
            ListView listView = view.FindViewById<ListView>(Resource.Id.listView1);
            //Declare the ArrayAdapter Object which will be used by the ListView
            ArrayAdapter adapter = new ArrayAdapter(Application.Context, Resource.Layout.support_simple_spinner_dropdown_item, MainActivity.list);
            listView.Adapter = adapter;
            return view;
        }
    }
}