using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MyLove.Droid
{
    [Activity(Label = "Text Me", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // check to see if we can actually record - if we can, assign the event to the button
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                //var alert = new AlertDialog.Builder(recButton.Context);
                //alert.SetTitle("You don't seem to have a microphone to record with");
                //alert.SetPositiveButton("OK", (sender, e) =>
                //{
                //    textBox.Text = "No microphone present";
                //    recButton.Enabled = false;
                //    return;
                //});

                //alert.Show();
            }
            LoadApplication(new App());
        }
    }
}

