using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SimpleDemo.Android
{
    [Activity (Label = "SimpleDemo.Android", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation, Icon = "@drawable/icon")]
    public class MainActivity : AndroidActivity
    {
        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            OxyPlot.XamarinFormsAndroid.Forms.Init ();
            Forms.Init (this, bundle);

            SetPage (App.GetMainPage ());
        }
    }
}
