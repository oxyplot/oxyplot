// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupingItemsControlConverter.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Class that implements simple grouping for ItemsControl and its subclasses (ex: ListBox)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExampleBrowser
{
    // http://blogs.msdn.com/b/delay/archive/2010/03/17/confessions-of-a-listbox-groupie-using-ivalueconverter-to-create-a-grouped-list-of-items-simply-and-flexibly.aspx

    /// <summary>
    /// Class that implements simple grouping for ItemsControl and its subclasses (ex: ListBox)
    /// </summary>
    public class GroupingItemsControlConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The Type of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Validate parameters
            var valueAsIEnumerable = value as IEnumerable;
            if (null == valueAsIEnumerable)
            {
                throw new ArgumentException("GroupingItemsControlConverter works for only IEnumerable inputs.", "value");
            }
            var parameterAsGroupingItemsControlConverterParameter = parameter as GroupingItemsControlConverterParameters;
            if (null == parameterAsGroupingItemsControlConverterParameter)
            {
                throw new ArgumentException("Missing required GroupingItemsControlConverterParameter.", "parameter");
            }
            var groupSelectorAsIGroupingItemsControlConverterSelector =
                parameterAsGroupingItemsControlConverterParameter.GroupSelector as IGroupingItemsControlConverterSelector;
            if (null == groupSelectorAsIGroupingItemsControlConverterSelector)
            {
                throw new ArgumentException(
                    "GroupingItemsControlConverterParameter.GroupSelector must be non-null and implement IGroupingItemsControlConverterSelector.",
                    "parameter");
            }

            // Return the grouped results
            return ConvertAndGroupSequence(valueAsIEnumerable.Cast<object>(), parameterAsGroupingItemsControlConverterParameter);
        }

        /// <summary>
        /// Converts and groups the values of the specified sequence according to the settings of the specified parameters.
        /// </summary>
        /// <param name="sequence">Sequence of items.</param>
        /// <param name="parameters">Parameters for the grouping operation.</param>
        /// <returns>Converted and grouped sequence.</returns>
        private IEnumerable<object> ConvertAndGroupSequence(IEnumerable<object> sequence, GroupingItemsControlConverterParameters parameters)
        {
            // Validate parameters
            var groupSelector = ((IGroupingItemsControlConverterSelector)(parameters.GroupSelector)).GetGroupSelector();
            if (null == groupSelector)
            {
                throw new NotSupportedException("IGroupingItemsControlConverterSelector.GetGroupSelector must return a non-null value.");
            }

            // Do the grouping and ordering
            var groupedOrderedSequence = sequence.GroupBy(groupSelector).OrderBy(g => g.Key);

            // Return the wrapped results
            foreach (var group in groupedOrderedSequence)
            {
                yield return new ContentControl { Content = group.Key, ContentTemplate = parameters.GroupHeaderTemplate };
                foreach (var item in group)
                {
                    yield return new ContentControl { Content = item, ContentTemplate = parameters.ItemTemplate };
                }
            }
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The Type of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("GroupingItemsControlConverter does not support ConvertBack.");
        }
    }

    /// <summary>
    /// Class that represents the input parameters to the GroupingItemsControlConverter class.
    /// </summary>
    public class GroupingItemsControlConverterParameters
    {
        /// <summary>
        /// Template to use for the header for a group.
        /// </summary>
        public DataTemplate GroupHeaderTemplate { get; set; }

        /// <summary>
        /// Template to use for the items of a group.
        /// </summary>
        public DataTemplate ItemTemplate { get; set; }

        /// <summary>
        /// Selector to use for determining the grouping of the sequence.
        /// </summary>
        public IGroupingItemsControlConverterSelector GroupSelector { get; set; }
    }

    /// <summary>
    /// Interface for classes to be used as a selector for the GroupingItemsControlConverterParameters class.
    /// </summary>
    public interface IGroupingItemsControlConverterSelector
    {
        /// <summary>
        /// Function that returns the group selector.
        /// </summary>
        /// <returns>Key to use for grouping.</returns>
        Func<object, IComparable> GetGroupSelector();
    }
}