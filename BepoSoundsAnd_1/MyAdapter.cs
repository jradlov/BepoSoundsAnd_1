using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


// ListView_1 creates a Custom Adapter for lists of single lines of text.
// If we want more than one line of text we have to create a Custom Row.
// A Custom Row is a layout: we create custom_row.axml in Resources/layout.
// Implement the Custom Row in MyAdapter GetView() method.

// Steps for Custom Row:
// 1) design Custom Row in axml file under Rwsources/layout
// 2) in GetView method in custom adapter, inflate Custom Row you designed
//    view = context.LayoutInflater.Inflate(Resource.Layout.custom_row, null);
// 3) Find views you want to populate with data
// 4) Populate them
//    view.FindViewById<TextView>(Resource.Id.textViewCustomRow).Text = item;


namespace BepoSoundsAnd_1
{
	public class MyAdapter : BaseAdapter<string>
	{
		List<string> items;  // list of strings to populate the adapter
		Activity context;

		public MediaPlayer player;

		//public CustomRowViewHolder viewHolder;

		int[] soundId = {Resource.Raw.bepoHello, Resource.Raw.bepoCooCoo, Resource.Raw.bepoGoool,
				Resource.Raw.bepoHaveANiceDay, Resource.Raw.bepoOhMan, Resource.Raw.bepoOoooo, Resource.Raw.bepoOops1,
				Resource.Raw.bepoOops2, Resource.Raw.bepoOopsyDaisy, Resource.Raw.bepoPaparap, Resource.Raw.bepoRooster,
				Resource.Raw.bepoShoot, Resource.Raw.bepoSleepyHead, Resource.Raw.bepoWakyWaky, Resource.Raw.bepoWolves};


		public MyAdapter(Activity context, List<string> items)
		{
			this.items = items;
			this.context = context;

			//this.viewHolder = context.viewHolder;
			//this.viewHolder = viewHolder;

		}



		// Below: default methods that need to be called when the adapter is fired

		public override long GetItemId(int position)
		{ // return current adapter position
			return position;
		}

		public override string this[int index] {  // return item in question
			get { return items[index]; }
		}

		public override int Count {   // return the length of the list (so android knows how many rows to create)
			get { return items.Count; }  // .Count for List, items.Length if array
		}

		// since it's a custom adapter, we have to create a way to display our data:
		// GetView(): create/recycle row & allow you to populate it with data
		public override View GetView(int position, View convertView, ViewGroup parent)
		{

			var item = items[position];

			// converView is the recycled row that scrolled off screen that we can reuse
			View view = convertView;

			CustomRowViewHolder viewHolder;

			// each custom row has its own 'view' object.  each view contains a viewHolder for that row in view.Tag

			if (view == null) {  // we didn't get a recycled cell/row: have to create one
										// view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
										// replace SimpleListItem1 with our own Custom Row:
				view = context.LayoutInflater.Inflate(Resource.Layout.custom_row, null);

				viewHolder = new CustomRowViewHolder();

				viewHolder.txtViewFile = view.FindViewById<TextView>(Resource.Id.txtViewFile);
				viewHolder.btnPlay = view.FindViewById<ImageView>(Resource.Id.btnPlay);
				viewHolder.btnSave = view.FindViewById<Button>(Resource.Id.btnSave);

				// define button click event here so it's done only once
				viewHolder.btnSave.Click += OnbtnSaveClick;
				viewHolder.btnPlay.Click += OnBtnPlayClick;

				view.Tag = viewHolder;
			}
			else { viewHolder = (CustomRowViewHolder)view.Tag; }



			// this 'if' is not needed, but good away to add ad/purchase to listview (trick users into buying something :) 
			// can also cut list from 100 items to 10 with last row asking for subscription to get the whole list :) !!!
			//if (position == 3)   // replace item 3 with below
			//item = "(Available to subscribers only)";

			// inside this row we have some text elements we'd like to populate with data
			//view.FindViewById<TextView>(Resource.Id.textViewCustomRow).Text = item;
			viewHolder.txtViewFile.Text = item;
			//viewHolder.txtViewFile.Text = item.Substring(0, item.Length-4);  // cut out '.mp3' at end of string

			viewHolder.btnSave.Tag = position;  // we need postion in the event method
			viewHolder.btnPlay.Tag = position;  // we need postion in the event method

			return view;
		}

		// want to have these methods for media play in the Main Activity.  HOW ???
		private void OnBtnPlayClick(object sender, EventArgs e)
		{
			var position = (int)(sender as ImageView).Tag;

			//Toast.MakeText(context, "Play at row: " + position, ToastLength.Short).Show();


			//var resourceId = (int)typeof(Resource.Raw).GetField(items[position]).GetValue(null);
			//Toast.MakeText(context, "id:"+resourceId, ToastLength.Short).Show();



			//if (player != null && player.IsPlaying)
			//if (player != null)
			playerStop();  // stop playing & release resource
			if (player == null) { // create new resource for sound

				/*var val = position % 3;
				switch (val) { // load the appropiate sound
					case 0: player = MediaPlayer.Create(context, Resource.Raw.bepoHello); break;
					case 1: player = MediaPlayer.Create(context, Resource.Raw.bepoPaparap); break;
					case 2: player = MediaPlayer.Create(context, Resource.Raw.bepoOopsyDaisy); break;
					default: player = MediaPlayer.Create(context, Resource.Raw.bepoHello); break;
				}*/
				//if (position % 3 == 0) player = MediaPlayer.Create(context, Resource.Raw.bepoHello);
				//else if (position % 3 == 1) player = MediaPlayer.Create(context, Resource.Raw.bepoPaparap);
				//else player = MediaPlayer.Create(context, Resource.Raw.bepoOopsyDaisy);

				player = MediaPlayer.Create(context, soundId[position]);
			}
			if (player != null)
				player.Start();  // play sound

		}

		private void OnbtnSaveClick(object sender, EventArgs e)
		{
			var position = (int)(sender as Button).Tag;

			//Log.Debug("-----DEBUG-----", "btn clicked at row: " + position);


			// ========== copy mito.txt file to pos#.txt file in Download folder =====

			FileUtilities fu = new FileUtilities();
			string downloadFolder = fu.DownloadFolder;

			if (fu.IsExternalStorageReady()) {
				// ========== copy audio mp3 file to Download folder ==========
				//var soundFilePath = Path.Combine(downloadFolder, "bepoHello.mp3");
				var soundFilePath = Path.Combine(downloadFolder, items[position] + ".mp3");
				using (FileStream writeStream = File.OpenWrite(soundFilePath)) {
					//var readStream = context.Resources.OpenRawResource(Resource.Raw.bepoHello);
					var readStream = context.Resources.OpenRawResource(soundId[position]);
					BinaryWriter writer = new BinaryWriter(writeStream);

					// create a buffer to hold the bytes 
					byte[] buffer = new Byte[1024];
					int bytesRead;

					// while the read method returns bytes keep writing them to the output stream
					while ((bytesRead = readStream.Read(buffer, 0, 1024)) > 0) {
						writeStream.Write(buffer, 0, bytesRead);
					}
				}

			}
			else
				Toast.MakeText(context, "External Storage not ready!", ToastLength.Short).Show();

			Toast.MakeText(context, "file copied at row: " + position, ToastLength.Short).Show();
		}

		public void playerStop()
		{
			if (player != null) {
				if (player.IsPlaying)
					player.Stop();
				player.Release();
				player = null;
			}
		}
	}
}