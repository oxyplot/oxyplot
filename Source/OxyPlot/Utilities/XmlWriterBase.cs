// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlWriterBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for exporters that write xml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Provides an abstract base class for exporters that write xml.
    /// </summary>
    public abstract class XmlWriterBase : IDisposable
    {
        /// <summary>
        /// The xml writer.
        /// </summary>
        private readonly XmlWriter w;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref = "XmlWriterBase" /> class.
        /// </summary>
        protected XmlWriterBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlWriterBase" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected XmlWriterBase(Stream stream)
        {
            this.w = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 });
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            this.Flush();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            this.w.Flush();
        }

        /// <summary>
        /// Writes an attribute string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected void WriteAttributeString(string name, string value)
        {
            this.w.WriteAttributeString(name, value);
        }

        /// <summary>
        /// Writes an attribute string with a prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="name">The name.</param>
        /// <param name="ns">The constant.</param>
        /// <param name="value">The value.</param>
        protected void WriteAttributeString(string prefix, string name, string ns, string value)
        {
            this.w.WriteAttributeString(prefix, name, ns, value);
        }

        /// <summary>
        /// Writes the doc type.
        /// </summary>
        /// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
        /// <param name="pubid">If non-<c>null</c> it also writes PUBLIC "pubid" "sysid" where pubid and sysid are replaced with the value of the given arguments.</param>
        /// <param name="sysid">If pubid is <c>null</c> and sysid is non-<c>null</c> it writes SYSTEM "sysid" where sysid is replaced with the value of this argument.</param>
        /// <param name="subset">If non-<c>null</c> it writes [subset] where subset is replaced with the value of this argument.</param>
        protected void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            this.w.WriteDocType(name, pubid, sysid, subset);
        }

        /// <summary>
        /// Writes an element string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="text">The text.</param>
        protected void WriteElementString(string name, string text)
        {
            this.w.WriteElementString(name, text);
        }

        /// <summary>
        /// Writes the end document.
        /// </summary>
        protected void WriteEndDocument()
        {
            this.w.WriteEndDocument();
        }

        /// <summary>
        /// Writes an element end tag.
        /// </summary>
        protected void WriteEndElement()
        {
            this.w.WriteEndElement();
        }

        /// <summary>
        /// Writes raw text.
        /// </summary>
        /// <param name="text">The text.</param>
        protected void WriteRaw(string text)
        {
            this.w.WriteRaw(text);
        }

        /// <summary>
        /// Writes the start document.
        /// </summary>
        /// <param name="standalone">The standalone.</param>
        protected void WriteStartDocument(bool standalone)
        {
            this.w.WriteStartDocument(standalone);
        }

        /// <summary>
        /// Writes an element start tag.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void WriteStartElement(string name)
        {
            this.w.WriteStartElement(name);
        }

        /// <summary>
        /// Writes an element tag with the specified name and namespace.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ns">The ns.</param>
        protected void WriteStartElement(string name, string ns)
        {
            this.w.WriteStartElement(name, ns);
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="text">The text.</param>
        protected void WriteString(string text)
        {
            this.w.WriteString(text);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Close();
                }
            }

            this.disposed = true;
        }
    }
}