using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;

namespace SharpCanvas.Context.Skia
{
    public class SkiaLinearCanvasGradient : ILinearCanvasGradient
    {
        private readonly SKPoint _start;
        private readonly SKPoint _end;
        private readonly List<(float, SKColor)> _colorStops = new List<(float, SKColor)>();

        public SkiaLinearCanvasGradient(SKPoint start, SKPoint end)
        {
            _start = start;
            _end = end;
        }

        public void addColorStop(double offset, string color)
        {
            if (offset < 0 || offset > 1)
            {
                // The spec requires an INDEX_SIZE_ERR DOMException.
                // Throwing ArgumentOutOfRangeException is a reasonable C# equivalent.
                throw new System.ArgumentOutOfRangeException(nameof(offset), "Offset must be between 0 and 1.");
            }
            _colorStops.Add(((float)offset, ColorParser.Parse(color)));
            // The spec requires that stops at the same offset are ordered by insertion time.
            // List.Sort is stable, which preserves insertion order for equal elements.
            _colorStops.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }

        public object GetBrush()
        {
            return GetShader();
        }

        public SKShader GetShader()
        {
            if (_colorStops.Count == 0)
            {
                // If there are no stops, the gradient is transparent black.
                // We can achieve this by creating a gradient with two transparent stops.
                var transparent = SKColors.Transparent;
                return SKShader.CreateLinearGradient(_start, _end, new[] { transparent, transparent }, new[] { 0f, 1f }, SKShaderTileMode.Clamp);
            }

            var colors = new SKColor[_colorStops.Count];
            var positions = new float[_colorStops.Count];

            for (int i = 0; i < _colorStops.Count; i++)
            {
                colors[i] = _colorStops[i].Item2;
                positions[i] = _colorStops[i].Item1;
            }

            // SKShaderTileMode.Clamp ensures that the color before the first stop is the first stop's color,
            // and the color after the last stop is the last stop's color, which matches the spec.
            return SKShader.CreateLinearGradient(_start, _end, colors, positions, SKShaderTileMode.Clamp);
        }
    }
}
