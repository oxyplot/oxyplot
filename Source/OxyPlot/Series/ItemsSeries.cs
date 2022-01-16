// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series that can contain items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Abstract base class for series that can contain items.
    /// </summary>
    public abstract class ItemsSeries : Series
    {
        /// <summary>
        /// Gets or sets the items source. The default is <c>null</c>.
        /// </summary>
        /// <value>The items source.</value>
        [CodeGeneration(false)]
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets the item for the specified index.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        /// <param name="index">The index.</param>
        /// <returns>The get item.</returns>
        /// <remarks>Returns <c>null</c> if ItemsSource is not set, or the index is outside the boundaries.</remarks>
        protected static object GetItem(IEnumerable itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }

                return null;
            }

            var i = 0;
            return itemsSource.Cast<object>().FirstOrDefault(item => i++ == index);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected virtual object GetItem(int i)
        {
            return GetItem(this.ItemsSource, i);
        }

        /// <summary>
        /// Gets or sets the format string for the item's label.
        /// </summary>
        public string LabelFormatString 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Returns formatted label
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract string GetFormattedLabel(object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="formatter">The code actually doing formatting</param>
        /// <returns></returns>
        protected string GetFormattedLabel<T>(object item, Func<T, string> formatter)
        {
            if (item == null)
                return "";

            if (item is T typedItem)
                return formatter(typedItem);

            throw GetInvalidItemType();
        }

        /// <summary>
        /// Returns exception to be thrown when derived class is not supporting GetFormattedLabel method.
        /// </summary>
        /// <returns></returns>
        protected Exception GetFormattedLabelNotSupported() { return new InvalidOperationException("GetFormattedLabel is not valid for this class"); }

        /// <summary>
        /// Returns exception to be thrown when item send to GetFormattedLabel method is not of the expected type.
        /// </summary>
        /// <returns></returns>
        protected Exception GetInvalidItemType() { return new InvalidOperationException("Invalid item type"); }
    }
}
