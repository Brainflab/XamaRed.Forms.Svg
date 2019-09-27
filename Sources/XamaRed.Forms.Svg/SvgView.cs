using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace XamaRed.Forms.Svg
{
    /// <summary>
    /// Control which displays a simple SVG image
    /// </summary>
    public class SvgView : SKCanvasView
    {
        private static readonly IDictionary<string, SKSvg> SvgCache = new Dictionary<string, SKSvg>();
        private SKSvg _skSvg;
        private SKRect _svgRect;

        /// <summary>
        /// Gets or sets the global prefix which will be added before every <see cref="SvgView"/> provided <see cref="ResourceId"/>
        /// </summary>
        public static string ResourceIdsPrefix { get; set; } = string.Empty;

        private static Assembly _mainPclAssembly;
        /// <summary>
        /// Gets or sets the PCL assembly containing the SVGs to load
        /// <remarks>If not set, it will use the application PCL assembly</remarks>
        /// </summary>
        public static Assembly MainPclAssembly
        {
            get
            {
                if (_mainPclAssembly == null)
                {
                    if (Application.Current == null)
                        throw new InvalidOperationException("SvgView must be used after the Application load (or you can set MainPclAssembly)");
                    _mainPclAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
                }
                return _mainPclAssembly;
            }
            set
            {
                _mainPclAssembly = value;
            }
        }

        #region Bindable properties

        /// <summary>
        /// <see cref="ResourceId"/> bindable property declaration
        /// </summary>
        public static readonly BindableProperty ResourceIdProperty = BindableProperty.Create("ResourceId", typeof(string), typeof(SvgView), default(string));

        /// <summary>
        /// Gets or sets the path of the SVG resource file
        /// <remarks>
        /// The SVG file must be included in a projet as an EmbeddedResource (can be set via the files properties).
        /// Other modes, such as downloading a remote SVG, are not supported
        /// The resource id must be composed of alpha characters and dots only. It should not contain slashes.
        /// </remarks>
        /// </summary>
        public string ResourceId
        {
            get { return (string)GetValue(ResourceIdProperty); }
            set { SetValue(ResourceIdProperty, value); }
        }

        /// <summary>
        /// <see cref="Stretch"/> property declaration
        /// </summary>
        public static readonly BindableProperty StretchProperty = BindableProperty.Create("Stretch", typeof(SvgStretch), typeof(SvgView), SvgStretch.Uniform);
        /// <summary>
        /// Gets or sets the SVG <see cref="SvgStretch"/> mode
        /// </summary>
        public SvgStretch Stretch
        {
            get { return (SvgStretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// <see cref="HorizontalAligment"/> property declaration
        /// </summary>
        public static readonly BindableProperty HorizontalAligmentProperty = BindableProperty.Create("HorizontalAligment", typeof(SvgAlignment), typeof(SvgView), default(SvgAlignment));
        /// <summary>
        /// Gets or sets the SVG horizontal alignment
        /// </summary>
        public SvgAlignment HorizontalAligment
        {
            get { return (SvgAlignment)GetValue(HorizontalAligmentProperty); }
            set { SetValue(HorizontalAligmentProperty, value); }
        }

        /// <summary>
        /// <see cref="VerticalAligment"/> property declaration
        /// </summary>
        public static readonly BindableProperty VerticalAligmentProperty = BindableProperty.Create("VerticalAligment", typeof(SvgAlignment), typeof(SvgView), default(SvgAlignment));
        /// <summary>
        /// Gets or sets the SVG Vertical alignment
        /// </summary>
        public SvgAlignment VerticalAligment
        {
            get { return (SvgAlignment)GetValue(VerticalAligmentProperty); }
            set { SetValue(VerticalAligmentProperty, value); }
        }

        #endregion

        /// <summary>
        /// Does nothing, but might be useful to prevent dependencies removal
        /// </summary>
        public static void Init()
        {
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            e.Surface.Canvas.Clear();
            if (!string.IsNullOrEmpty(ResourceId))
            {
                EnsurePicture();
                SKMatrix scaleMatrix = GetScaleMatrix(e.Info);
                SKMatrix translationMatrix = GetTranslationMatrix(e.Info, scaleMatrix);
                SKMatrix.PostConcat(ref scaleMatrix, translationMatrix);
                e.Surface.Canvas.DrawPicture(_skSvg.Picture, ref scaleMatrix);
            }
        }

        private SKMatrix GetScaleMatrix(SKImageInfo canvasInfo)
        {
            if (Stretch == SvgStretch.None || _svgRect.Width < 0.00001f || _svgRect.Height < 0.0000001f)
                return SKMatrix.MakeIdentity();
            float widthRatio = canvasInfo.Width / _svgRect.Width;
            float heightRatio = canvasInfo.Height / _svgRect.Height;
            switch (Stretch)
            {
                case SvgStretch.Uniform:
                    widthRatio = heightRatio = Math.Min(widthRatio, heightRatio);
                    break;
                case SvgStretch.UniformToFill:
                    widthRatio = heightRatio = Math.Max(widthRatio, heightRatio);
                    break;
                case SvgStretch.Fill:
                    // Do nothing, use dimension ratios
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return SKMatrix.MakeScale(widthRatio, heightRatio);
        }

        private SKMatrix GetTranslationMatrix(SKImageInfo canvasInfo, SKMatrix scaleMatrix)
        {
            SKRect scaledSvgBounds = scaleMatrix.MapRect(_svgRect);
            float xTranslation = GetTranslation(canvasInfo.Width, scaledSvgBounds.Width, HorizontalAligment);
            float yTranslation = GetTranslation(canvasInfo.Height, scaledSvgBounds.Height, VerticalAligment);
            return SKMatrix.MakeTranslation(xTranslation, yTranslation);
        }

        private float GetTranslation(float canvasDimension, float svgDimension, SvgAlignment alignment)
        {
            float remainingSpace = canvasDimension - svgDimension;
            float translation;
            switch (alignment)
            {
                case SvgAlignment.Start:
                    translation = 0;
                    break;
                case SvgAlignment.Middle:
                    translation = remainingSpace / 2;
                    break;
                case SvgAlignment.End:
                    translation = remainingSpace;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
            }
            return translation;
        }

        private void EnsurePicture()
        {
            if (_skSvg != null)
                return;
            if (!SvgCache.TryGetValue(ResourceId, out _skSvg))
            {
                string fullKey = $"{ResourceIdsPrefix}{ResourceId}";
                using (Stream stream = MainPclAssembly.GetManifestResourceStream(fullKey))
                {
                    if (stream == null)
                        throw new FileNotFoundException($"SvgView : could not load SVG file {fullKey} in assembly {MainPclAssembly}. Make sure the ID is correct, the file is there and it is set to Embedded Resource build action.");
                    _skSvg = new SKSvg();
                    _skSvg.Load(stream);
                    SvgCache.Add(ResourceId, _skSvg);
                }
            }
            _svgRect = _skSvg.Picture.CullRect;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ResourceIdProperty.PropertyName
                || propertyName == HorizontalAligmentProperty.PropertyName
                || propertyName == VerticalAligmentProperty.PropertyName
                || propertyName == StretchProperty.PropertyName)
            {
                if (propertyName == ResourceIdProperty.PropertyName)
                    _skSvg = null;
                InvalidateSurface();
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Stretch == SvgStretch.None || double.IsInfinity(widthConstraint) && double.IsInfinity(heightConstraint))
            {
                EnsurePicture();
                return new SizeRequest(new Size(_svgRect.Width, _svgRect.Height));
            }
            return base.OnMeasure(widthConstraint, heightConstraint);
        }
    }
}
