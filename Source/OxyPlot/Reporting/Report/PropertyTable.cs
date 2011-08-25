// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTable.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The PropertyTable autogenerates columns based on reflecting the Items type.
//   Only [Browsable] properties are included.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// The PropertyTable autogenerates columns based on reflecting the Items type.
    /// Only [Browsable] properties are included.
    /// </summary>
    public class PropertyTable : ItemsTable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTable"/> class.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="itemsInRows">
        /// The items in rows.
        /// </param>
        public PropertyTable(IEnumerable items, bool itemsInRows)
            : base(itemsInRows)
        {
            this.Alignment = Alignment.Left;
            this.UpdateFields(items);
            this.Items = items;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get item type.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <returns>
        /// </returns>
        private Type GetItemType(IEnumerable items)
        {
            Type result = null;
            foreach (object item in items)
            {
                Type t = item.GetType();
                if (result == null)
                {
                    result = t;
                }

                if (t != result)
                {
                    return null;
                }
            }

            return result;
        }

        /// <summary>
        /// The update fields.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        private void UpdateFields(IEnumerable items)
        {
            Type type = this.GetItemType(items);
            if (type == null)
            {
                return;
            }

            this.Columns.Clear();

#if SILVERLIGHT
            foreach (var pi in type.GetProperties())
            {
                var header = pi.Name;
                Fields.Add(new ItemsTableField(header, pi.Name, null, Alignment.Left));
            }
#else
            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(type))
            {
                if (!p.IsBrowsable)
                {
                    continue;
                }

                string header = p.DisplayName ?? p.Name;
                this.Fields.Add(new ItemsTableField(header, p.Name));
            }

#endif
        }

        #endregion
    }
}