// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanZoomGestureRecognizer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Recognizes drag/pinch multitouch gestures and translates them into pan/zoom information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Foundation;
    using UIKit;

    /// <summary>
    /// Recognizes drag/pinch multitouch gestures and translates them into pan/zoom information.
    /// </summary>
    public class PanZoomGestureRecognizer : UIGestureRecognizer
    {
        /// <summary>
        /// Up to 2 touches being currently tracked in a pan/zoom.
        /// </summary>
        private List<UITouch> activeTouches = new List<UITouch>();

        // Distance between touchpoints when the second touchpoint begins. Used to determine
        // whether the touchpoints cross along a given axis during the zoom gesture.
        //
        private ScreenVector startingDistance = default(ScreenVector);

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OxyPlot.Xamarin.iOS.PanZoomGestureRecognizer"/> keeps the aspect ratio when pinching.
        /// </summary>
        /// <value><c>true</c> if keep aspect ratio when pinching; otherwise, <c>false</c>.</value>
        public bool KeepAspectRatioWhenPinching { get; set; }

        /// <summary>
        /// How far apart touch points must be on a certain axis to enable scaling that axis.
        /// (only applies if KeepAspectRatioWhenPinching is <c>false</c>)
        /// </summary>
        public double ZoomThreshold { get; set; }

        /// <summary>
        /// If <c>true</c>, and KeepAspectRatioWhenPinching is <c>false</c>, a zoom-out gesture
        /// can turn into a zoom-in gesture if the fingers cross. Setting to <c>false</c> will
        /// instead simply stop the zoom at that point.
        /// </summary>
        public bool AllowPinchPastZero { get; set; }

        /// <summary>
        /// The current calculated pan/zoom changes
        /// </summary>
        public OxyTouchEventArgs TouchEventArgs { get; set; }


        public PanZoomGestureRecognizer()
        {
            this.ZoomThreshold = 20d;
            this.AllowPinchPastZero = true;
        }


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

            activeTouches.AddRange(newTouches.Take(2 - this.activeTouches.Count));

            if (firstTouch)
            {
                // HandleTouchStarted initializes the entire multitouch gesture,
                // with the first touch used for panning.
                //
                TouchEventArgs = this.activeTouches.First().ToTouchEventArgs(this.View);
            }

            CalculateStartingDistance();
        }

        /// <summary>
        /// Called when a touch gesture is moving.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (activeTouches.Any(touch => touch.Phase == UITouchPhase.Moved))
            {
                // get current and previous location of the first touch point
                var t1 = this.activeTouches.First();
                var l1 = t1.LocationInView(this.View).ToScreenPoint();
                var pl1 = t1.Phase == UITouchPhase.Moved ? t1.PreviousLocationInView(this.View).ToScreenPoint() : l1;

                var l = l1;
                var t = l1 - pl1;
                var s = new ScreenVector(1, 1);

                if (activeTouches.Count > 1)
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
                            d = PreventCross(d);
                        }

                        var scalex = CalculateScaleFactor(d.X, pd.X);
                        var scaley = CalculateScaleFactor(d.Y, pd.Y);
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
            //
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
                    TouchEventArgs = firstTouch.ToTouchEventArgs(this.View);
                    State = UIGestureRecognizerState.Ended;
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

            // TODO: Is it possible for one touch to be canceled while others remain in play?

            var touch = this.activeTouches.FirstOrDefault();
            if (touch != null && touch.Phase == UITouchPhase.Cancelled)
            {
                TouchEventArgs = touch.ToTouchEventArgs(this.View);
                State = UIGestureRecognizerState.Cancelled;
            }
        }


        private double CalculateScaleFactor(double distance, double previousDistance)
        {
            return Math.Abs(previousDistance) > this.ZoomThreshold
                && Math.Abs(distance) > this.ZoomThreshold
                ? Math.Abs(distance / previousDistance)
                : 1;
        }

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

        private static bool DidDirectionChange(double current, double original)
        {
            return ((current >= 0) != (original >= 0));
        }
    }
}