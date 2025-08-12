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
            _colorStops.Add((offset, ColorParser.Parse(color)));
            _colorStops.Sort((a, b) => a.offset.CompareTo(b.offset));
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

            return SKShader.CreateTwoPointConicalGradient(_start, _startRadius, _end, _endRadius, colors, positions, SKShaderTileMode.Clamp);
        }
    }
}
