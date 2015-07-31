// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanZoomGestureRecognizer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Recognizes drag/pinch multi-touch gestures and translates them into pan/zoom information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if __UNIFIED__
namespace OxyPlot.Xamarin.iOS
#else
namespace OxyPlot.MonoTouch
#endif
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if __UNIFIED__
    using global::Foundation;
    using global::UIKit;
#else
    using global::MonoTouch.Foundation;
    using global::MonoTouch.UIKit;
#endif

    /// <summary>
    /// Recognizes drag/pinch multi-touch gestures and translates them into pan/zoom information.
    /// </summary>
    public class PanZoomGestureRecognizer : UIGestureRecognizer
    {
        /// <summary>
        /// Up to 2 touches being currently tracked in a pan/zoom.
        /// </summary>
        private readonly List<UITouch> activeTouches = new List<UITouch>();

        /// <summary>
        /// Distance between touch points when the second touch point begins. Used to determine
        /// whether the touch points cross along a given axis during the zoom gesture.
        /// </summary>
        private ScreenVector startingDistance = default(ScreenVector);

        /// <summary>
        /// Initializes a new instance of the <see cref="PanZoomGestureRecognizer"/> class.
        /// </summary>
        /// <remarks>
        /// To add methods that will be invoked upon recognition, you can use the AddTarget method.
        /// </remarks>
        public PanZoomGestureRecognizer()
        {
            this.ZoomThreshold = 20d;
            this.AllowPinchPastZero = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PanZoomGestureRecognizer"/> keeps the aspect ratio when pinching.
        /// </summary>
        /// <value><c>true</c> if keep aspect ratio when pinching; otherwise, <c>false</c>.</value>
        public bool KeepAspectRatioWhenPinching { get; set; }

        /// <summary>
        /// Gets or sets how far apart touch points must be on a certain axis to enable scaling that axis.
        /// (only applies if KeepAspectRatioWhenPinching is <c>false</c>)
        /// </summary>
        public double ZoomThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a zoom-out gesture can turn into a zoom-in gesture if the fingers cross. 
        /// If <c>true</c>, and <see cref="KeepAspectRatioWhenPinching" /> is <c>false</c>, a zoom-out gesture
        /// can turn into a zoom-in gesture if the fingers cross. Setting to <c>false</c> will
        /// instead simply stop the zoom at that point.
        /// </summary>
        public bool AllowPinchPastZero { get; set; }

        /// <summary>
        /// Gets or sets the current calculated pan/zoom changes.
        /// </summary>
        /// <value>
        /// The touch event arguments.
        /// </value>
        public OxyTouchEventArgs TouchEventArgs { get; set; }

        /// <summary>
        /// Called when a touch gesture begins.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (this.activeTouches.Count >= 2)
            {
                // we already have two touches
                return;
            }

            // Grab 1-2 touches to track
            var newTouches = touches.ToArray<UITouch>();
            var firstTouch = !this.activeTouches.Any();

            this.activeTouches.AddRange(newTouches.Take(2 - this.activeTouches.Count));

            if (firstTouch)
            {
                // HandleTouchStarted initializes the entire multitouch gesture,
                // with the first touch used for panning.
                this.TouchEventArgs = this.activeTouches.First().ToTouchEventArgs(this.View);
            }

            this.CalculateStartingDistance();
        }

        /// <summary>
        /// Called when a touch gesture is moving.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (this.activeTouches.Any(touch => touch.Phase == UITouchPhase.Moved))
            {
                // get current and previous location of the first touch point
                var t1 = this.activeTouches.First();
                var l1 = t1.LocationInView(this.View).ToScreenPoint();
                var pl1 = t1.Phase == UITouchPhase.Moved ? t1.PreviousLocationInView(this.View).ToScreenPoint() : l1;

                var l = l1;
                var t = l1 - pl1;
                var s = new ScreenVector(1, 1);

                if (this.activeTouches.Count > 1)
                {
                    // get current and previous location of the second touch point
                    var t2 = this.activeTouches.ElementAt(1);
                    var l2 = t2.LocationInView(this.View).ToScreenPoint();
                    var pl2 = t2.Phase == UITouchPhase.Moved ? t2.PreviousLocationInView(this.View).ToScreenPoint() : l2;

                    var d = l1 - l2;
                    var pd = pl1 - pl2;

                    if (!this.KeepAspectRatioWhenPinching)
                    {
                        if (!this.AllowPinchPastZero)
                        {
                            // Don't allow fingers crossing in a zoom-out gesture to turn it back into a zoom-in gesture
                            d = this.PreventCross(d);
                        }

                        var scalex = this.CalculateScaleFactor(d.X, pd.X);
                        var scaley = this.CalculateScaleFactor(d.Y, pd.Y);
                        s = new ScreenVector(scalex, scaley);
                    }
                    else
                    {
                        var scale = pd.Length > 0 ? d.Length / pd.Length : 1;
                        s = new ScreenVector(scale, scale);
                    }
                }

                var e = new OxyTouchEventArgs { Position = l, DeltaTranslation = t, DeltaScale = s };
                this.TouchEventArgs = e;
                this.State = UIGestureRecognizerState.Changed;
            }
        }

        /// <summary>
        /// Called when a touch gesture ends.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            // We already have the only two touches we care about, so ignore the params
            var secondTouch = this.activeTouches.ElementAtOrDefault(1);

            if (secondTouch != null && secondTouch.Phase == UITouchPhase.Ended)
            {
                this.activeTouches.Remove(secondTouch);
            }

            var firstTouch = this.activeTouches.FirstOrDefault();

            if (firstTouch != null && firstTouch.Phase == UITouchPhase.Ended)
            {
                this.activeTouches.Remove(firstTouch);

                if (!this.activeTouches.Any())
                {
                    this.TouchEventArgs = firstTouch.ToTouchEventArgs(this.View);

					if (this.State == UIGestureRecognizerState.Possible)
					{
						this.State = UIGestureRecognizerState.Failed;
					}
					else
					{
						this.State = UIGestureRecognizerState.Ended;
					}
                }
            }
        }

        /// <summary>
        /// Called when a touch gesture is cancelled.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

			// I'm not sure if it's actually possible for one touch to be canceled without
			// both being canceled, but just to be safe let's stay consistent with TouchesEnded
			// and handle that scenario.

			// We already have the only two touches we care about, so ignore the params
			var secondTouch = this.activeTouches.ElementAtOrDefault(1);

			if (secondTouch != null && secondTouch.Phase == UITouchPhase.Cancelled)
			{
				this.activeTouches.Remove(secondTouch);
			}

			var firstTouch = this.activeTouches.FirstOrDefault();

			if (firstTouch != null && firstTouch.Phase == UITouchPhase.Cancelled)
			{
				this.activeTouches.Remove(firstTouch);

				if (!this.activeTouches.Any())
				{
					this.TouchEventArgs = firstTouch.ToTouchEventArgs(this.View);

					if (this.State == UIGestureRecognizerState.Possible)
					{
						this.State = UIGestureRecognizerState.Failed;
					}
					else
					{
						this.State = UIGestureRecognizerState.Cancelled;
					}
				}
			}
        }

        /// <summary>
        /// Determines whether the direction has changed.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="original">The original value.</param>
        /// <returns><c>true</c> if the direction changed.</returns>
        private static bool DidDirectionChange(double current, double original)
        {
            return (current >= 0) != (original >= 0);
        }

        /// <summary>
        /// Calculates the scale factor.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="previousDistance">The previous distance.</param>
        /// <returns>The scale factor.</returns>
        private double CalculateScaleFactor(double distance, double previousDistance)
        {
            return Math.Abs(previousDistance) > this.ZoomThreshold
                && Math.Abs(distance) > this.ZoomThreshold
                ? Math.Abs(distance / previousDistance)
                : 1;
        }

        /// <summary>
        /// Calculates the starting distance.
        /// </summary>
        private void CalculateStartingDistance()
        {
            if (this.activeTouches.Count < 2)
            {
                this.startingDistance = default(ScreenVector);
                return;
            }

            var loc1 = this.activeTouches.ElementAt(0).LocationInView(this.View).ToScreenPoint();
            var loc2 = this.activeTouches.ElementAt(1).LocationInView(this.View).ToScreenPoint();

            this.startingDistance = loc1 - loc2;
        }

        /// <summary>
        /// Applies the "prevent fingers crossing" to the specified vector.
        /// </summary>
        /// <param name="currentDistance">The current distance.</param>
        /// <returns>A vector where the "prevent fingers crossing" is applied.</returns>
        private ScreenVector PreventCross(ScreenVector currentDistance)
        {
            var x = currentDistance.X;
            var y = currentDistance.Y;

            if (DidDirectionChange(x, this.startingDistance.X))
            {
                x = 0;
            }

            if (DidDirectionChange(y, this.startingDistance.Y))
            {
                y = 0;
            }

            return new ScreenVector(x, y);
        }
    }
}