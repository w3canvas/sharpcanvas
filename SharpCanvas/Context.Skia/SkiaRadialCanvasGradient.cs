#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace SharpCanvas.Context.Skia
{
    public class SkiaRadialCanvasGradient : IPathCanvasGradient
    {
        private readonly SKPoint _start;
        private readonly float _startRadius;
        private readonly SKPoint _end;
        private readonly float _endRadius;
        private readonly List<(float offset, SKColor color)> _colorStops = new List<(float, SKColor)>();

        public SkiaRadialCanvasGradient(SKPoint start, float startRadius, SKPoint end, float endRadius)
        {
            _start = start;
            _startRadius = startRadius;
            _end = end;
            _endRadius = endRadius;
        }

        public void addColorStop(float offset, string color)
        {
            if (offset < 0 || offset > 1)
            {
                throw new System.ArgumentOutOfRangeException(nameof(offset), "Offset must be between 0 and 1.");
            }
            _colorStops.Add((offset, ColorParser.Parse(color)));
            _colorStops.Sort((a, b) => a.offset.CompareTo(b.offset));
        }

        public void addColorStop(double offset, string color)
        {
            addColorStop((float)offset, color);
        }

        public void addColorStop(uint offset, string color)
        {
            addColorStop((float)offset, color);
        }

        public object GetBrush()
        {
            return GetShader();
        }

        public SKShader GetShader()
        {
            if (_colorStops.Count < 2)
            {
                // Gradient requires at least two colors. If we have one, just use that color. If we have none, transparent.
                return SKShader.CreateColor(_colorStops.FirstOrDefault().color);
            }

            var colors = _colorStops.Select(cs => cs.color).ToArray();
            var positions = _colorStops.Select(cs => cs.offset).ToArray();

            // Check if this is an off-center radial gradient (different centers for inner and outer circles)
            var dx = _end.X - _start.X;
            var dy = _end.Y - _start.Y;
            var centerDistance = (float)System.Math.Sqrt(dx * dx + dy * dy);

            // For off-center gradients, the Canvas API behavior differs from Skia's CreateTwoPointConicalGradient.
            // We need to swap the circles and reverse the colors to get the correct gradient direction.
            // This is because Skia's algorithm may calculate the offset differently for non-concentric circles.
            if (centerDistance > 0.0001f && System.Math.Abs(_startRadius - _endRadius) > 0.0001f)
            {
                // Swap the circles and reverse the colors to correct the gradient direction
                return SKShader.CreateTwoPointConicalGradient(
                    _end, _endRadius,
                    _start, _startRadius,
                    colors.Reverse().ToArray(),
                    positions.Reverse().ToArray(),
                    SKShaderTileMode.Clamp);
            }

            return SKShader.CreateTwoPointConicalGradient(_start, _startRadius, _end, _endRadius, colors, positions, SKShaderTileMode.Clamp);
        }
    }
}
