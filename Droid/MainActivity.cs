using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.SecureStorage;
using Plugin.Permissions;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace FoodSpecs.Droid
{
	[Activity(Label = "Food Specs", Icon = "@drawable/foodspecs", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//XFormsApplicationDroid//
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			//Needed for android secure storage
			SecureStorageImplementation.StoragePassword = "password";

			UserDialogs.Init(() => (Activity)Forms.Context);

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
