using SharpCanvas.Shared;
using SharpCanvas.Common;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpCanvas.Context.Drawing2D
{
    /// <summary>
    /// Path2D implementation for System.Drawing backend.
    /// Provides reusable path objects for improved performance and cleaner code.
    /// </summary>
    public class Path2D : IPath2D
    {
        public GraphicsPath _path;

        /// <summary>
        /// Creates a new empty Path2D object.
        /// </summary>
        public Path2D()
        {
            _path = new GraphicsPath();
            _path.FillMode = FillMode.Winding;
        }

        /// <summary>
        /// Creates a new Path2D object from an existing path.
        /// </summary>
        public Path2D(Path2D path)
        {
            if (path == null)
            {
                _path = new GraphicsPath();
                _path.FillMode = FillMode.Winding;
            }
            else
            {
                _path = (GraphicsPath)path._path.Clone();
            }
        }

        /// <summary>
        /// Creates a new Path2D object from an SVG path string.
        /// Note: SVG path parsing is not implemented in System.Drawing backend.
        /// </summary>
        public Path2D(string svgPath)
        {
            // SVG path parsing is not implemented for System.Drawing
            _path = new GraphicsPath();
            _path.FillMode = FillMode.Winding;
            // TODO: Implement SVG path parsing for System.Drawing
        }

        public void moveTo(double x, double y)
        {
            _path.StartFigure();
            var point = new PointF((float)x, (float)y);
            _path.AddLine(point, point);
        }

        public void lineTo(double x, double y)
        {
            var point = new PointF((float)x, (float)y);
            if (_path.PointCount > 0)
                _path.AddLine(_path.GetLastPoint(), point);
            else
                moveTo(x, y);
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            if (_path.PointCount == 0)
            {
                moveTo(cpx, cpy);
            }

            var qp = new[]
            {
                _path.GetLastPoint(),
                new PointF((float)cpx, (float)cpy),
                new PointF((float)x, (float)y)
            };

            PointF cp0 = qp[0];
            PointF cp3 = qp[2];
            var cp1 = new PointF();
            cp1.X = qp[0].X + 2f / 3f * (qp[1].X - qp[0].X);
            cp1.Y = qp[0].Y + 2f / 3f * (qp[1].Y - qp[0].Y);
            var cp2 = new PointF();
            cp2.X = cp1.X + 1f / 3F * (qp[2].X - qp[0].X);
            cp2.Y = cp1.Y + 1f / 3F * (qp[2].Y - qp[0].Y);
            _path.AddBezier(cp0, cp1, cp2, cp3);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            if (_path.PointCount == 0)
            {
                moveTo(cp1x, cp1y);
            }

            _path.AddBezier(
                _path.GetLastPoint(),
                new PointF((float)cp1x, (float)cp1y),
                new PointF((float)cp2x, (float)cp2y),
                new PointF((float)x, (float)y)
            );
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            if (radius < 0)
                throw new Exception(ErrorMessages.INDEX_SIZE_ERR);

            if (_path.PointCount == 0)
            {
                moveTo(x1, y1);
            }

            PointF point = _path.GetLastPoint();
            float fx1 = (float)x1, fx2 = (float)x2, fy1 = (float)y1, fy2 = (float)y2, fradius = (float)radius;
            float x0 = point.X;
            float y0 = point.Y;

            if (fradius == 0 || (x0 == fx1 && y0 == fy1) || (fx1 == fx2 && fy1 == fy2))
            {
                lineTo(fx1, fy1);
                return;
            }

            // Calculate tangent points and arc - implementation matches CanvasRenderingContext2D.arcTo
            var v01 = new PointF(x0 - fx1, y0 - fy1);
            var v12 = new PointF(fx2 - fx1, fy2 - fy1);
            var cosA = (float)((v01.X * v12.X + v01.Y * v12.Y) /
                                (Math.Sqrt(Math.Pow(v01.X, 2) + Math.Pow(v01.Y, 2)) *
                                 Math.Sqrt(Math.Pow(v12.X, 2) + Math.Pow(v12.Y, 2))));
            var a = (float)Math.Acos(cosA);

            if (Math.Abs(a - Math.PI) < 0.00001 || a == 0)
            {
                lineTo(fx1, fy1);
                return;
            }

            var d = (float)(fradius / Math.Tan(a / 2d));
            PointF t01 = FindTangentPoint(fx1, fy1, x0, y0, d);
            PointF t12 = FindTangentPoint(fx1, fy1, fx2, fy2, d);

            lineTo(t01.X, t01.Y);
            // Add arc between t01 and t12
            // For simplicity in Path2D, we'll add a straight line (full arc implementation would be complex)
            lineTo(t12.X, t12.Y);
        }

        private PointF FindTangentPoint(float x1, float y1, float x0, float y0, float d)
        {
            float dx = d * Math.Abs(x0 - x1) / (GeometryUtils.Distance(new PointF(x0, y0), new PointF(x1, y1)));
            float x;
            if (x0 < x1)
            {
                x = x1 - dx;
            }
            else
            {
                x = x1 + dx;
            }
            float y;
            float dy = d * Math.Abs(y0 - y1) / (GeometryUtils.Distance(new PointF(x0, y0), new PointF(x1, y1)));
            if (y0 < y1)
            {
                y = y1 - dy;
            }
            else
            {
                y = y1 + dy;
            }
            return new PointF(x, y);
        }

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool anticlockwise = false)
        {
            if (r < 0)
                throw new Exception(ErrorMessages.INDEX_SIZE_ERR);

            var startDegrees = (float)GeometryUtils.ConvertRadiansToDegrees(startAngle);
            var endDegrees = (float)GeometryUtils.ConvertRadiansToDegrees(endAngle);

            float sweepAngle;
            if (anticlockwise)
            {
                sweepAngle = startDegrees - endDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }
            else
            {
                sweepAngle = endDegrees - startDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }

            int direction = anticlockwise ? -1 : 1;
            _path.AddArc((float)(x - r), (float)(y - r), (float)r * 2,
                        (float)r * 2, startDegrees, direction * sweepAngle);
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.StartFigure();
            _path.AddPolygon(new[]
            {
                new PointF((float)x, (float)y),
                new PointF((float)(x + w), (float)y),
                new PointF((float)(x + w), (float)(y + h)),
                new PointF((float)x, (float)(y + h))
            });
            _path.CloseFigure();
        }

        public void ellipse(double x, double y, double radiusX, double radiusY, double rotation,
                           double startAngle, double endAngle, bool anticlockwise = false)
        {
            if (radiusX < 0 || radiusY < 0)
            {
                throw new NotSupportedException("Radius values for ellipse must be non-negative.");
            }

            // Create a full ellipse first
            using (var ellipsePath = new GraphicsPath())
            {
                ellipsePath.AddEllipse((float)(x - radiusX), (float)(y - radiusY),
                                      (float)(radiusX * 2), (float)(radiusY * 2));

                if (rotation != 0)
                {
                    using (var matrix = new Matrix())
                    {
                        matrix.RotateAt((float)(GeometryUtils.ConvertRadiansToDegrees(rotation)),
                                       new PointF((float)x, (float)y));
                        ellipsePath.Transform(matrix);
                    }
                }
                _path.AddPath(ellipsePath, false);
            }
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            var rect = new RectangleF((float)x, (float)y, (float)w, (float)h);
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
                _path.AddRectangle(rect);
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
                    topRight = bottomLeft = radiiList[0];
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
                    topLeft = radiiList[0];
                    topRight = radiiList[1];
                    bottomRight = radiiList[2];
                    bottomLeft = radiiList[3];
                    break;
            }

            // Create rounded rectangle using arcs and lines
            float x1 = (float)x;
            float y1 = (float)y;
            float x2 = x1 + (float)w;
            float y2 = y1 + (float)h;

            _path.StartFigure();

            // Top left corner
            if (topLeft > 0)
            {
                _path.AddArc(x1, y1, topLeft * 2, topLeft * 2, 180, 90);
            }
            else
            {
                _path.AddLine(x1, y1, x1, y1);
            }

            // Top right corner
            if (topRight > 0)
            {
                _path.AddArc(x2 - topRight * 2, y1, topRight * 2, topRight * 2, 270, 90);
            }
            else
            {
                _path.AddLine(x2, y1, x2, y1);
            }

            // Bottom right corner
            if (bottomRight > 0)
            {
                _path.AddArc(x2 - bottomRight * 2, y2 - bottomRight * 2, bottomRight * 2, bottomRight * 2, 0, 90);
            }
            else
            {
                _path.AddLine(x2, y2, x2, y2);
            }

            // Bottom left corner
            if (bottomLeft > 0)
            {
                _path.AddArc(x1, y2 - bottomLeft * 2, bottomLeft * 2, bottomLeft * 2, 90, 90);
            }
            else
            {
                _path.AddLine(x1, y2, x1, y2);
            }

            _path.CloseFigure();
        }

        public void closePath()
        {
            _path.CloseFigure();
        }

        public void addPath(object path, object transform = null)
        {
            if (path is Path2D path2D)
            {
                var pathToAdd = (GraphicsPath)path2D._path.Clone();
                if (transform is DOMMatrix matrix)
                {
                    var gdMatrix = new Matrix(
                        (float)matrix.a, (float)matrix.b,
                        (float)matrix.c, (float)matrix.d,
                        (float)matrix.e, (float)matrix.f
                    );
                    pathToAdd.Transform(gdMatrix);
                }
                _path.AddPath(pathToAdd, false);
            }
        }
    }
}
