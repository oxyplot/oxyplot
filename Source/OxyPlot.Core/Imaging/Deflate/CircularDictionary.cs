// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircularDictionary.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a circular dictionary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides a circular dictionary.
    /// </summary>
    /// <remarks>The code is a c# port of the DEFLATE project by Nayuki Minase at <a href="https://github.com/nayuki/DEFLATE">github</a>.
    /// Original source code: <a href="https://github.com/nayuki/DEFLATE/blob/master/src/nayuki/deflate/CircularDictionary.java">CircularDictionary.java</a>.</remarks>
    internal class CircularDictionary
    {
        /// <summary>
        /// The data
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// The mask
        /// </summary>
        private readonly int mask;

        /// <summary>
        /// The index
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDictionary" /> class.
        /// </summary>
        /// <param name="size">The size of the dictionary.</param>
        public CircularDictionary(int size)
        {
            this.data = new byte[size];
            this.index = 0;

            // Test if size is a power of 2
            if (size > 0 && (size & (size - 1)) == 0)
            {
                this.mask = size - 1;
            }
            else
            {
                this.mask = 0;
            }
        }

        /// <summary>
        /// Appends the specified byte.
        /// </summary>
        /// <param name="b">The byte.</param>
        public void Append(int b)
        {
            this.data[this.index] = (byte)b;
            if (this.mask != 0)
            {
                this.index = (this.index + 1) & this.mask;
            }
            else
            {
                this.index = (this.index + 1) % this.data.Length;
            }
        }

        /// <summary>
        /// Copies the specified bytes to the output writer.
        /// </summary>
        /// <param name="dist">The distance?</param>
        /// <param name="len">The length.</param>
        /// <param name="w">The writer.</param>
        public void Copy(int dist, int len, BinaryWriter w)
        {
            if (len < 0 || dist < 1 || dist > this.data.Length)
            {
                throw new Exception();
            }

            if (this.mask != 0)
            {
                int readIndex = (this.index - dist + this.data.Length) & this.mask;
                for (int i = 0; i < len; i++)
                {
                    w.Write(this.data[readIndex]);
                    this.data[this.index] = this.data[readIndex];
                    readIndex = (readIndex + 1) & this.mask;
                    this.index = (this.index + 1) & this.mask;
                }
            }
            else
            {
                int readIndex = (this.index - dist + this.data.Length) % this.data.Length;
                for (int i = 0; i < len; i++)
                {
                    w.Write(this.data[readIndex]);
                    this.data[this.index] = this.data[readIndex];
                    readIndex = (readIndex + 1) % this.data.Length;
                    this.index = (this.index + 1) % this.data.Length;
                }
            }
        }
    }
}