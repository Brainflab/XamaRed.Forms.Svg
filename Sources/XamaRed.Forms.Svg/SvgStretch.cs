namespace XamaRed.Forms.Svg
{
    /// <summary>
    /// Stretch options for a svg, relative to its parent
    /// </summary>
    public enum SvgStretch
    {
        /// <summary>
        /// Does not stretch the SVG
        /// </summary>
        None,

        /// <summary>
        /// Uniform stretch as much as possible while keeping the SVG fully visible
        /// </summary>
        Uniform,

        /// <summary>
        /// Uniform stretch as much as possible until the parent is fully filled with the SVG. Some parts of the SVG will probably be hidden
        /// </summary>
        UniformToFill,

        /// <summary>
        /// Stretch the SVG so it matches its parent width and height. The SVG will probably look distorted.
        /// </summary>
        Fill
    }
}
