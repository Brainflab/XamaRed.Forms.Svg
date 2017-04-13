# XamaRed.Forms.Svg
XamaRed.Forms.Svg is simple SVG viewer control for Xamarin Forms.
It is based on [SkiaSharp](https://github.com/mono/SkiaSharp).
## Features
- Display simple SVG files directly from a PCL project
- Stretching options
- Horizontal and vertical alignment of the SVG inside its canvas
- Caching
## Download
This control is available as a Nuget package : `XamaRed.Forms.Svg`.
## Usage
- SVG files must be located in a PCL assembly, with `Embedded Resource` build action.
- The [resource identifier](https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/files/#Loading_Files_Embedded_as_Resources) of the file must be provided.

Simple image display :
````xml
<svg:SvgView ResourceId="MyProject.Assets.myfile.svg" />
````
Image display with custom stretch mode and alignment :
````xml
<svg:SvgView ResourceId="MyProject.Assets.myfile.svg" VerticalAligment="Middle" HorizontalAligment="Middle" Stretch="UniformToFill" />
````
## About stretch mode and alignment
The stretch modes are the same as in WPF :
- `Uniform` : uniform stretch the SVG as much as possible while keeping the SVG fully visible
- `UniformToFill` : uniform stretch the SVG as much as possible until the parent is fully filled with the SVG. Some parts of the SVG will probably be hidden
- `Fill` : stretch the SVG so it matches its parent width and height. The SVG will probably look distorted.
- `None` : no stretch. Uses the SVG viewbox dimensions.

The alignment values match the ones in Xamarin Forms :
- `Start`
- `Middle` 
- `End`

Stretch and Horizontal/Vertical alignments should be used together in order to obtain the intended rendering.

| Stretch       | Horizontal alignment | Vertical alignment | Result example                           |
|---------------|----------------------|--------------------|------------------------------------------|
| Uniform       | Start                | (any)              | ![example1](ReadmeExamples/example1.png) |
| UniformToFill | (any)                | End                | ![example2](ReadmeExamples/example2.png) |
| None          | Middle               | Middle             | ![example3](ReadmeExamples/example3.png) |

Please check the included samples for more examples.

## More advanced features
The assembly containing the SVG files will default to the Xamarin Forms application assembly. It is possible to override this behavior by setting the `MainPclAssembly` static property.
In order to simplify the resource identifiers, it is also possible to set a default prefix which will be added in front of all identifiers of all `SvgView` instances. This is done through the `ResourceIdsPrefix` static property.

```csharp
SvgView.ResourceIdsPrefix = "XamaRed.Forms.Svg.Tests.Assets.";
SvgView.MainPclAssembly = typeof(EnsurePictureTests).Assembly;
```
````xml
<svg:SvgView ResourceId="inkscape.svg" />
````

## Limitations
The SVG support of Skia is currently limited to basic features.
Advanced SVG files may not render correctly.
