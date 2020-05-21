// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTrimmerTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="SimpleTextTrimmer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OxyPlot.Rendering;

    /// <summary>
    /// Provides unit tests for the <see cref="SimpleTextTrimmer" /> class.
    /// </summary>
    [TestFixture]
    public class TextTrimmerTests
    {
        /// <summary>
        /// Tests word boundaries with boring ascii text.
        /// </summary>
        [Test]
        public void CharacterBoundariesAscii()
        {
            CollectionAssert.AreEqual(new int[0], SimpleTextTrimmer.GetCharacterBoundaries(""));
            CollectionAssert.AreEqual(new[] { 1 }, SimpleTextTrimmer.GetCharacterBoundaries("A"));
            CollectionAssert.AreEqual(Range(1, 7), SimpleTextTrimmer.GetCharacterBoundaries("OxyPlot"));
            CollectionAssert.AreEqual(Range(2, 7), SimpleTextTrimmer.GetCharacterBoundaries(" OxyPlot "));
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 7, 10 }, SimpleTextTrimmer.GetCharacterBoundaries("a b\tc\nd  ?"));
        }

        /// <summary>
        /// Tests word boundaries with boring ascii punctuation.
        /// </summary>
        [Test]
        public void CharacterBoundariesPunctuation()
        {
            CollectionAssert.AreEqual(new[] { 1 } , SimpleTextTrimmer.GetCharacterBoundaries("."));
            CollectionAssert.AreEqual(Range(1, 15), SimpleTextTrimmer.GetCharacterBoundaries(".!?#@£$%^&*()[]"));
            CollectionAssert.AreEqual(Range(1, 8), SimpleTextTrimmer.GetCharacterBoundaries("OxyPlot!"));
        }

        /// <summary>
        /// Tests word boundaries with random western characters.
        /// </summary>
        [Test]
        public void CharacterBoundariesWestern()
        {
            // non-combining
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, SimpleTextTrimmer.GetCharacterBoundaries("öüäßáéíóú"));

            // combining
            CollectionAssert.AreEqual(new[] { 2, 4, 6, 8, 10, 12, 14, 16 }, SimpleTextTrimmer.GetCharacterBoundaries("äöüáéíóú"));
        }

        /// <summary>
        /// Tests word boundaries with Zalgo text.
        /// </summary>
        [Test]
        public void CharacterBoundariesZalgo()
        {
            var zalgo = "O̴͙͇͔̺̓̐x̶̹̙͕͕͈͚̫͂̇̑̌̒́̄͐̽̇̕y̴̡͕͗̔̆̈́̓̈́̚͝͠P̸̼̜̯͕̜̟̞̥̦͓̦̙̦͕̜̹͌̅̐̉̑̕̚ḽ̷̢͈͉͙̬̫̔͋̋͆͐͝o̷̘̪̒̏̈͊̂̿̀̀̒͗̿̅̀̉̑t̵̨̧̙͖͈̲̬͚̤̦͎͈̣̀͊";

            Assert.AreEqual(7, SimpleTextTrimmer.GetCharacterBoundaries(zalgo).Count);
        }

        /// <summary>
        /// Tests word boundaries with UTF-16 surrogate pairs.
        /// </summary>
        [Test]
        public void CharacterBoundariesSurrogatePairs()
        {
            // TODO: more tests, ideally written by someone who knows about this stuff
            CollectionAssert.AreEqual(new[] { 2 }, SimpleTextTrimmer.GetCharacterBoundaries("\uD852\uDF62"));
        }

        /// <summary>
        /// Tests word boundaries with UTF-16 surrogate pairs.
        /// </summary>
        [Test]
        public void CharacterBoundariesEmoji()
        {
            // TODO: more tests, ideally written by someone who knows about this stuff

            // one
            CollectionAssert.AreEqual(new[] { 2 }, SimpleTextTrimmer.GetCharacterBoundaries("📊"));

            // with zero width joiner
            CollectionAssert.AreEqual(new[] { 5 }, SimpleTextTrimmer.GetCharacterBoundaries("👨‍🦰"));

            // mixed
            CollectionAssert.AreEqual(new[] { 2, 4, 9, 10 }, SimpleTextTrimmer.GetCharacterBoundaries("📊📈👨‍🦰."));
        }

        /// <summary>
        /// Tests word boundaries with only whitespace.
        /// </summary>
        [Test]
        public void WordBoundariesNoWord()
        {
            var empty = new int[] { };
            CollectionAssert.AreEqual(empty, SimpleTextTrimmer.GetWordBoundaries(""));
            CollectionAssert.AreEqual(empty, SimpleTextTrimmer.GetWordBoundaries("\t"));
            CollectionAssert.AreEqual(empty, SimpleTextTrimmer.GetWordBoundaries("  "));
            CollectionAssert.AreEqual(empty, SimpleTextTrimmer.GetWordBoundaries("\r\n"));
        }

        /// <summary>
        /// Tests word boundaries with only one word.
        /// </summary>
        [Test]
        public void WordBoundariesOneWord()
        {
            CollectionAssert.AreEqual(new[] { 1 }, SimpleTextTrimmer.GetWordBoundaries("A"));
            CollectionAssert.AreEqual(new[] { 7 }, SimpleTextTrimmer.GetWordBoundaries("OxyPlot"));
            CollectionAssert.AreEqual(new[] { 8 }, SimpleTextTrimmer.GetWordBoundaries(" OxyPlot "));
            CollectionAssert.AreEqual(new[] { 9 }, SimpleTextTrimmer.GetWordBoundaries("  OxyPlot  "));
        }

        /// <summary>
        /// Tests word boundaries with boring ascii text.
        /// </summary>
        [Test]
        public void WordBoundariesAscii()
        {
            CollectionAssert.AreEqual(new[] { 7, 10, 12, 21, 30 }, SimpleTextTrimmer.GetWordBoundaries("OxyPlot is a plotting library."));
            CollectionAssert.AreEqual(new[] { 8, 12, 15, 25, 35 }, SimpleTextTrimmer.GetWordBoundaries(" OxyPlot  is  a  plotting  library.  "));
        }

        /// <summary>
        /// Tests word boundaries with western characters.
        /// </summary>
        [Test]
        public void WordBoundariesWestern()
        {
            // non-combining
            CollectionAssert.AreEqual(new[] { 3, 5, 11, 15 }, SimpleTextTrimmer.GetWordBoundaries("öäü ß áéíóú €20"));

            // combining
            CollectionAssert.AreEqual(new[] { 6, 17 }, SimpleTextTrimmer.GetWordBoundaries("äöü áéíóú"));
        }

        /// <summary>
        /// Tests word boundaries with a mix of whitespace.
        /// </summary>
        [Test]
        public void WordBoundariesWhiteSpace()
        {
            CollectionAssert.AreEqual(new[] { 7, 10, 12, 21, 31 }, SimpleTextTrimmer.GetWordBoundaries("OxyPlot\tis\na plotting  library.\r\n"));
        }

        /// <summary>
        /// Comvenience method to return an array of sequential integers.
        /// </summary>
        /// <param name="start">The first value, if any, in the array.</param>
        /// <param name="count">The number of values in this array.</param>
        /// <returns>The array.</returns>
        private static int[] Range(int start, int count)
        {
            return Enumerable.Range(start, count).ToArray();
        }

        /// <summary>
        /// Tests the <see cref="MockTextMeasurer"/>.
        /// </summary>
        [Test]
        public void MockTextMeasurerMeasureTextWidth()
        {
            var textMeasurer = new MockTextMeasurer();

            var simpleTestString = "OxyPlot"; // 7 basic chars
            var trickyTestString = "áéíóú"; // 10 chars, 5 characters with width
            var fontSize = 10.0;

            Assert.AreEqual(0.0, textMeasurer.MeasureTextWidth(simpleTestString, null, fontSize, 500));
            Assert.AreEqual(0.0, textMeasurer.MeasureTextWidth(trickyTestString, null, fontSize, 500));

            textMeasurer.AddBasicAlphabet();

            Assert.AreEqual(fontSize * textMeasurer.CharacterWidth * 7, textMeasurer.MeasureTextWidth(simpleTestString, null, fontSize, 10));
            Assert.AreEqual(fontSize * textMeasurer.CharacterWidth * 5, textMeasurer.MeasureTextWidth(trickyTestString, null, fontSize, 10));
        }

        /// <summary>
        /// Basics tests of character trimming.
        /// </summary>
        [Test]
        public void TrimmingByChar()
        {
            var textMeasurer = new MockTextMeasurer();
            textMeasurer.AddBasicAlphabet();
            textMeasurer.AddEllipsisChars();

            var trimmer = new SimpleTextTrimmer();

            trimmer.AppendEllipsis = false;
            trimmer.TrimToWord = false;

            var simpleTestString = "OxyPlot"; // 7 basic chars
            var trickyTestString = "áéíóúaa"; // 12 chars, 5 characters with width
            var fontSize = 10.0;

            var widthZero = textMeasurer.MeasureTextWidth("", null, fontSize, 500);
            var widthNearZero = textMeasurer.MeasureTextWidth("a", null, fontSize, 500) / 10.0;

            var widthSmall = textMeasurer.MeasureTextWidth("aaa", null, fontSize, 500);
            var widthNearSmall = textMeasurer.MeasureTextWidth("aaa", null, fontSize, 500) + widthNearZero;

            var widthLarge = textMeasurer.MeasureTextWidth("aaaaaaa", null, fontSize, 500);
            var widthVeryLarge = widthLarge * 2.0;

            Assert.AreEqual("", trimmer.Trim(textMeasurer, simpleTestString, widthZero, null, fontSize, 500));
            Assert.AreEqual("", trimmer.Trim(textMeasurer, trickyTestString, widthZero, null, fontSize, 500));

            Assert.AreEqual("", trimmer.Trim(textMeasurer, simpleTestString, widthNearZero, null, fontSize, 500));
            Assert.AreEqual("", trimmer.Trim(textMeasurer, trickyTestString, widthNearZero, null, fontSize, 500));

            Assert.AreEqual(simpleTestString.Substring(0, 3), trimmer.Trim(textMeasurer, simpleTestString, widthSmall, null, fontSize, 500));
            Assert.AreEqual(trickyTestString.Substring(0, 6), trimmer.Trim(textMeasurer, trickyTestString, widthSmall, null, fontSize, 500));

            Assert.AreEqual(simpleTestString.Substring(0, 3), trimmer.Trim(textMeasurer, simpleTestString, widthNearSmall, null, fontSize, 500));
            Assert.AreEqual(trickyTestString.Substring(0, 6), trimmer.Trim(textMeasurer, trickyTestString, widthNearSmall, null, fontSize, 500));

            Assert.AreEqual(simpleTestString, trimmer.Trim(textMeasurer, simpleTestString, widthLarge, null, fontSize, 500));
            Assert.AreEqual(trickyTestString, trimmer.Trim(textMeasurer, trickyTestString, widthLarge, null, fontSize, 500));

            Assert.AreEqual(simpleTestString, trimmer.Trim(textMeasurer, simpleTestString, widthVeryLarge, null, fontSize, 500));
            Assert.AreEqual(trickyTestString, trimmer.Trim(textMeasurer, trickyTestString, widthVeryLarge, null, fontSize, 500));
        }

        /// <summary>
        /// Basics tests of word trimming.
        /// </summary>
        [Test]
        public void TrimmingByWord()
        {
            var textMeasurer = new MockTextMeasurer();
            textMeasurer.AddBasicAlphabet();
            textMeasurer.AddEllipsisChars();
            textMeasurer.AddBasicWhitespace();

            var trimmer = new SimpleTextTrimmer();

            trimmer.TrimToWord = true;

            var text = "OxyPlot is a multiplatform plotting library";
            var fontSize = 10.0;
            var oneChar = textMeasurer.CharacterWidth * fontSize;
            var tiny = textMeasurer.CharacterWidth * fontSize / 10;

            var previousTarget = "";
            int i = 0;
            while (++i < text.Length && (i = text.IndexOf(' ', i)) > 0)
            {
                var target = text.Substring(0, i);
                var width = textMeasurer.MeasureTextWidth(target, null, fontSize, 500);
                var widthWithEllipsis = textMeasurer.MeasureTextWidth(target + trimmer.Ellipsis, null, fontSize, 500);

                trimmer.AppendEllipsis = false;
                Assert.AreEqual(previousTarget, trimmer.Trim(textMeasurer, text, width - tiny - oneChar, null, fontSize, 500));
                Assert.AreEqual(previousTarget, trimmer.Trim(textMeasurer, text, width - tiny, null, fontSize, 500));
                Assert.AreEqual(target, trimmer.Trim(textMeasurer, text, width, null, fontSize, 500));
                Assert.AreEqual(target, trimmer.Trim(textMeasurer, text, width + tiny, null, fontSize, 500));
                Assert.AreEqual(target, trimmer.Trim(textMeasurer, text, width + tiny + oneChar, null, fontSize, 500));

                trimmer.AppendEllipsis = true;
                Assert.AreEqual(previousTarget + trimmer.Ellipsis, trimmer.Trim(textMeasurer, text, widthWithEllipsis - tiny - oneChar, null, fontSize, 500));
                Assert.AreEqual(previousTarget + trimmer.Ellipsis, trimmer.Trim(textMeasurer, text, widthWithEllipsis - tiny, null, fontSize, 500));
                Assert.AreEqual(target + trimmer.Ellipsis, trimmer.Trim(textMeasurer, text, widthWithEllipsis, null, fontSize, 500));
                Assert.AreEqual(target + trimmer.Ellipsis, trimmer.Trim(textMeasurer, text, widthWithEllipsis + tiny, null, fontSize, 500));
                Assert.AreEqual(target + trimmer.Ellipsis, trimmer.Trim(textMeasurer, text, widthWithEllipsis + tiny + oneChar, null, fontSize, 500));

                previousTarget = target;
            }
        }
    }
}
