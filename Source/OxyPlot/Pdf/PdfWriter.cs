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
        internal interface IPortableDocumentObject
        {
            int ObjectNumber { get; }
        }

        internal enum ObjectType
        {
            Catalog,

            Pages,

            Page,

            Font,

            XObject,

            ExtGState,

            FontDescriptor
        }

        private BinaryWriter w;

        public PdfWriter(Stream s)
        {
            this.w = new BinaryWriter(s);
        }

        public long Position
        {
            get
            {
                return this.w.BaseStream.Position;
            }
        }

        public void Write(string format, params object[] args)
        {
            // TODO: Encoding?
            this.w.Write(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, format, args)));
        }

        public void WriteLine(string format, params object[] args)
        {
            this.Write(format + "\n", args);
        }

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


        public void Write(byte[] buffer)
        {
            this.w.Write(buffer);
        }

        public void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        public void Dispose()
        {
#if NET35
            this.w.Close();
#else
            this.w.Dispose();
#endif
        }
    }
}