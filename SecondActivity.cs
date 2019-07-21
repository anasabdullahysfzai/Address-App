using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;

namespace Address_Test_App
{
    [Activity(Label = "SecondActivity")]
    public class SecondActivity : Activity
    {
        AutoCompleteTextView autoCompleteTextView;

        private SQLiteConnection db;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.secondactivity);

            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            db = new SQLiteConnection(System.IO.Path.Combine(folder, "Address.db"));

            autoCompleteTextView = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            //TextChangedListener which is executed when text field text is changed
            autoCompleteTextView.TextChanged += AutoCompleteTextView_TextChanged;
            //ItemClick Listener which is executed when item is clicked
            autoCompleteTextView.ItemClick += AutoCompleteTextView_ItemClick;

            //Create Table for database
            db.CreateTable<Address>();
        }

        private void AutoCompleteTextView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //This Method is executed when AutoCompleteTextView Item is clicked.
            String item = (String)e.Parent.GetItemAtPosition(e.Position);
            Address address = new Address();
            address.address = item;

            //insert Address Record to the Database
            db.Insert(address);
            //Go back to Previous Activity
            Finish();
        }

        private async void AutoCompleteTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //This is an Asynchronous method which feteches the places api and gets the Predictions 
            String url = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + e.Text + "&key=AIzaSyAITUi29oDEmxFXe47TPyzfwSOZCcBUHXI";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            List<String> predictionsList = new List<string>();
           

            if(response.IsSuccessStatusCode)
            {
                String resp = await response.Content.ReadAsStringAsync();
                RootObject root = JsonConvert.DeserializeObject<RootObject>(resp);
                foreach (Prediction prediction in root.predictions)
                {
                    predictionsList.Add(prediction.description);
                }
                Console.WriteLine(predictionsList);
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, predictionsList);
                autoCompleteTextView.Adapter = adapter;
            }

        }
    }
}