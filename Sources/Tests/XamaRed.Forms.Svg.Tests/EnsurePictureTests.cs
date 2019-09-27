using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace XamaRed.Forms.Svg.Tests
{
    [TestClass]
    public class EnsurePictureTests
    {
        [TestCleanup]
        public void Reset()
        {
            SvgView.ResourceIdsPrefix = null;
            SvgView.MainPclAssembly = null;
            IDictionary<string, SKSvg> cache = typeof(SvgView).GetStaticMemberValue<IDictionary<string, SKSvg>>("SvgCache");
            cache.Clear();
        }

        [TestMethod]
        public void EnsurePicture_AlreadyResolved()
        {
            SvgView view = new SvgView();
            SKSvg skSvg = new SKSvg();
            view.SetMemberValue("_skSvg", skSvg);
            view.CallPrivateMethod("EnsurePicture");
            Assert.AreEqual(skSvg, view.GetMemberValue<SKSvg>("_skSvg"));
        }

        [TestMethod]
        public void EnsurePicture_NoApplication()
        {
            TestHelpers.HandleInvocationException(() =>
            {
                SvgView view = new SvgView { ResourceId = "DoesNotExist" };
                view.CallPrivateMethod("EnsurePicture");
            }, typeof(InvalidOperationException));
        }

        [TestMethod]
        public void EnsurePicture_NotFound()
        {
            TestHelpers.HandleInvocationException(() =>
            {
                SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
                SvgView view = new SvgView { ResourceId = "DoesNotExist" };
                view.CallPrivateMethod("EnsurePicture");
            }, typeof(FileNotFoundException));
        }

        [TestMethod]
        public void EnsurePicture_Simple()
        {
            SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
            SvgView view = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg" };
            view.CallPrivateMethod("EnsurePicture");
            Assert.IsNotNull(view.GetMemberValue<SKSvg>("_skSvg"));
            Assert.IsNotNull(view.GetMemberValue<SKRect>("_svgRect"));
        }

        [TestMethod]
        public void EnsurePicture_CacheAdd()
        {
            SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
            SvgView view = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg" };
            view.CallPrivateMethod("EnsurePicture");
            IDictionary<string, SKSvg> cache = typeof(SvgView).GetStaticMemberValue<IDictionary<string, SKSvg>>("SvgCache");
            Assert.AreEqual(1, cache.Count);

        }

        [TestMethod]
        public void EnsurePicture_CacheUse()
        {
            SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
            SvgView viewer1 = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg" };
            viewer1.CallPrivateMethod("EnsurePicture");
            SvgView viewer2 = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg" };
            viewer2.CallPrivateMethod("EnsurePicture");
            IDictionary<string, SKSvg> cache = typeof(SvgView).GetStaticMemberValue<IDictionary<string, SKSvg>>("SvgCache");
            Assert.AreEqual(1, cache.Count);
        }

        [TestMethod]
        public void EnsurePicture_CacheOther()
        {
            SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
            SvgView viewer1 = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg" };
            viewer1.CallPrivateMethod("EnsurePicture");
            SvgView viewer2 = new SvgView { ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape2.svg" };
            viewer2.CallPrivateMethod("EnsurePicture");
            IDictionary<string, SKSvg> cache = typeof(SvgView).GetStaticMemberValue<IDictionary<string, SKSvg>>("SvgCache");
            Assert.AreEqual(2, cache.Count);
        }

        [TestMethod]
        public void EnsurePicture_PrefixResolve()
        {
            SvgView.ResourceIdsPrefix = "XamaRed.Forms.Svg.Tests.Assets.";
            SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
            SvgView view = new SvgView { ResourceId = "inkscape.svg" };
            view.CallPrivateMethod("EnsurePicture");
        }

        [TestMethod]
        public void EnsurePicture_PrefixAbsolute()
        {
            TestHelpers.HandleInvocationException(() =>
            {
                SvgView.ResourceIdsPrefix = "XamaRed.Forms.Svg.Tests.Assets.";
                SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
                SvgView view = new SvgView {ResourceId = "XamaRed.Forms.Svg.Tests.Assets.inkscape.svg"};
                view.CallPrivateMethod("EnsurePicture");
            }, typeof(FileNotFoundException));
        }
    }
}
