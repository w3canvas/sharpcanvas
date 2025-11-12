using SharpCanvas.Shared;
using SkiaSharp;
using System;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Path2D implementation for SkiaSharp backend.
    /// Provides reusable path objects for improved performance and cleaner code.
    /// </summary>
    public class Path2D : IPath2D
    {
        internal SKPath _path;

        /// <summary>
        /// Creates a new empty Path2D object.
        /// </summary>
        public Path2D()
        {
            _path = new SKPath();
        }

        /// <summary>
        /// Creates a new Path2D object from an existing path.
        /// </summary>
        public Path2D(Path2D path)
        {
            if (path == null)
            {
                _path = new SKPath();
            }
            else
            {
                _path = new SKPath(path._path);
            }
        }

        /// <summary>
        /// Creates a new Path2D object from an SVG path string.
        /// </summary>
        public Path2D(string svgPath)
        {
            _path = SKPath.ParseSvgPathData(svgPath);
            if (_path == null)
            {
                _path = new SKPath();
            }
        }

        public void moveTo(double x, double y)
        {
            _path.MoveTo((float)x, (float)y);
        }

        public void lineTo(double x, double y)
        {
            _path.LineTo((float)x, (float)y);
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            _path.QuadTo((float)cpx, (float)cpy, (float)x, (float)y);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            _path.CubicTo((float)cp1x, (float)cp1y, (float)cp2x, (float)cp2y, (float)x, (float)y);
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            _path.ArcTo((float)x1, (float)y1, (float)x2, (float)y2, (float)radius);
        }

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool anticlockwise = false)
        {
            var startDegrees = (float)(startAngle * 180 / Math.PI);
            var endDegrees = (float)(endAngle * 180 / Math.PI);
            var sweepAngle = endDegrees - startDegrees;

            if (anticlockwise && sweepAngle > 0)
            {
                sweepAngle -= 360;
            }
            else if (!anticlockwise && sweepAngle < 0)
            {
                sweepAngle += 360;
            }

            var rect = new SKRect((float)(x - r), (float)(y - r), (float)(x + r), (float)(y + r));

            // Calculate the start point of the arc
            var startX = (float)(x + r * Math.Cos(startAngle));
            var startY = (float)(y + r * Math.Sin(startAngle));

            if (_path.IsEmpty)
            {
                _path.MoveTo(startX, startY);
            }
            else
            {
                _path.LineTo(startX, startY);
            }

            _path.AddArc(rect, startDegrees, sweepAngle);
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.AddRect(new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h)));
        }

        public void ellipse(double x, double y, double radiusX, double radiusY, double rotation,
                           double startAngle, double endAngle, bool anticlockwise = false)
        {
            if (radiusX < 0 || radiusY < 0)
            {
                throw new NotSupportedException("Radius values for ellipse must be non-negative.");
            }

            var startDegrees = (float)(startAngle * 180 / Math.PI);
            var endDegrees = (float)(endAngle * 180 / Math.PI);
            var sweepAngle = endDegrees - startDegrees;

            if (anticlockwise && sweepAngle > 0)
            {
                sweepAngle -= 360;
            }
            else if (!anticlockwise && sweepAngle < 0)
            {
                sweepAngle += 360;
            }

            var rect = new SKRect((float)(x - radiusX), (float)(y - radiusY),
                                  (float)(x + radiusX), (float)(y + radiusY));

            using (var matrix = SKMatrix.CreateRotationDegrees((float)(rotation * 180 / Math.PI), (float)x, (float)y))
            {
                var transformed = new SKPath();
                transformed.AddArc(rect, startDegrees, sweepAngle);
                transformed.Transform(matrix);
                _path.AddPath(transformed);
            }
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            var rect = new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h));

            if (radii == null)
            {
                _path.AddRect(rect);
                return;
            }

            // Handle different radii formats
            if (radii is double radius)
            {
                _path.AddRoundRect(rect, (float)radius, (float)radius);
            }
            else if (radii is double[] radiiArray)
            {
                if (radiiArray.Length == 1)
                {
                    _path.AddRoundRect(rect, (float)radiiArray[0], (float)radiiArray[0]);
                }
                else if (radiiArray.Length >= 4)
                {
                    // Top-left, top-right, bottom-right, bottom-left
                    var radiiValues = new float[8];
                    for (int i = 0; i < 4 && i < radiiArray.Length; i++)
                    {
                        radiiValues[i * 2] = (float)radiiArray[i];
                        radiiValues[i * 2 + 1] = (float)radiiArray[i];
                    }
                    _path.AddRoundRect(rect, radiiValues[0], radiiValues[1]);
                }
            }
            else
            {
                _path.AddRect(rect);
            }
        }

        public void closePath()
        {
            _path.Close();
        }

        public void addPath(object path, object transform = null)
        {
            if (path is Path2D path2D)
            {
                if (transform == null)
                {
                    _path.AddPath(path2D._path);
                }
                else if (transform is DOMMatrix matrix)
                {
                    var skMatrix = new SKMatrix
                    {
                        ScaleX = (float)matrix.a,
                        SkewY = (float)matrix.b,
                        SkewX = (float)matrix.c,
                        ScaleY = (float)matrix.d,
                        TransX = (float)matrix.e,
                        TransY = (float)matrix.f,
                        Persp2 = 1
                    };
                    _path.AddPath(path2D._path, skMatrix);
                }
            }
        }

        /// <summary>
        /// Gets the underlying SKPath for use with drawing operations.
        /// </summary>
        internal SKPath GetSKPath()
        {
            return _path;
        }
    }
}
