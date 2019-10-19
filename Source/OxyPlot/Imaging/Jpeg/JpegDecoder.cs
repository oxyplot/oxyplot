// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegDecoder.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements support for decoding jpeg images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Implements support for decoding jpeg images.
    /// </summary>
    public class JpegDecoder : IImageDecoder
    {
        // http://en.wikipedia.org/wiki/Exchangeable_image_file_format
        // http://www.exif.org/Exif2-2.PDF
        // http://gvsoft.homedns.org/exif/exif-explanation.html
        // http://www.codeproject.com/Articles/43665/ExifLibrary-for-NET
        // http://www.codeproject.com/Articles/36342/ExifLib-A-Fast-Exif-Data-Extractor-for-NET-2-0
        // http://www.codeproject.com/Articles/27242/ExifTagCollection-An-EXIF-metadata-extraction-libr
        // http://www.codeproject.com/Articles/47486/Understanding-and-Reading-Exif-Data
        // https://drewnoakes.com/code/exif/

        /// <summary>
        /// Defines the EXIF tags.
        /// </summary>
        public enum ExifTags
        {
            // IFD0 items
#pragma warning disable 1591
            ImageWidth = 0x100,
            ImageLength = 0x101,
            BitsPerSample = 0x102,
            Compression = 0x103,
            PhotometricInterpretation = 0x106,
            ImageDescription = 0x10E,
            Make = 0x10F,
            Model = 0x110,
            StripOffsets = 0x111,
            Orientation = 0x112,
            SamplesPerPixel = 0x115,
            RowsPerStrip = 0x116,
            StripByteCounts = 0x117,
            XResolution = 0x11A,
            YResolution = 0x11B,
            PlanarConfiguration = 0x11C,
            ResolutionUnit = 0x128,
            TransferFunction = 0x12D,
            Software = 0x131,
            DateTime = 0x132,
            Artist = 0x13B,
            WhitePoint = 0x13E,
            PrimaryChromaticities = 0x13F,
            JPEGInterchangeFormat = 0x201,
            JPEGInterchangeFormatLength = 0x202,
            YCbCrCoefficients = 0x211,
            YCbCrSubSampling = 0x212,
            YCbCrPositioning = 0x213,
            ReferenceBlackWhite = 0x214,
            Copyright = 0x8298,

            // SubIFD items
            ExposureTime = 0x829A,
            FNumber = 0x829D,
            ExposureProgram = 0x8822,
            SpectralSensitivity = 0x8824,
            ISOSpeedRatings = 0x8827,
            OECF = 0x8828,
            ExifVersion = 0x9000,
            DateTimeOriginal = 0x9003,
            DateTimeDigitized = 0x9004,
            ComponentsConfiguration = 0x9101,
            CompressedBitsPerPixel = 0x9102,
            ShutterSpeedValue = 0x9201,
            ApertureValue = 0x9202,
            BrightnessValue = 0x9203,
            ExposureBiasValue = 0x9204,
            MaxApertureValue = 0x9205,
            SubjectDistance = 0x9206,
            MeteringMode = 0x9207,
            LightSource = 0x9208,
            Flash = 0x9209,
            FocalLength = 0x920A,
            SubjectArea = 0x9214,
            MakerNote = 0x927C,
            UserComment = 0x9286,
            SubsecTime = 0x9290,
            SubsecTimeOriginal = 0x9291,
            SubsecTimeDigitized = 0x9292,
            FlashpixVersion = 0xA000,
            ColorSpace = 0xA001,
            PixelXDimension = 0xA002,
            PixelYDimension = 0xA003,
            RelatedSoundFile = 0xA004,
            FlashEnergy = 0xA20B,
            SpatialFrequencyResponse = 0xA20C,
            FocalPlaneXResolution = 0xA20E,
            FocalPlaneYResolution = 0xA20F,
            FocalPlaneResolutionUnit = 0xA210,
            SubjectLocation = 0xA214,
            ExposureIndex = 0xA215,
            SensingMethod = 0xA217,
            FileSource = 0xA300,
            SceneType = 0xA301,
            CFAPattern = 0xA302,
            CustomRendered = 0xA401,
            ExposureMode = 0xA402,
            WhiteBalance = 0xA403,
            DigitalZoomRatio = 0xA404,
            FocalLengthIn35mmFilm = 0xA405,
            SceneCaptureType = 0xA406,
            GainControl = 0xA407,
            Contrast = 0xA408,
            Saturation = 0xA409,
            Sharpness = 0xA40A,
            DeviceSettingDescription = 0xA40B,
            SubjectDistanceRange = 0xA40C,
            ImageUniqueID = 0xA420,

            // GPS subifd items
            GPSVersionID = 0x0,
            GPSLatitudeRef = 0x1,
            GPSLatitude = 0x2,
            GPSLongitudeRef = 0x3,
            GPSLongitude = 0x4,
            GPSAltitudeRef = 0x5,
            GPSAltitude = 0x6,
            GPSTimestamp = 0x7,
            GPSSatellites = 0x8,
            GPSStatus = 0x9,
            GPSMeasureMode = 0xA,
            GPSDOP = 0xB,
            GPSSpeedRef = 0xC,
            GPSSpeed = 0xD,
            GPSTrackRef = 0xE,
            GPSTrack = 0xF,
            GPSImgDirectionRef = 0x10,
            GPSImgDirection = 0x11,
            GPSMapDatum = 0x12,
            GPSDestLatitudeRef = 0x13,
            GPSDestLatitude = 0x14,
            GPSDestLongitudeRef = 0x15,
            GPSDestLongitude = 0x16,
            GPSDestBearingRef = 0x17,
            GPSDestBearing = 0x18,
            GPSDestDistanceRef = 0x19,
            GPSDestDistance = 0x1A,
            GPSProcessingMethod = 0x1B,
            GPSAreaInformation = 0x1C,
            GPSDateStamp = 0x1D,
            GPSDifferential = 0x1E
#pragma warning restore 1591
        }

        /// <summary>
        /// Gets information about the image in the specified byte array.
        /// </summary>
        /// <param name="bytes">The image data.</param>
        /// <returns>An <see cref="OxyImageInfo" /> structure.</returns>
        /// <exception cref="System.FormatException">Invalid SOI
        /// or
        /// Invalid APP0 marker
        /// or
        /// Invalid marker
        /// or
        /// Invalid Exif identifier
        /// or
        /// Invalid TIFF identifier</exception>
        public OxyImageInfo GetImageInfo(byte[] bytes)
        {
            throw new NotImplementedException(); 
            /*
            int width = -1;
            int height = -1;
            double dpix = double.NaN;
            double dpiy = double.NaN;
            int bitDepth = -1;

            // TODO: This code is not yet working.
            var ms = new MemoryStream(bytes);
            var inputReader = new BinaryReader(ms);
            var soi = inputReader.ReadBytes(2); // FFD8
            if (soi[0] != 0xFF || soi[1] != 0xD8)
            {
                throw new FormatException("Invalid SOI");
            }

            // APP0 marker
            var app0 = inputReader.ReadBytes(2); // FFE0
            if (app0[0] != 0xFF || app0[1] != 0xE0)
            {
                throw new FormatException("Invalid APP0 marker");
            }

            var length0 = inputReader.ReadUInt16();
            var identifier = inputReader.ReadString(4);
            var zero = inputReader.ReadByte();
            var version = inputReader.ReadBytes(2);

            var densityUnits = inputReader.ReadByte();
            var xdensity = inputReader.ReadUInt16();
            var ydensity = inputReader.ReadUInt16();

            // thumbnail
            var tw = inputReader.ReadByte();
            var th = inputReader.ReadByte();
            var td = inputReader.ReadBytes(3 * tw * th);

            while (true)
            {
                var ff = inputReader.ReadByte();
                if (ff != 0xFF)
                {
                    throw new FormatException("Invalid marker");
                }

                var marker = inputReader.ReadByte();
                var length = inputReader.ReadUInt16();
                switch (marker)
                {
                    case 0xE1: // APP1
                        var id = inputReader.ReadString(4); // Exif
                        if (id != "Exif")
                        {
                            throw new FormatException("Invalid Exif identifier");
                        }

                        var zeros = inputReader.ReadBytes(2);

                        // TIFF HEADER
                        var baseOffset = ms.Position;
                        var byteOrder = inputReader.ReadBytes(2);
                        var isLittleEndian = byteOrder[0] == 0x49 && byteOrder[1] == 0x49; // Intel
                        var tiffId = inputReader.ReadBytes(2); // 002A
                        if (tiffId[0] != 0x00 || tiffId[1] != 0x2A)
                        {
                            throw new FormatException("Invalid TIFF identifier");
                        }

                        uint exifIfdOffset = 0;

                        var offset = inputReader.ReadUInt32(isLittleEndian);
                        ms.Seek(offset - 8, SeekOrigin.Current);
                        var fieldCount = inputReader.ReadUInt16(isLittleEndian);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            var tag = (ExifTags)inputReader.ReadUInt16(isLittleEndian);
                            var fieldType = inputReader.ReadUInt16(isLittleEndian);
                            var count = (int)inputReader.ReadUInt32(isLittleEndian);

                            var current = ms.Position;
                            var value = ReadValue(inputReader, ms, isLittleEndian, fieldType, count, baseOffset);

                            if ((uint)tag == 0x8769)
                            {
                                exifIfdOffset = (uint)value;
                            }

                            System.Diagnostics.Debug.WriteLine(tag + ":" + value);
                            ms.Position = current + 4;
                        }

                        ms.Position = baseOffset + exifIfdOffset;
                        fieldCount = inputReader.ReadUInt16(isLittleEndian);
                        for (int i = 0; i < fieldCount; i++)
                        {
                            var tag = (ExifTags)inputReader.ReadUInt16(isLittleEndian);
                            var fieldType = inputReader.ReadUInt16(isLittleEndian);
                            var count = (int)inputReader.ReadUInt32(isLittleEndian);

                            var current = ms.Position;
                            var value = ReadValue(inputReader, ms, isLittleEndian, fieldType, count, baseOffset);

                            if ((uint)tag == 0x8769)
                            {
                                exifIfdOffset = (uint)value;
                            }

                            System.Diagnostics.Debug.WriteLine(tag + ":" + value);
                            ms.Position = current + 4;
                        }

                        break;
                    default:
                        ms.Seek(length - 2, SeekOrigin.Current);
                        break;
                }
            }

#pragma warning disable CS0162 // Unreachable code detected
            // Analyzer isn't properly detecting break in infinite loop and therefore shows a warning.
            return new OxyImageInfo
            {
                Width = width,
                Height = height,
                DpiX = dpix,
                DpiY = dpiy,
                BitsPerPixel = bitDepth
            };
            */
        }
#pragma warning restore CS0162 // Unreachable code detected

        private static object ReadValue(
            BinaryReader inputReader,
            MemoryStream ms,
            bool isLittleEndian,
            ushort fieldType,
            int count,
            long baseOffset)
        {
            switch (fieldType)
            {
                case 1:
                    {
                        var valueOrOffset = inputReader.ReadUInt32(isLittleEndian);

                        if (count == 1)
                        {
                            var value = inputReader.ReadByte();
                            var x = inputReader.ReadBytes(3);
                            return value;
                        }

                        if (count < 4)
                        {
                            var value = inputReader.ReadBytes(count);
                            if (count < 4)
                            {
                                var x = inputReader.ReadBytes(4 - count);
                            }

                            return value;
                        }

                        ms.Position = baseOffset + valueOrOffset;
                        return inputReader.ReadBytes(count);
                    }

                case 2:
                    {
                        if (count <= 4)
                        {
                            var value = inputReader.ReadString(count).Trim('\0');
                            if (count < 4)
                            {
                                inputReader.ReadBytes(4 - count);
                            }

                            return value;
                        }

                        var valueOrOffset = inputReader.ReadUInt32(isLittleEndian);
                        ms.Position = baseOffset + valueOrOffset;
                        return inputReader.ReadString(count).Trim('\0');
                    }

                case 3:
                    {
                        if (count == 1)
                        {
                            var value = inputReader.ReadUInt16(isLittleEndian);
                            var x = inputReader.ReadUInt16(isLittleEndian);
                            return value;
                        }

                        if (count == 2)
                        {
                            return inputReader.ReadUInt16Array(count, isLittleEndian);
                        }

                        var valueOrOffset = inputReader.ReadUInt32(isLittleEndian);
                        ms.Position = baseOffset + valueOrOffset;
                        return inputReader.ReadUInt16Array(count, isLittleEndian);
                    }
                case 4:
                    {
                        if (count == 1)
                        {
                            return inputReader.ReadUInt32(isLittleEndian);
                        }

                        var valueOrOffset = inputReader.ReadUInt32(isLittleEndian);
                        ms.Position = baseOffset + valueOrOffset;
                        return inputReader.ReadUInt32Array(count, isLittleEndian);
                    }

                case 5:
                    {
                        var valueOrOffset = inputReader.ReadUInt32(isLittleEndian);
                        ms.Position = baseOffset + valueOrOffset;
                        if (count == 1)
                        {
                            var nominator = inputReader.ReadUInt32(isLittleEndian);
                            var denominator = inputReader.ReadUInt32(isLittleEndian);
                            return (double)nominator / denominator;
                        }

                        throw new NotImplementedException();
                    }

                case 10:
                    {
                        var valueOrOffset = inputReader.ReadInt32(isLittleEndian);
                        ms.Position = baseOffset + valueOrOffset;
                        if (count == 1)
                        {
                            var nominator = inputReader.ReadInt32(isLittleEndian);
                            var denominator = inputReader.ReadInt32(isLittleEndian);
                            return (double)nominator / denominator;
                        }

                        throw new NotImplementedException();
                    }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Decodes an image from the specified stream.
        /// </summary>
        /// <param name="bytes">The data to decode.</param>
        /// <returns>The 32-bit pixel data.</returns>
        public OxyColor[,] Decode(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
