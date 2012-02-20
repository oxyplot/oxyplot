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
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
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
			
			View = new GraphView(exampleInfo);
		}
		
		private void Email(string exportType)
		{
			if(!MFMailComposeViewController.CanSendMail) 
				return;
			
			var title = exampleInfo.Title + "." + exportType;
			NSData nsData = null;
			string attachmentType = "text/plain";
			var export = new Export(View, new SizeF(800,600));
			switch(exportType)
			{
			case "png":
				nsData =  export.ToPng();
				attachmentType = "image/png";
				break;
			case "pdf":
				nsData = export.ToPdf();
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