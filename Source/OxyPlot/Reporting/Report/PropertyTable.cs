// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTable.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table of auto generated property values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a table of auto generated property values.
    /// </summary>
    /// <remarks>The PropertyTable auto generates columns or rows based on reflecting the Items type.</remarks>
    public class PropertyTable : ItemsTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTable" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="itemsInRows">The items in rows.</param>
        public PropertyTable(IEnumerable items, bool itemsInRows)
            : base(itemsInRows)
        {
            this.Alignment = Alignment.Left;
            var input = items.Cast<object>().ToArray();
            this.UpdateFields(input);
            this.Items = input;
        }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The type of the items.</returns>
        private Type GetItemType(IEnumerable items)
        {
            Type result = null;
            foreach (var item in items)
            {
                var t = item.GetType();
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
        /// Updates the fields.
        /// </summary>
        /// <param name="items">The items.</param>
        private void UpdateFields(IEnumerable items)
        {
            Type type = this.GetItemType(items);
            if (type == null)
            {
                return;
            }

            this.Columns.Clear();

            var properties = type.GetRuntimeProperties().Where(pi => pi.GetMethod.IsPublic && !pi.GetMethod.IsStatic);
            foreach (var pi in properties)
            {
                // TODO: support Browsable and Displayname attributes
                var header = pi.Name;
                this.Fields.Add(new ItemsTableField(header, pi.Name, null, Alignment.Left));
            }
        }
    }
}