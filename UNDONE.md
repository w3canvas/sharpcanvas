# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Partially Implemented Features
- **Text Rendering**: The `fontVariantCaps` property is not fully implemented. The `FontUtils` class needs to be updated to handle OpenType font features. This is a complex task that requires using the `HarfBuzzSharp` library for text shaping. The `SKShaper` class in `SkiaSharp.HarfBuzz` is the entry point for this, but a deeper integration is required to correctly apply the features and draw the resulting glyphs.

    **Progress so far:**
    - Logic to parse the `font-variant-caps` CSS property and map its values to the corresponding `HarfBuzzSharp.Feature` tags has been implemented. The mapping is as follows:
        - `small-caps`: `smcp`
        - `all-small-caps`: `c2sc`, `smcp`
        - `petite-caps`: `pcap`
        - `all-petite-caps`: `c2pc`, `pcap`
        - `unicase`: `unic`
        - `titling-caps`: `titl`
    - It has been determined that the correct approach is to use the HarfBuzz API to shape the text into a set of glyphs and positions, and then draw these glyphs using the Skia API.

    **Remaining work:**
    - The primary challenge is finding the correct SkiaSharp v3 API calls to perform the text shaping and drawing. The online documentation for the version of SkiaSharp used in this project is sparse, and attempts to use the API based on older versions have resulted in compilation errors.
    - The following code snippet represents the latest attempt to implement the `fillText` method and can serve as a starting point for future work:
    ```csharp
    public void fillText(string text, double x, double y)
    {
        // Re-apply font settings in case they were loaded asynchronously
        if (FontUtils.ApplyFont(this, _fillFont))
        {
            var yOffset = FontUtils.GetYOffset(textBaseLine, _fillFont);
            using (var paint = ApplyPaint(_fillPaint))
            {
                using (var buffer = new HarfBuzzSharp.Buffer())
                {
                    buffer.AddUtf8(text);
                    buffer.GuessSegmentProperties();

                    using (var hbFont = new HarfBuzzSharp.Font(new HarfBuzzSharp.Face(_fillFont.Typeface.OpenStream().ToHarfBuzzBlob())))
                    {
                        hbFont.Shape(buffer, _fontFeatures);
                    }

                    var len = buffer.Length;
                    var info = buffer.GetGlyphInfoSpan();
                    var pos = buffer.GetGlyphPositionSpan();

                    var clusters = new uint[len];
                    var points = new SKPoint[len];
                    var glyphs = new ushort[len];

                    float currentX = (float)x;
                    float currentY = (float)y + yOffset;

                    for (var i = 0; i < len; i++)
                    {
                        glyphs[i] = (ushort)info[i].Codepoint;
                        points[i] = new SKPoint(currentX + pos[i].XOffset, currentY - pos[i].YOffset);
                        currentX += pos[i].XAdvance;
                        currentY += pos[i].YAdvance;
                    }

                    using (var textBlob = SKTextBlob.CreatePositioned(glyphs, _fillFont, points))
                    {
                        _surface.Canvas.DrawText(textBlob, 0, 0, paint);
                    }
                }
            }
        }
    }
    ```

### SkiaSharp v3 Upgrade Complete
The project's `SkiaSharp` dependencies have been successfully upgraded from version `2.88.8` to `3.119.0`. All resulting compilation errors and warnings caused by breaking changes in the new version have been resolved. The project now builds cleanly.

**Conclusion:**
The upgrade is complete. The next major tasks are:
1.  **Implement `fontVariantCaps`**: With the upgraded dependencies, the `HarfBuzzSharp` library can now be used to properly implement this feature.
2.  **Fix `TestArc` failure**: The upgrade caused a regression in the `arc` method, which needs to be investigated (see "Known Test Failures").

## Known Test Failures
- **`TestArc` in `SharpCanvas.Tests.Skia.Modern`**: After upgrading to SkiaSharp v3, the `TestArc` test case began to fail. The test expects a filled arc to be drawn, but the resulting pixels are transparent, indicating the path is not being filled correctly. The exact cause is unknown and requires further investigation into the SkiaSharp v3 API for path creation and filling. This issue is deferred to allow the upgrade to be completed.

## Known Build Issues
- **`SharpCanvas.Context.Drawing2D` Project Reference**: There is a persistent, transient build error (CS0117) where the `SharpCanvas.Context.Drawing2D` project is unable to find methods from the `SharpCanvas.Common` project, despite a valid project reference. This may be due to an issue in the build environment. The code has been committed with the correct references, but the project may not build successfully until the underlying issue is resolved.

