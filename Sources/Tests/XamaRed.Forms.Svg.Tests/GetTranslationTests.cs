using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XamaRed.Forms.Svg.Tests
{
    [TestClass]
    public class GetTranslationTests
    {
        [TestMethod]
        public void GetTranslation_SmallerStart()
        {
            TestGetTranslation(200, 150, SvgAlignment.Start, 0);
        }

        [TestMethod]
        public void GetTranslation_SmallerMiddle()
        {
            TestGetTranslation(200, 150, SvgAlignment.Middle, 25);
        }

        [TestMethod]
        public void GetTranslation_SmallerEnd()
        {
            TestGetTranslation(200, 150, SvgAlignment.End, 50);
        }

        [TestMethod]
        public void GetTranslation_BiggerStart()
        {
            TestGetTranslation(200, 250, SvgAlignment.Start, 0);
        }

        [TestMethod]
        public void GetTranslation_BiggerMiddle()
        {
            TestGetTranslation(200, 250, SvgAlignment.Middle, -25);
        }

        [TestMethod]
        public void GetTranslation_BiggerEnd()
        {
            TestGetTranslation(200, 250, SvgAlignment.End, -50);
        }

        [TestMethod]
        public void GetTranslation_EqualStart()
        {
            TestGetTranslation(250, 250, SvgAlignment.Start, 0);
        }

        [TestMethod]
        public void GetTranslation_EqualMiddle()
        {
            TestGetTranslation(250, 250, SvgAlignment.Middle, 0);
        }

        [TestMethod]
        public void GetTranslation_EqualEnd()
        {
            TestGetTranslation(250, 250, SvgAlignment.End, 0);
        }
        [TestMethod]
        public void GetTranslation_ZeroStart()
        {
            TestGetTranslation(250, 0, SvgAlignment.Start, 0);
        }

        [TestMethod]
        public void GetTranslation_ZeroMiddle()
        {
            TestGetTranslation(250, 0, SvgAlignment.Middle, 125);
        }

        [TestMethod]
        public void GetTranslation_ZeroEnd()
        {
            TestGetTranslation(250, 0, SvgAlignment.End, 250);
        }

        private void TestGetTranslation(float canvasDimension, float svgDimension, SvgAlignment alignment, float expected)
        {
            SvgView svgView = new SvgView();
            float result = svgView.CallPrivateMethod<float>("GetTranslation", canvasDimension, svgDimension, alignment);
            Assert.AreEqual(expected, result);
        }
    }
}
