using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MessageUI;
using ExampleLibrary;

namespace MonoTouch.Demo
{
	public class GraphViewController : UIViewController
	{
		private readonly ExampleInfo exampleInfo;
		
		public GraphViewController (ExampleInfo exampleInfo)
		{
			this.exampleInfo = exampleInfo;
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
			
			var scrollView = new GraphScrollView(exampleInfo, 
			                 new RectangleF(new PointF(0, 0), 
			               new SizeF(UIScreen.MainScreen.ApplicationFrame.Size.Width,
			          UIScreen.MainScreen.ApplicationFrame.Height -
			          UIScreen.MainScreen.ApplicationFrame.Top - 10)));
			View = scrollView;
		}
		
		private void Email(string exportType)
		{
			if(!MFMailComposeViewController.CanSendMail) 
				return;
			
			var title = exampleInfo.Title + "." + exportType;
			NSData nsData = null;
			string attachmentType = "text/plain";
			var rect = new RectangleF(0,0,800,600);
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

			this.PresentModalViewController (mail, true);
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
		    e.Controller.DismissModalViewControllerAnimated (true);
		}
	}
}