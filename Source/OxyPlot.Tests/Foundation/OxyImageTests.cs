namespace OxyPlot.Tests
{
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class OxyImageTests
    {
        [Test]
        public void FromArgb()
        {
            var data = new OxyColor[2, 4];
            data[1, 0] = OxyColors.Blue;
            data[1, 1] = OxyColors.Green;
            data[1, 2] = OxyColors.Red;
            data[1, 3] = OxyColors.White;
            data[0, 0] = OxyColors.Yellow.ChangeAlpha(127);
            data[0, 1] = OxyColors.Orange.ChangeAlpha(127);
            data[0, 2] = OxyColors.Pink.ChangeAlpha(127);
            data[0, 3] = OxyColors.Transparent;
            var img = OxyImage.FromArgb(data);
            var bytes = img.GetData();
            File.WriteAllBytes("FromArgb.bmp", bytes);
        }

        [Test]
        public void PngFromArgb()
        {
            var data = new OxyColor[2, 4];
            data[1, 0] = OxyColors.Blue;
            data[1, 1] = OxyColors.Green;
            data[1, 2] = OxyColors.Red;
            data[1, 3] = OxyColors.White;
            data[0, 0] = OxyColors.Yellow.ChangeAlpha(127);
            data[0, 1] = OxyColors.Orange.ChangeAlpha(127);
            data[0, 2] = OxyColors.Pink.ChangeAlpha(127);
            data[0, 3] = OxyColors.Transparent;
            var img = OxyImage.PngFromArgb(data);
            var bytes = img.GetData();
            File.WriteAllBytes("PngFromArgb.png", bytes);
        }

        [Test]
        public void PngFromArgb2()
        {
            int w = 266;
            int h = 40;
            var data = new OxyColor[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    data[i, j] = OxyColor.FromHsv((double)j / w, 1, 1);
                }
            }

            var img = OxyImage.PngFromArgb(data);
            var bytes = img.GetData();
            File.WriteAllBytes("PngFromArgb2.png", bytes);
        }

        [Test]
        public void FromArgbX()
        {
            var data = new OxyColor[2, 4];
            data[1, 0] = OxyColors.Blue;
            data[1, 1] = OxyColors.Green;
            data[1, 2] = OxyColors.Red;
            data[1, 3] = OxyColors.White;
            data[0, 0] = OxyColors.Yellow.ChangeAlpha(127);
            data[0, 1] = OxyColors.Orange.ChangeAlpha(127);
            data[0, 2] = OxyColors.Pink.ChangeAlpha(127);
            data[0, 3] = OxyColors.Transparent;
            var img = OxyImage.FromArgbX(data);
            var bytes = img.GetData();
            File.WriteAllBytes("FromArgbX.bmp", bytes);
        }

        [Test]
        public void FromIndexed8()
        {
            var data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var data2 = new byte[,] { { 0, 1, 2, 3 }, { 4, 5, 6, 7 } };

            var palette = new OxyColor[8];
            palette[4] = OxyColors.Blue;
            palette[5] = OxyColors.Green;
            palette[6] = OxyColors.Red;
            palette[7] = OxyColors.White;
            palette[0] = OxyColors.Yellow.ChangeAlpha(127);
            palette[1] = OxyColors.Orange.ChangeAlpha(127);
            palette[2] = OxyColors.Pink.ChangeAlpha(127);
            palette[3] = OxyColors.Transparent;
            var img = OxyImage.FromIndexed8(4, 2, data, palette);
            var bytes = img.GetData();
            File.WriteAllBytes("FromIndexed8.bmp", bytes);

            var img2 = OxyImage.FromIndexed8(data2, palette);
            var bytes2 = img2.GetData();
            File.WriteAllBytes("FromIndexed8_2.bmp", bytes2);
        }
    }
}