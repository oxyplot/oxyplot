using System;
using System.IO;
using System.Text;
using System.Xml;

namespace OxyPlot
{
    /// <summary>
    /// Base class for exporters that writes Xml.
    /// </summary>
    public abstract class XmlWriterBase : IDisposable
    {
        private Stream s;
        private XmlWriter w;

        protected XmlWriterBase(string path)
        {
            s = File.OpenWrite(path);
            w = XmlWriter.Create(s, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 });
        }

        protected XmlWriterBase(Stream stream)
        {
            w = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 });
        }

        protected void WriteStartDocument(bool standalone)
        {
            w.WriteStartDocument(standalone);
        }

        protected void WriteEndDocument()
        {
            w.WriteEndDocument();
        }

        protected void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            w.WriteDocType(name, pubid, sysid, subset);
        }

        protected void WriteStartElement(string name)
        {
            w.WriteStartElement(name);
        }

        protected void WriteStartElement(string name, string ns)
        {
            w.WriteStartElement(name, ns);
        }

        protected void WriteEndElement()
        {
            w.WriteEndElement();
        }

        protected void WriteElementString(string name, string text)
        {
            w.WriteElementString(name, text);
        }

        protected void WriteString(string text)
        {
            w.WriteString(text);
        }

        protected void WriteRaw(string text)
        {
            w.WriteRaw(text);
        }

        protected void WriteAttributeString(string name, string value)
        {
            w.WriteAttributeString(name, value);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            if (w == null)
                return;
            w.Close();
            w = null;

            if (s != null)
            {
                s.Close();
                s = null;
            }
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            w.Flush();
        }

        public void Dispose()
        {
            Close();
        }
    }
}