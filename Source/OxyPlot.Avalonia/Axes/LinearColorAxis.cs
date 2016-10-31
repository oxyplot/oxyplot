// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a Avalonia wrapper for the <see cref="OxyPlot.Axes.LinearColorAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;
    using global::Avalonia.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Provides a Avalonia wrapper for the <see cref="OxyPlot.Axes.LinearColorAxis" />.
    /// </summary>
    public class LinearColorAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="GradientStopsProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IList<GradientStop>> GradientStopsProperty = AvaloniaProperty.Register<LinearColorAxis, IList<GradientStop>>(nameof(GradientStops));

        /// <summary>
        /// Identifies the <see cref="HighColorProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> HighColorProperty = AvaloniaProperty.Register<LinearColorAxis, Color>(nameof(HighColor), Colors.White);

        /// <summary>
        /// Identifies the <see cref="LowColorProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LowColorProperty = AvaloniaProperty.Register<LinearColorAxis, Color>(nameof(LowColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="PaletteSizeProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> PaletteSizeProperty = AvaloniaProperty.Register<LinearColorAxis, int>(nameof(PaletteSize), 20, validate: (obj, val) =>
        {
            if (!ValidatePaletteSize(val))
            {
                throw new System.ArgumentException();
            }

            return val;
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearColorAxis"/> class.
        /// </summary>
        public LinearColorAxis()
        {
            InternalAxis = new Axes.LinearColorAxis();
            GradientStops = new List<GradientStop>();
        }

        /// <summary>
        /// Gets or sets the palette size.
        /// </summary>
        public int PaletteSize
        {
            get
            {
                return GetValue(PaletteSizeProperty);
            }

            set
            {
                SetValue(PaletteSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the high color.
        /// </summary>
        public Color HighColor
        {
            get
            {
                return GetValue(HighColorProperty);
            }

            set
            {
                SetValue(HighColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the low color.
        /// </summary>
        public Color LowColor
        {
            get
            {
                return GetValue(LowColorProperty);
            }

            set
            {
                SetValue(LowColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the gradient stops.
        /// </summary>
        [Content]
        public IList<GradientStop> GradientStops
        {
            get
            {
                return GetValue(GradientStopsProperty);
            }

            set
            {
                SetValue(GradientStopsProperty, value);
            }
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns>
        /// An axis object.
        /// </returns>
        public override Axes.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var axis = InternalAxis as Axes.LinearColorAxis;
            Contract.Requires<InvalidOperationException>(axis != null);
            if (GradientStops != null)
            {
                axis.Palette = GradientStops.Count > 2
                                   ? Interpolate(GradientStops.ToList(), PaletteSize)
                                   : new OxyPalette();
            }

            axis.HighColor = HighColor.ToOxyColor();
            axis.LowColor = LowColor.ToOxyColor();
            axis.Minimum = Minimum;
            axis.Maximum = Maximum;
        }

        /// <summary>
        /// Translates a collection of <see cref="GradientStop" /> to an <see cref="OxyPalette" />.
        /// </summary>
        /// <param name="stops">
        /// The gradient stops collection to convert.
        /// </param>
        /// <param name="paletteSize">
        /// The palette size.
        /// </param>
        /// <returns>
        /// The interpolated <see cref="OxyPalette"/>.
        /// </returns>
        private static OxyPalette Interpolate(List<GradientStop> stops, int paletteSize)
        {
            Debug.Assert(stops.Count >= 2, "Can't interpolate less than 2 gradient stops.");
            Debug.Assert(paletteSize > 0, "Palette size must be non-zero positive number.");

            var palette = new List<OxyColor>();
            stops.Sort((x1, x2) => x1.Offset.CompareTo(x2.Offset));

            var palettePositions = stops[0].Offset;
            var step = (double)stops.Count / paletteSize;

            for (int i = 0; i < stops.Count - 1; i++)
            {
                var start = stops[i];
                var end = stops[i + 1];

                while (palettePositions <= end.Offset)
                {
                    palette.Add(
                        OxyColor.Interpolate(
                            start.Color.ToOxyColor(),
                            end.Color.ToOxyColor(),
                            (palettePositions - start.Offset) / (end.Offset - start.Offset)));
                    palettePositions += step;
                }
            }

            return new OxyPalette(palette);
        }

        /// <summary>
        /// Validates the palette size.
        /// </summary>
        /// <param name="value">
        /// The property value.
        /// </param>
        /// <returns>
        /// The validation result.
        /// </returns>
        private static bool ValidatePaletteSize(object value)
        {
            return (int)value >= 1;
        }

        static LinearColorAxis()
        {
            GradientStopsProperty.Changed.AddClassHandler<LinearColorAxis>(AppearanceChanged);
            HighColorProperty.Changed.AddClassHandler<LinearColorAxis>(AppearanceChanged);
            LowColorProperty.Changed.AddClassHandler<LinearColorAxis>(AppearanceChanged);
            PaletteSizeProperty.Changed.AddClassHandler<LinearColorAxis>(AppearanceChanged);
        }
    }
}