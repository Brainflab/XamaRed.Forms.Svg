using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;

namespace XamaRed.Forms.Svg.Tests
{
    [TestClass]
    public class GetScaleTests
    {
        [TestMethod]
        public void GetScaleMatrix_Zero()
        {
            TestGetScale(SvgStretch.Fill, 100, 200, 0, 0, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_None_XS_YS()
        {
            TestGetScale(SvgStretch.None, 200, 150, 150, 75, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_None_XS_YB()
        {
            TestGetScale(SvgStretch.None, 200, 150, 150, 250, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_None_XB_YS()
        {
            TestGetScale(SvgStretch.None, 150, 150, 200, 75, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_None_XB_YB()
        {
            TestGetScale(SvgStretch.None, 100, 20, 120, 80, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_None_Same()
        {
            TestGetScale(SvgStretch.None, 11, 11, 11, 11, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_Fill_XS_YS()
        {
            TestGetScale(SvgStretch.Fill, 200, 150, 150, 75, 4f / 3, 2);
        }

        [TestMethod]
        public void GetScaleMatrix_Fill_XS_YB()
        {
            TestGetScale(SvgStretch.Fill, 200, 150, 150, 250, 4f / 3, 3f / 5);
        }

        [TestMethod]
        public void GetScaleMatrix_Fill_XB_YS()
        {
            TestGetScale(SvgStretch.Fill, 150, 150, 200, 75, 0.75f, 2);
        }

        [TestMethod]
        public void GetScaleMatrix_Fill_XB_YB()
        {
            TestGetScale(SvgStretch.Fill, 100, 20, 120, 80, 5f / 6, 0.25f);
        }

        [TestMethod]
        public void GetScaleMatrix_Fill_Same()
        {
            TestGetScale(SvgStretch.Fill, 11, 11, 11, 11, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_Uniform_XS_YS()
        {
            TestGetScale(SvgStretch.Uniform, 200, 150, 150, 75, 4f / 3, 4f / 3);
        }

        [TestMethod]
        public void GetScaleMatrix_Uniform_XS_YB()
        {
            TestGetScale(SvgStretch.Uniform, 200, 150, 150, 250, 3f / 5, 3f / 5);
        }

        [TestMethod]
        public void GetScaleMatrix_Uniform_XB_YS()
        {
            TestGetScale(SvgStretch.Uniform, 150, 150, 200, 75, 0.75f, 0.75f);
        }

        [TestMethod]
        public void GetScaleMatrix_Uniform_XB_YB()
        {
            TestGetScale(SvgStretch.Uniform, 100, 20, 120, 80, 0.25f, 0.25f);
        }

        [TestMethod]
        public void GetScaleMatrix_Uniform_Same()
        {
            TestGetScale(SvgStretch.Uniform, 11, 11, 11, 11, 1, 1);
        }

        [TestMethod]
        public void GetScaleMatrix_UniformToFill_XS_YS()
        {
            TestGetScale(SvgStretch.UniformToFill, 200, 150, 150, 75, 2, 2);
        }

        [TestMethod]
        public void GetScaleMatrix_UniformToFill_XS_YB()
        {
            TestGetScale(SvgStretch.UniformToFill, 200, 150, 150, 250, 4f / 3, 4f / 3);
        }

        [TestMethod]
        public void GetScaleMatrix_UniformToFill_XB_YS()
        {
            TestGetScale(SvgStretch.UniformToFill, 150, 150, 200, 75, 2f, 2f);
        }

        [TestMethod]
        public void GetScaleMatrix_UniformToFill_XB_YB()
        {
            TestGetScale(SvgStretch.UniformToFill, 100, 20, 120, 80, 5f / 6, 5f / 6);
        }

        [TestMethod]
        public void GetScaleMatrix_UniformToFill_Same()
        {
            TestGetScale(SvgStretch.UniformToFill, 11, 11, 11, 11, 1, 1);
        }

        private void TestGetScale(SvgStretch stretch, int canvasWidth, int canvasHeight, float svgWidth, float svgHeight, float expectedScaleX, float expectedScaleY)
        {
            SvgView svgView = new SvgView { Stretch = stretch };
            svgView.SetMemberValue("_svgRect", new SKRect(0, 0, svgWidth, svgHeight));
            SKImageInfo canvasInfo = new SKImageInfo(canvasWidth, canvasHeight);
            SKMatrix result = svgView.CallPrivateMethod<SKMatrix>("GetScaleMatrix", canvasInfo);
            Assert.AreEqual(expectedScaleX, result.ScaleX);
            Assert.AreEqual(expectedScaleY, result.ScaleY);
        }
    }
}
