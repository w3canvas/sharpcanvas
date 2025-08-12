#nullable enable
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace SharpCanvas.Context.Skia
{
    public class SkiaConicCanvasGradient
    {
        private readonly SKPoint _center;
        private readonly float _startAngle;
        private readonly List<(float offset, SKColor color)> _colorStops = new List<(float, SKColor)>();

        public SkiaConicCanvasGradient(float startAngle, SKPoint center)
        {
            _startAngle = startAngle;
            _center = center;
        }

        public void addColorStop(float offset, string color)
        {
            _colorStops.Add((offset, ColorParser.Parse(color)));
            _colorStops.Sort((a, b) => a.offset.CompareTo(b.offset));
        }

        public SKShader GetShader()
        {
            if (_colorStops.Count < 2)
            {
                return SKShader.CreateColor(_colorStops.FirstOrDefault().color);
            }

            var colors = _colorStops.Select(cs => cs.color).ToArray();
            var positions = _colorStops.Select(cs => cs.offset).ToArray();

            // The gradient is over 360 degrees
            return SKShader.CreateSweepGradient(_center, colors, positions, SKShaderTileMode.Clamp, _startAngle, _startAngle + 360);
        }
    }
}
