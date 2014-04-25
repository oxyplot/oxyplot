using Android.App;
using Android.Views;
using Android.OS;

namespace OxyPlotSample.Android
{
	using OxyPlot.XamarinAndroid;

	[Activity (Label = "OxyPlotSample.Android", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private readonly MyClass myClass = new MyClass();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.RequestWindowFeature (WindowFeatures.NoTitle);

			var plotView = new PlotView(this) {
				Model = myClass.MyModel
			};

			this.AddContentView (
				plotView, 
				new ViewGroup.LayoutParams (
					ViewGroup.LayoutParams.FillParent, 
					ViewGroup.LayoutParams.FillParent));
		}
	}
}


