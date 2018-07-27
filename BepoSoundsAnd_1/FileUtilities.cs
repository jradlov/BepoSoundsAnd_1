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

// Ref: Oreilly book on Safari:"Android Programming Concepts:, section 9.4


namespace BepoSoundsAnd_1
{
	public class FileUtilities
	{
		public string InternalFolder {
			get { return Android.App.Application.Context.FilesDir.AbsolutePath; }
		}

		public string PrivateExternalFolder {
			get { return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath; }
		}
		public string PublicExternalFolder {
			get { return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath; }
		}

		// check the folders exist & they're accessible: Android.OS.Environment.GetExternalStorageState()
		public string ExternalStorageState {
			get { return Android.OS.Environment.ExternalStorageState; }
		}

		// MediaMounted: external storage is present, mounted, with read/write access
		public bool IsExternalStorageReady()
		{
			if (Android.OS.Environment.MediaMounted.Equals(ExternalStorageState))
				return true;
			return false;
		}


		// return absolute path within External Storage:
		public string DownloadFolder {
			get { return PublicExternalFolder + "/" + Android.OS.Environment.DirectoryDownloads + "/"; }
		}

		public string AlarmsFolder {
			get { return PublicExternalFolder + "/" + Android.OS.Environment.DirectoryAlarms + "/"; }
		}

		public string NotificationsFolder {
			get { return PublicExternalFolder + "/" + Android.OS.Environment.DirectoryNotifications + "/"; }
		}

		public string RingtonesFolder {
			get { return PublicExternalFolder + "/" + Android.OS.Environment.DirectoryRingtones + "/"; }
		}

	}
}
