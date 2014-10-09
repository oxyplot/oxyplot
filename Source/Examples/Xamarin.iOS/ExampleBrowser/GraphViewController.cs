// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphViewController.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Handles device orientation changes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{

    using CoreGraphics;

    using Foundation;
    using UIKit;
    using MessageUI;

    using ExampleLibrary;

    using OxyPlot.Xamarin.iOS;

    public class GraphViewController : UIViewController
    {
        private readonly ExampleInfo exampleInfo;

		private PlotView plotView;

        public GraphViewController (ExampleInfo exampleInfo)
        {
            this.exampleInfo = exampleInfo;
			this.plotView = new PlotView ();
			this.plotView.Model = exampleInfo.PlotModel;
        }

        public override void LoadView ()
        {
            NavigationItem.RightBarButtonItem= new UIBarButtonItem(UIBarButtonSystemItem.Compose,
                delegate {
                    var actionSheet = new UIActionSheet ("Email", null, "Cancel", "PNG", "PDF"){
                        Style = UIActionSheetStyle.Default
                    };

                    actionSheet.Clicked += delegate (object sender, UIButtonEventArgs args){

                        if(args.ButtonIndex > 1)
                            return;

                        Email(args.ButtonIndex == 0 ? "png" : "pdf");
                    };

                    actionSheet.ShowInView (View);
                });

            // Only for iOS 7 and later?
            this.EdgesForExtendedLayout = UIRectEdge.None;

            this.View = this.plotView;

        }

		/// <summary>
		/// Handles device orientation changes.
		/// </summary>
		/// <param name="fromInterfaceOrientation">The previous interface orientation.</param>
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			this.plotView.InvalidatePlot (false);
		}
        
		private void Email(string exportType)
        {
            if(!MFMailComposeViewController.CanSendMail)
                return;

            var title = exampleInfo.Title + "." + exportType;
            NSData nsData = null;
            string attachmentType = "text/plain";
            var rect = new CGRect(0,0,800,600);
            switch(exportType)
            {
            case "png":
                nsData =  View.ToPng(rect);
                attachmentType = "image/png";
                break;
            case "pdf":
                nsData = View.ToPdf(rect);
                attachmentType = "text/x-pdf";
                break;
            }

            var mail = new MFMailComposeViewController ();
            mail.SetSubject("OxyPlot - " + title);
            mail.SetMessageBody ("Please find attached " + title, false);
            mail.Finished += HandleMailFinished;
            mail.AddAttachmentData(nsData, attachmentType, title);

            this.PresentViewController (mail, true, null);
        }

        private void HandleMailFinished (object sender, MFComposeResultEventArgs e)
        {
            if (e.Result == MFMailComposeResult.Sent) {
                UIAlertView alert = new UIAlertView ("Mail Alert", "Mail Sent",
                    null, "Yippie", null);
                alert.Show ();

                // you should handle other values that could be returned
                // in e.Result and also in e.Error
            }

            e.Controller.DismissViewController(true, null);
        }
    }
}