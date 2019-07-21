using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using SQLite;
using Android.Support.Design.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Address_Test_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity,IOnMapReadyCallback
    {
        private SQLiteConnection db;//This object is used for returning the addresses from the Database
        FrameLayout container;//This object is used as a container for the fragments
        public static ArrayList list;//This list contains the addresses returned by db and is used by ArrayAdapter in ListFragment Class to populate the ListView with Addresses

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            db = new SQLiteConnection(System.IO.Path.Combine(folder, "Address.db"));

            FloatingActionButton button = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButton1);
            button.Click += AddAddressClicked;
            
            //Get the Reference of the 
            BottomNavigationView bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationView1);

            //BottomNavigationView NavigationItemSelected Listener
            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

            //Get the Reference of the framelayout and store it in container variable of type FrameLayout
            container = FindViewById<FrameLayout>(Resource.Id.frameLayout1);

            //Load the ListFragment into the FrameLayout
            SetAddressList();
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frameLayout1, new ListFragment()).Commit();
        }

        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            //This Method is executed when user selectes any item from the BottomNavigationView
            switch(e.Item.ItemId)
            {
                case Resource.Id.list:
 
                    SetAddressList();
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frameLayout1, new ListFragment()).Commit();
                    break;
                case Resource.Id.map:
                    var map = SupportMapFragment.NewInstance();
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frameLayout1, map,"map").Commit();
                    map.GetMapAsync(this);
                    break;
            }
        }

        public void SetAddressList()
        {
            //This Method gets the location list from the Database and add it to the static ArrayList which is then used by ArrayAdapter to Display Elements
            list = new ArrayList();
            //Get the 
            foreach (var address in db.Table<Address>())
            {
                list.Add(address.address);
            }
        }

        private void AddAddressClicked(object sender, System.EventArgs e)
        {
            //This Method is executed when AddAddress Button is Clicked and It takes us from Activity 1 to Second Activity
            Intent intent = new Intent(this, typeof(SecondActivity));
            StartActivity(intent);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            //This Callback is executed when map is ready.It has an argument of type GoogleMap which can be used to perform different fuctions over the Map such as Adding Markers,Overlaying etc
            googleMap.MapType = GoogleMap.MapTypeNormal;
            foreach (var address in db.Table<Address>())
            {
                LatLng point = getLatLngFromName(address.address);
                MarkerOptions opt = new MarkerOptions();
                opt.SetPosition(point);
                opt.SetTitle(address.address);

                googleMap.AddMarker(opt);
            }
        }

        public LatLng getLatLngFromName(string name)
        {
            //This Method takes a Location name as an argument and returns the LatLng Object of the Location
            LatLng geoPoint;

            Geocoder geocoder = new Geocoder(this);

            IList<Android.Locations.Address> list = geocoder.GetFromLocationName(name, 5);

            Android.Locations.Address address = list[0];

            geoPoint = new LatLng(address.Latitude, address.Longitude);

            return geoPoint;
        }
    }   
}