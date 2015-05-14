// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a low-level PDF writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provides a low-level PDF writer.
    /// </summary>
    internal class PdfWriter : IDisposable
    {
        /// <summary>
        /// The output writer.
        /// </summary>
        private BinaryWriter w;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfWriter" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public PdfWriter(Stream s)
        {
            this.w = new BinaryWriter(s);
        }

        /// <summary>
        /// Specifies the object type.
        /// </summary>
        internal enum ObjectType
        {
            /// <summary>
            /// The Catalog type.
            /// </summary>
            Catalog,

            /// <summary>
            /// The Pages type.
            /// </summary>
            Pages,

            /// <summary>
            /// The Page type.
            /// </summary>
            Page,

            /// <summary>
            /// The Font type.
            /// </summary>
            Font,

            /// <summary>
            /// The XObject type.
            /// </summary>
            XObject,

            /// <summary>
            /// The ExtGState type.
            /// </summary>
            ExtGState,

            /// <summary>
            /// The FontDescriptor type.
            /// </summary>
            FontDescriptor
        }

        /// <summary>
        /// Specifies a document object.
        /// </summary>
        internal interface IPortableDocumentObject
        {
            /// <summary>
            /// Gets the object number.
            /// </summary>
            int ObjectNumber { get; }
        }

        /// <summary>
        /// Gets the position in the stream.
        /// </summary>
        public long Position
        {
            get
            {
                return this.w.BaseStream.Position;
            }
        }

        /// <summary>
        /// Writes a formatted string.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        public void Write(string format, params object[] args)
        {
            // TODO: Encoding?
            this.w.Write(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, format, args)));
        }

        /// <summary>
        /// Writes a formatted line.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        public void WriteLine(string format, params object[] args)
        {
            this.Write(format + "\n", args);
        }

        /// <summary>
        /// Writes a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public void Write(Dictionary<string, object> dictionary)
        {
            this.WriteLine("<<");
            foreach (var kvp in dictionary)
            {
                this.Write(kvp.Key);
                this.Write(" ");
                this.WriteCore(kvp.Value);
                this.WriteLine();
            }

            this.Write(">>");
        }

        /// <summary>
        /// Writes a byte array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        public void Write(byte[] bytes)
        {
            this.w.Write(bytes);
        }

        /// <summary>
        /// Writes an empty line.
        /// </summary>
        public void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.w.Dispose();
        }

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="o">The object to write.</param>
        private void WriteCore(object o)
        {
            var pdfObject = o as IPortableDocumentObject;
            if (pdfObject != null)
            {
                this.Write("{0} 0 R", pdfObject.ObjectNumber);
                return;
            }

            if (o is ObjectType)
            {
                this.Write("/{0}", o);
                return;
            }

            if (o is int || o is double)
            {
                this.Write("{0}", o);
                return;
            }

            if (o is bool)
            {
                this.Write((bool)o ? "true" : "false");
                return;
            }

            if (o is DateTime)
            {
                var dt = (DateTime)o;
                var dts = "(D:" + dt.ToString("yyyyMMddHHmmsszz") + "'00)";
                this.Write(dts);
                return;
            }

            var s = o as string;
            if (s != null)
            {
                this.Write(s);
                return;
            }

            var list = o as IList;
            if (list != null)
            {
                this.WriteList(list);
                return;
            }

            var dictionary = o as Dictionary<string, object>;
            if (dictionary != null)
            {
                this.Write(dictionary);
            }
        }

        /// <summary>
        /// Writes a list.
        /// </summary>
        /// <param name="list">The list.</param>
        private void WriteList(IList list)
        {
            this.Write("[");
            bool first = true;

            foreach (var o in list)
            {
                if (!first)
                {
                    this.Write(" ");
                }
                else
                {
                    first = false;
                }

                this.WriteCore(o);
            }

            this.Write("]");
        }
    }
}