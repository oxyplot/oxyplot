// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;

    using Xamarin.Forms.Platform.Android;

    [Activity(Label = "SimpleDemo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            this.LoadApplication(new App());
        }
    }
}

