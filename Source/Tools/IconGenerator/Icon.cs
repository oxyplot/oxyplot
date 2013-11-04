namespace IcoMaker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public enum ImageType { Icon = 1, Cursor = 2 }

    public class Icon
    {
        public IList<Entry> Entries { get; private set; }

        public ImageType ImageType { get; set; }

        public Icon()
        {
            this.Entries = new List<Entry>();
        }

        public enum ImageFormat { Unknown, PNG, BMP }

        [CLSCompliant(false)]
        public class Entry
        {
            public byte Width { get; set; }
            public byte Height { get; set; }
            public byte Colors { get; set; }
            public byte Reserved { get; set; }
            public ushort ColorPlanes { get; set; }
            public ushort BitsPerPixel { get; set; }
            public uint Size { get; set; }
            public uint Offset { get; set; }
            public byte[] ImageData { get; set; }

            public Image Image { get; set; }
            public IcoMaker.Bitmap Bitmap { get; set; }

            public ImageFormat Format
            {
                get
                {
                    if (this.ImageData == null || this.ImageData.Length < 4)
                        return ImageFormat.Unknown;
                    if ((this.ImageData[0] == 137) && (this.ImageData[1] == 80) && (this.ImageData[2] == 78) && (this.ImageData[3] == 71))
                        return ImageFormat.PNG;
                    return ImageFormat.BMP;
                }
            }
        }

        public void Load(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                using (var r = new BinaryReader(stream))
                {
                    var reserved = r.ReadUInt16();
                    if (reserved != 0)
                    {
                        throw new InvalidOperationException();
                    }

                    this.ImageType = (ImageType)r.ReadUInt16();
                    var n = r.ReadUInt16();
                    for (int i = 0; i < n; i++)
                    {
                        var entry = new Entry
                                        {
                                            Width = r.ReadByte(),
                                            Height = r.ReadByte(),
                                            Colors = r.ReadByte(),
                                            Reserved = r.ReadByte(),
                                            ColorPlanes = r.ReadUInt16(),
                                            BitsPerPixel = r.ReadUInt16(),
                                            Size = r.ReadUInt32(),
                                            Offset = r.ReadUInt32()
                                        };
                        this.Entries.Add(entry);
                    }

                    for (int i = 0; i < n; i++)
                    {
                        var entry = this.Entries[i];
                        r.BaseStream.Seek(entry.Offset, SeekOrigin.Begin);
                        entry.ImageData = r.ReadBytes((int)entry.Size);

                        using (var ms = new MemoryStream(entry.ImageData))
                        {
                            switch (entry.Format)
                            {
                                case ImageFormat.PNG:
                                    entry.Image = Image.FromStream(ms);
                                    break;
                                case ImageFormat.BMP:
                                    // Recall that if an image is stored in BMP format, it must exclude the opening BITMAPFILEHEADER.
                                    entry.Bitmap = IcoMaker.Bitmap.Load(ms, true);
                                    break;
                            }
                        }
                    }
                }
            }

        }

        public void Save(string path)
        {
            // http://en.wikipedia.org/wiki/ICO_(file_format)
            using (var s = File.OpenWrite(path))
            {
                using (var w = new BinaryWriter(s))
                {
                    w.Write((short)0);
                    w.Write((short)1);
                    w.Write((short)this.Entries.Count);
                    int offset = 6 + this.Entries.Count * 16;
                    var buffers = new List<byte[]>();
                    foreach (var e in this.Entries)
                    {
                        w.Write(e.Width);        // Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
                        w.Write(e.Height);       // Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
                        w.Write(e.Colors);       // Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
                        w.Write((byte)0);        // Reserved
                        w.Write(e.ColorPlanes);  // Specifies color planes. Should be 0 or 1
                        w.Write(e.BitsPerPixel); // Specifies bits per pixel.

                        byte[] bytes = null;
                        switch (e.Format)
                        {
                            case ImageFormat.PNG:
                                bytes = this.ImageToByteArray(e.Image);
                                break;
                            case ImageFormat.BMP:
                                bytes = this.BitmapToByteArray(e.Bitmap, true);
                                break;
                        }

                        buffers.Add(bytes);
                        var size = bytes != null ? bytes.Length : 0;
                        w.Write((uint)size); // Specifies the size of the image's data in bytes
                        w.Write((uint)offset); // Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
                        offset += size;
                    }

                    // All image data referenced by entries in the image directory proceed directly after the image directory.
                    // It is customary practice to store them in the same order as defined in the image directory.
                    foreach (var b in buffers)
                    {
                        // Recall that if an image is stored in BMP format, it must exclude the opening BITMAPFILEHEADER structure,
                        // whereas if it is stored in PNG format, it must be stored in its entirety.
                        w.Write(b);
                    }
                }
            }
        }

        private byte[] BitmapToByteArray(IcoMaker.Bitmap bitmap, bool excludeFileHeader)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, excludeFileHeader);
                return ms.GetBuffer();
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.GetBuffer();
            }
        }

        public void AddImage(string filename)
        {
            var image = Image.FromFile(filename);
            var entry = new Entry
                            {
                                Image = image,
                                ImageData = this.ImageToByteArray(image),
                                Width = (byte)image.Width,
                                Height = (byte)image.Height,
                                Colors = (byte)image.Height,
                                ColorPlanes = 1,
                                BitsPerPixel = 0
                            };
            this.Entries.Add(entry);
        }

        public void AddBitmap(string filename)
        {
            var bitmap = IcoMaker.Bitmap.LoadFromFile(filename);
            var entry = new Entry
                            {
                                Bitmap = bitmap,
                                ImageData = this.BitmapToByteArray(bitmap, false),
                                Width = (byte)bitmap.Width,
                                Height = (byte)bitmap.Height,
                                Colors = (byte)bitmap.Height,
                                ColorPlanes = 1,
                                BitsPerPixel = 0
                            };
            this.Entries.Add(entry);
        }
    }
}