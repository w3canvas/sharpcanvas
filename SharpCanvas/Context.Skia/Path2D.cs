using SharpCanvas.Shared;
using SkiaSharp;
using System;
using System.Linq;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Path2D implementation for SkiaSharp backend.
    /// Provides reusable path objects for improved performance and cleaner code.
    /// </summary>
    public class Path2D : IPath2D
    {
        public SKPath _path;

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

            // For anticlockwise arcs, SkiaSharp uses positive sweep angles to go counterclockwise
            // For clockwise arcs, SkiaSharp uses positive sweep angles to go clockwise
            // The key insight: we need to reverse the sweep calculation for anticlockwise
            float sweepAngle;
            if (anticlockwise)
            {
                // Anticlockwise: calculate from start to end going backwards (positive sweep in SkiaSharp)
                sweepAngle = startDegrees - endDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }
            else
            {
                // Clockwise: calculate from start to end going forwards (positive sweep in SkiaSharp)
                sweepAngle = endDegrees - startDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }

            var rect = new SKRect((float)(x - r), (float)(y - r), (float)(x + r), (float)(y + r));

            // According to the HTML5 Canvas spec, if the path is empty, we need to
            // implicitly moveTo the start point of the arc. If not empty, we should
            // lineTo from the current point to the start of the arc.
            //
            // Calculate the start point of the arc
            var startX = (float)(x + r * System.Math.Cos(startAngle));
            var startY = (float)(y + r * System.Math.Sin(startAngle));

            if (_path.IsEmpty)
            {
                // Path is empty - moveTo the start point
                _path.MoveTo(startX, startY);
            }
            else
            {
                // Path is not empty - lineTo the start point
                _path.LineTo(startX, startY);
            }
            _path.AddArc(rect, startDegrees, sweepAngle);
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.MoveTo((float)x, (float)y);
            _path.LineTo((float)x + (float)w, (float)y);
            _path.LineTo((float)x + (float)w, (float)y + (float)h);
            _path.LineTo((float)x, (float)y + (float)h);
            _path.Close();
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

            var matrix = SKMatrix.CreateRotationDegrees((float)(rotation * 180 / Math.PI), (float)x, (float)y);
            var ellipsePath = new SKPath();
            ellipsePath.AddArc(rect, startDegrees, sweepAngle);
            ellipsePath.Transform(matrix);
            _path.AddPath(ellipsePath);
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            var rect = new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h));
            var radiiList = new System.Collections.Generic.List<float>();

            if (radii is System.Collections.IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    radiiList.Add(System.Convert.ToSingle(item));
                }
            }
            else if (radii is double || radii is int || radii is float)
            {
                radiiList.Add(System.Convert.ToSingle(radii));
            }

            if (radiiList.Count == 0)
            {
                _path.AddRect(rect);
                return;
            }

            float topLeft, topRight, bottomRight, bottomLeft;
            switch (radiiList.Count)
            {
                case 1:
                    topLeft = topRight = bottomRight = bottomLeft = radiiList[0];
                    break;
                case 2:
                    topLeft = bottomRight = radiiList[0];
                    topRight = bottomLeft = radiiList[1];
                    break;
                case 3:
                    topLeft = radiiList[0];
                    topRight = bottomLeft = radiiList[1];
                    bottomRight = radiiList[2];
                    break;
                case 4:
                    topLeft = radiiList[0];
                    topRight = radiiList[1];
                    bottomRight = radiiList[2];
                    bottomLeft = radiiList[3];
                    break;
                default:
                    // Spec says to use the first 4 values if more are provided.
                    topLeft = radiiList[0];
                    topRight = radiiList[1];
                    bottomRight = radiiList[2];
                    bottomLeft = radiiList[3];
                    break;
            }
            var roundRect = new SKRoundRect(rect);
            roundRect.SetRectRadii(rect, new[]
            {
                new SKPoint(topLeft, topLeft),
                new SKPoint(topRight, topRight),
                new SKPoint(bottomRight, bottomRight),
                new SKPoint(bottomLeft, bottomLeft),
            });
            _path.AddRoundRect(roundRect);
        }

        public void closePath()
        {
            _path.Close();
        }

        public void addPath(object path, object transform = null)
        {
            if (path is Path2D path2D)
            {
                var path_to_add = new SKPath(path2D._path);
                if (transform is DOMMatrix matrix)
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
                    path_to_add.Transform(skMatrix);
                }
                _path.AddPath(path_to_add, SKPathAddMode.Extend);
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
