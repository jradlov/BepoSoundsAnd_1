using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using System.Collections.Generic;

namespace BepoSoundsAnd_1
{
	[Activity(Label = "@string/toolbar_title", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		string[] soundFiles = {"bepoHello", "bepoCooCoo", "bepoGoool",
				"bepoHaveANiceDay", "bepoOhMan", "bepoOoooo", "bepoOops1",
				"bepoOops2", "bepoOopsyDaisy", "bepoPaparap", "bepoRooster",
				"bepoShoot", "bepoSleepyHead", "bepoWakyWaky", "bepoWolves"};

		MyAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			var data = new List<string>(soundFiles);

			// reference listView with layout resource (listView1)
			var listView = FindViewById<ListView>(Resource.Id.listView1);
			listView.FastScrollEnabled = true;  // enable vertical scrollbar 

			// create custom adapter and apply it to listView
			//var adapter = new MyAdapter(this, data);
			adapter = new MyAdapter(this, data);
			listView.Adapter = adapter;


			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

		}

		public override bool OnCreateOptionsMenu(IMenu menu) {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
       }

      public override bool OnOptionsItemSelected(IMenuItem item) {
         int id = item.ItemId;
			if(id == Resource.Id.action_settings)
				Toast.MakeText(this, "Settings", ToastLength.Short).Show();
			else if(id == Resource.Id.action_about) 
				Toast.MakeText(this, "About", ToastLength.Short).Show();

		return base.OnOptionsItemSelected(item);
      }


		protected override void OnPause() { // if App goes in the background, stop playing the sound
			base.OnPause();

			if (adapter.player != null && adapter.player.IsPlaying) {
				adapter.playerStop(); // stops and unloads the audio file: when app resumes, clicking the btn plays sound normally
											 //adapter.player.Pause();  // just pauses the the playing.  When app resumes, the audio resumes where it left off
			}
		}


	}
}

