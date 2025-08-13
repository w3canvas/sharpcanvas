using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SharpCanvas.Common;
//using System.Linq;

namespace SharpCanvas.Media
{
    public class CanvasPath
    {
        private static int _numberOfCommits;
        private static int _numberOfPaths;

        #region Fields

        private readonly Canvas _surface;
        private PathFigure _figure;
        private PathGeometry _geometry;
        private bool _isEmpty;
        private CanvasStyle _latestStyle;
        private Path _path;
        private MatrixTransform _transformation;
        private GeometryGroup _cachedData;
        private Brush _cachedFillBrush;
        private Brush _cachedStrokeBrush;

        #endregion

        #region Constructors

        public CanvasPath(Canvas surface)
        {
            _surface = surface;
            _transformation = new MatrixTransform();
            Init();
        }

        #endregion

        #region Path Core

        private GeometryGroup _group = new GeometryGroup();

        public void CommitAndApplyStyle(CanvasStyle style)
        {
            bool isDifferentStyle = !AreStylesEquals(_path, style);
            if (!_isEmpty)
            {
                if (isDifferentStyle)
                {
//style is changed, so we have to have new path
                    Commit();
                }
                else
                {
//style the same, so we can keep current path and continue draw in it
                    CommitToGeometryAndInit();
                }
            }
            ApplyStyle(style);
        }

        private bool AreStylesEquals(Path _path, CanvasStyle style)
        {
            return _path.Fill == style.Fill &&
                   _path.Stroke == style.Stroke &&
                   _path.StrokeThickness == style.StrokeWidth &&
                   _path.StrokeLineJoin == style.StrokeLineJoin &&
                   _path.StrokeEndLineCap == style.LineCap &&
                   _path.StrokeStartLineCap == style.LineCap &&
                   _path.StrokeMiterLimit == style.StrokeMiterLimit &&
                   _path.Opacity == style.GlobalAlpha;
        }

        public void ApplyStyle(CanvasStyle style)
        {
            _path.Fill = style.Fill;
            _path.Stroke = style.Stroke;
            _path.StrokeThickness = style.StrokeWidth;
            _path.StrokeLineJoin = style.StrokeLineJoin;
            _path.StrokeEndLineCap = style.LineCap;
            _path.StrokeStartLineCap = style.LineCap;
            _path.StrokeMiterLimit = style.StrokeMiterLimit;
            _path.Opacity = style.GlobalAlpha;
            _latestStyle = style;
        }

        public CanvasStyle GetStyle()
        {
            //use super constructor in order to avoid triggers CommitAndApplyStyle after each property get-call
            var style = new CanvasStyle(this, _path.Fill.Clone(), _path.Stroke.Clone(), _path.StrokeThickness,
                                        _path.StrokeLineJoin, _path.StrokeEndLineCap,
                                        (float) _path.StrokeMiterLimit, _path.Opacity);
            return style;
        }

        /// <summary>
        /// Commits current geometry to the geometry group and init Geometry as well as Figure instances
        /// </summary>
        private void CommitToGeometryAndInit()
        {
            CommitToGeometry();
            _group.Children.Add(_geometry);
            InitGeometry();
            InitSubPath();
        }

        /// <summary>
        /// Add all the graphics to the drawing surface and initialize next "Path's tree"
        /// </summary>
        public void Commit()
        {
            if (!_isEmpty)
            {
                _surface.Children.Add(_path);
                CommitToGeometry();
                CommitToPath();
                Init();
                _numberOfCommits++;
            }
        }

        /// <summary>
        /// Commit current set of geometries to the path
        /// </summary>
        private void CommitToPath()
        {
            //we shouldn't freeze geometry here because it can be re-used later
            _path.Data = _group;
            if (!this.IsEmpty(_group))
            {
                //cache data for further reuse in stroke or fill commands
                _cachedData = _group.Clone();
                //_cachedPath = _path;
            }
            else if(!this.IsEmpty(_cachedData))//if no graphics were added between previous and current commit, then it means we want to reuse the same(cached for now) graphics for stroke or fill
            {
                _path.Data = _cachedData;
                if(_cachedFillBrush != null)
                {
                    _path.Fill = _cachedFillBrush;
                }
                else if(_cachedStrokeBrush != null)
                {
                    _path.Stroke = _cachedStrokeBrush;
                }
            }
        }

        /// <summary>
        /// Commits current figure to the current geometry object
        /// Should be called before each moveTo
        /// </summary>
        private void CommitToGeometry()
        {
            //we shouldn't freeze figure here because it can be re-used later
            if (!_geometry.Figures.Contains(_figure))
                _geometry.Figures.Add(_figure);
        }

        private void Init()
        {
            if (!_isEmpty)
            {
                //contains all graphics data which should be drawn by some particular style
                //once style changed, new path (surface) should be created.
                _path = new Path();
                //contains a set of geometries
                _group = new GeometryGroup();
                //contains a set of geometry figures
                InitGeometry();
                //contains a set of elementary geometry elements
                //new PathFigure should be created after each moveTo
                InitSubPath();
                if (_latestStyle != null)
                    ApplyStyle(_latestStyle);
                //once all the control's tree initialized, mark it as empty control set, because it doesn't contain any image
                _isEmpty = true;
                _numberOfPaths++;
            }
        }

        //todo:remove this method. Was created for profiling purposes
        public int GetNumberOfPaths()
        {
            int n = _numberOfPaths;
            _numberOfPaths = 0;
            return n;
        }

        //todo:remove this method. Was created for profiling purposes
        public int GetNumberOfCommits()
        {
            int n = _numberOfCommits;
            _numberOfCommits = 0;
            return n;
        }

        /// <summary>
        /// Geometry is the collection of subpaths, some kind of intermediate layer between Path and PathFigure
        /// InitGeometry should be called when new Path created.
        /// </summary>
        private void InitGeometry()
        {
            _geometry = new PathGeometry();
        }

        /// <summary>
        /// PathFigures represents subpaths in terms of HTML 5 spec
        /// InitSubPath should be called after each moveTo call
        /// </summary>
        private void InitSubPath()
        {
            _figure = new PathFigure();
        }

        #endregion

        /// <summary>
        /// The moveTo(x, y) method must create a new subpath with the specified point as its first (and only) point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveToWithoutTransformation(double x, double y)
        {
            //some start point exists but there is no segments inside the figure
            if (_figure.Segments.Count == 0)
            {
                _figure.StartPoint = new Point(x + _figure.StartPoint.X, y + _figure.StartPoint.Y);
            }
            else
            {
                //means that StartPoint is null, so we initialize it
                if (_figure.Segments.Count == 0)
                {
                    _figure.StartPoint = new Point(x, y);
                }
                else //means that there are some segments in the figure
                {
                    _isEmpty = false;
                    CommitToGeometry();
                    InitSubPath();
                    _figure.StartPoint = new Point(x, y);
                }
            }
            _isEmpty = false;
        }

        public void MoveTo(double x, double y)
        {
            Point transformedPoint = _transformation.Transform(new Point(x, y));
            //some start point exists but there is no segments inside the figure
            if (_figure.Segments.Count == 0)
            {
                _figure.StartPoint = new Point(transformedPoint.X + _figure.StartPoint.X,
                                               transformedPoint.Y + _figure.StartPoint.Y);
            }
            else
            {
                //means that StartPoint is null, so we initialize it
                if (_figure.Segments.Count == 0)
                {
                    _figure.StartPoint = transformedPoint;
                }
                else //means that there are some segments in the figure
                {
                    _isEmpty = false;
                    CommitToGeometry();
                    InitSubPath();
                    _figure.StartPoint = transformedPoint;
                }
            }
            _isEmpty = false;
        }

        public bool IsPointInPath(double x, double y)
        {
            return _geometry.FillContains(new Point(x, y));
        }

        public Geometry GetClip()
        {
            return _geometry;
        }

        #region lineJoin workaround

        private void RemoveFigureLastPoint(PathFigure figure, int stepBack)
        {
            if (figure.Segments.Count == 0)
            {
                figure.StartPoint = new Point(0, 0);
                return;
            }
            int count = figure.Segments.Count - 1;
            PathSegment lastSegment = count >= stepBack ? figure.Segments[count - stepBack] : null;
            if (lastSegment != null && lastSegment is LineSegment)
                figure.Segments.Remove(lastSegment);
        }

        private bool IsCutRequired(Point? a, Point? b, Point c)
        {
            if (a == null || b == null)
                return false;
            var v1 = new Vector(a.Value.X - b.Value.X, a.Value.Y - b.Value.Y);
            var v2 = new Vector(c.X - b.Value.X, c.Y - b.Value.Y);
            double angle = Math.Abs(Vector.AngleBetween(v1, v2));
            if (angle > 180)
                angle = 360 - angle;
            double d = _path.StrokeThickness/Math.Sin(angle*Math.PI/180);
            double heightLimit = _path.StrokeMiterLimit*(_path.StrokeThickness/2);
            return !double.IsInfinity(d) && d != 0 && d > heightLimit;
        }

        private bool IsEmpty(GeometryGroup group)
        {
            var isEmpty = true;
            foreach (Geometry geometry in group.Children)
            {
                if (!geometry.IsEmpty() && geometry.ToString() != "M0,0z" && geometry.ToString() != "M0,0")
                {
                    isEmpty = false;
                }
            }
            return isEmpty;
        }

        private Point? GetFigureLastPoint(PathFigure figure, int stepBack)
        {
            if (figure.Segments.Count == 0)
                return figure.StartPoint;
            int count = figure.Segments.Count - 1;
            PathSegment lastSegment = count >= stepBack ? figure.Segments[count - stepBack] : null;
            if (lastSegment != null && lastSegment is LineSegment)
                return ((LineSegment) lastSegment).Point;
            return null;
        }

        #endregion

        #region stroke, fill, Figures

        public void Stroke(bool isClosed)
        {
            _cachedFillBrush = _path.Fill;
            _cachedStrokeBrush = null;
            _path.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            _figure.IsClosed = isClosed;
            _figure.IsFilled = false;
            _isEmpty = false;
            CommitToGeometryAndInit();
        }

        

        public void Fill()
        {
            _cachedStrokeBrush = _path.Stroke;
            _cachedFillBrush = null;
            _path.Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            _figure.IsClosed = true;
            _figure.IsFilled = true;
            _isEmpty = false;
            CommitToGeometryAndInit();
        }

        /// <summary>
        /// The lineTo(x, y) method must ensure there is a subpath for (x, y) if the context has no subpaths. 
        /// Otherwise, it must connect the last point in the subpath to the given point (x, y) using a straight line, and must then 
        /// add the given point (x, y) to the subpath.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void LineTo(double x, double y)
        {
            //transform initial coordinates according to the latest transformation
            Point transformedPoint = _transformation.Transform(new Point(x, y));
            //The LineSegment class does not contain a property for the starting point of the line. 
            //The starting point of the line is the end point of the previous segment, or the StartPoint of the PathFigure 
            //if no other segments exist.

            //IMPORTANT: next comment related to lineJoin workaround
            //Point? a = GetFigureLastPoint(_figure, 1);
            //Point? b = GetFigureLastPoint(_figure, 0);
            //if (_path.StrokeLineJoin == PenLineJoin.Miter && IsCutRequired(a, b, transformedPoint))
            //{
            //    RemoveFigureLastPoint(_figure, 0);
            //    RemoveFigureLastPoint(_figure, 0);
            //    CommitToSurface();
            //    Init();
            //    _figure.StartPoint = (Point)a;
            //    _figure.Segments.Add(new LineSegment((Point)b, true));
            //    _figure.Segments.Add(new LineSegment(transformedPoint, true));
            //    _path.StrokeLineJoin = PenLineJoin.Bevel;
            //    CommitToSurface();
            //    Init();
            //    _figure.StartPoint = transformedPoint;
            //}
            //else
            //{
            var segment = new LineSegment(transformedPoint, true);
            _figure.Segments.Add(segment);
            _isEmpty = false;
            //}
        }

        public void BezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            //transform initial coordinates according to the latest transformation
            Point[] points = TransformPoints(new Point(cp1x, cp1y), new Point(cp2x, cp2y), new Point(x, y));
            var bezierSegment = new BezierSegment(points[0], points[1], points[2], true);
            _figure.Segments.Add(bezierSegment);
            _isEmpty = false;
        }

        public void QuadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            //transform initial coordinates according to the latest transformation
            Point[] points = TransformPoints(new Point(cpx, cpy), new Point(x, y));
            var segment = new QuadraticBezierSegment {Point1 = points[0], Point2 = points[1]};
            _figure.Segments.Add(segment);
            _isEmpty = false;
        }

        /// <summary>
        /// The beginPath()  method must empty the list of subpaths so that the context once again has zero subpaths.
        /// </summary>
        public void BeginPath()
        {
            Commit();
            _geometry.Figures.Clear();
            _isEmpty = true;
        }

        /// <summary>
        /// The closePath()  method must do nothing if the context has no subpaths. Otherwise, it must mark the last subpath 
        /// as closed, create a new subpath whose first point is the same as the previous subpath's first point, and finally 
        /// add this new subpath to the path.
        /// If the last subpath had more than one point in its list of points, then this is equivalent to adding a 
        /// straight line connecting the last point back to the first point, thus "closing" the shape, and then repeating the 
        /// last (possibly implied) moveTo() call.
        /// </summary>
        public void ClosePath()
        {
            CommitToGeometry();
            if (_geometry.IsEmpty() || _geometry.Figures.Count == 0 || _isEmpty)
                return;
            _geometry.Figures[_geometry.Figures.Count - 1].IsClosed = true;
            InitSubPath();
            //TODO: get last point of the last subpath and set it as StartPoint for new subpath
        }

        public void FillRect(double x, double y, double w, double h)
        {
            Point[] points = TransformPoints(new Point(x, y), new Point(x + w, y), new Point(x + w, y + h),
                                             new Point(x, y + h));
            MoveToWithoutTransformation(points[0].X, points[0].Y);
            _figure.Segments.Add(new PolyLineSegment(new[] {points[0], points[1], points[2], points[3]}, false));
            _isEmpty = false;
            Fill();
        }

        public void StrokeRect(double x, double y, double w, double h)
        {
            Point[] points = TransformPoints(new Point(x, y), new Point(x + w, y), new Point(x + w, y + h),
                                             new Point(x, y + h));
            MoveToWithoutTransformation(points[0].X, points[0].Y);
            _figure.Segments.Add(new PolyLineSegment(new[] {points[0], points[1], points[2], points[3]}, true));
            _isEmpty = false;
            Stroke(true);
        }

        public void Rect(double x, double y, double w, double h)
        {
            Point[] points = TransformPoints(new Point(x, y), new Point(x + w, y), new Point(x + w, y + h),
                                             new Point(x, y + h));
            MoveToWithoutTransformation(points[0].X, points[0].Y);
            _figure.Segments.Add(new PolyLineSegment(new[] {points[0], points[1], points[2], points[3]}, true));
            _isEmpty = false;
        }

        public void Arc(double x, double y, double r, double startAngle, double endAngle, bool clockwise)
        {
            //transform initial coordinates according to the latest transformation
            Point tp = _transformation.Transform(new Point(x, y));
            ArcInternal(x, y, tp, r, startAngle, endAngle, clockwise);
            _isEmpty = false;
        }

        private void ArcInternal(double x, double y, Point tp, double r, double startAngle, double endAngle,
                                 bool clockwise)
        {
            //calculate start point's coordinates
            var start = new Point(tp.X + (r*Math.Cos(startAngle)), tp.Y + (r*Math.Sin(startAngle)));
            //if current geometry has no subpaths, than we have to add on more line
            //and we can't use Figure.StartPoint
            if (_geometry.Figures.Count > 0 || _figure.Segments.Count > 0)
            {
                // Draw line from last point of path to start of arc
                _figure.Segments.Add(new LineSegment {Point = start});
            }
            else
            {
                // Start at start point
                _figure.StartPoint = start;
            }
            //Workaround for full circles
            if (2*Math.PI <= Math.Abs(endAngle - startAngle))
            {
                //We can draw complete circles as two half-circles
                ArcInternal(x, y, tp, r, startAngle, Math.PI, clockwise);
                startAngle = Math.PI;
            }
            // Normalize angles
            var end = new Point(tp.X + (r*Math.Cos(endAngle)), tp.Y + (r*Math.Sin(endAngle)));
            if (clockwise)
            {
                if (startAngle < endAngle)
                {
                    startAngle += 2*Math.PI;
                }
            }
            else
            {
                if (endAngle < startAngle)
                {
                    endAngle += 2*Math.PI;
                }
            }
            //Determine by angle if arc is large, i.e. is its angle is bigger then 180 degrees.
            bool isLargeArc = Math.PI < Math.Abs(endAngle - startAngle);
            //Add ArcSegment to the figure
            _figure.Segments.Add(new ArcSegment
                                     {
                                         Point = end,
                                         Size = new Size(r, r),
                                         IsLargeArc = isLargeArc,
                                         SweepDirection =
                                             clockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise
                                     });
        }

        public void ArcTo(double px1, double py1, double px2, double py2, double radius)
        {
            //transform initial coordinates according to the latest transformation
            Point[] points = TransformPoints(new Point(px1, py1), new Point(px2, py2));
            Point? point = GetFigureLastPoint(_figure, 0);
            //transfrom points            
            var x1 = (float) points[0].X;
            var y1 = (float) points[0].Y;
            var x2 = (float) points[1].X;
            var y2 = (float) points[1].Y;
            var x0 = (float) point.Value.X;
            var y0 = (float) point.Value.Y;
            if (radius == 0 || (x0 == x1 && y0 == y1) || (x1 == x2 && y1 == y2))
            {
                MoveTo(x1, y1);
                LineTo(x0, y0);
                return;
            }

            //find angle between two lines (p0, p1) and (p1, p2)
            var v01 = new Point(x0 - x1, y0 - y1);
            var v12 = new Point(x2 - x1, y2 - y1);
            var cosA = (float) ((v01.X*v12.X + v01.Y*v12.Y)/
                                (Math.Sqrt(Math.Pow(v01.X, 2) + Math.Pow(v01.Y, 2))*
                                 Math.Sqrt(Math.Pow(v12.X, 2) + Math.Pow(v12.Y, 2))));
            var a = (float) Math.Acos(cosA);
            if (Math.Abs(a - Math.PI) < 0.00001 || a == 0)
            {
                //all three points are on the straight line
                LineTo(x0, y0);
                return;
            }
            //find distance from point p1(x1, y1) to intersection with circle (arc)
            var d = (float) (radius/Math.Tan(a/2d));
            //tangent point of the line (p0, p1)
            Point t01 = FindTangentPoint(x1, y1, x0, y0, d);
            Point t12 = FindTangentPoint(x1, y1, x2, y2, d);
            LineTo(t01.X, t01.Y);
            DrawArcBetweenTwoPoints(t01, t12, radius, false);
            //from point (x0, y0) to t01            
            MoveTo(t12.X, t12.Y);
            _isEmpty = false;
        }

        private Point FindTangentPoint(float x1, float y1, float x0, float y0, float d)
        {
            Point t01;
            //find point on line (p0, p1) on distance d from point p
            float dx = d*Math.Abs(x0 - x1)/(Distance(new Point(x0, y0), new Point(x1, y1)));
            float x;
            if (x0 < x1)
            {
                //means x0 left from x1
                x = x1 - dx;
            }
            else //means x0 right from x1
            {
                x = x1 + dx;
            }
            float y;
            float dy = d*Math.Abs(y0 - y1)/(Distance(new Point(x0, y0), new Point(x1, y1)));
            if (y0 < y1)
            {
                //means y0 uppper y1
                y = y1 - dy;
            }
            else
            {
                //means y0 down y1
                y = y1 + dy;
            }
            t01 = new Point(x, y);
            return t01;
        }

        private float Distance(Point a, Point b)
        {
            return (float) Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private double AdjustAngle(double a, double x, double y, double rX, double rY)
        {
            switch (GetSectorNumber(x, y, rX, rY))
            {
                case 0:
                    return Math.PI*2 - a;
                case 1:
                    return Math.PI + a;
                case 2:
                    return Math.PI - a;
                case 3:
                    return a;
            }
            return a;
        }

        private int GetSectorNumber(double x, double y, double rX, double rY)
        {
            if (x >= rX && y < rY)
                return 0;
            if (x < rX && y <= rY)
                return 1;
            if (x <= rX && y > rY)
                return 2;
            if (x > rX && y >= rY)
                return 3;
            throw new Exception("Invalid coordinates");
        }

        private void DrawArcBetweenTwoPoints(Point a, Point b, double radius, bool clockwise)
        {
            //if current geometry has no subpaths, than we have to add on more line
            //and we can't use Figure.StartPoint
            if (_geometry.Figures.Count == 0 && _figure.Segments.Count == 0)
            {
                // Start at start point
                _figure.StartPoint = a;
            }
            _figure.Segments.Add(new ArcSegment
                                     {
                                         Point = b,
                                         Size = new Size(radius, radius),
                                         IsLargeArc = false,
                                         SweepDirection =
                                             clockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise
                                     });
            _isEmpty = false;
        }

        #endregion

        #region Transformations

        public void Translate(double x, double y)
        {
            var transform = new TranslateTransform(x, y);
            _transformation = new MatrixTransform(Matrix.Multiply(transform.Value, _transformation.Value));
        }

        public void Rotate(double angle)
        {
            var transform = new RotateTransform {Angle = ConvertRadiansToDegrees(angle)};
            _transformation = new MatrixTransform(Matrix.Multiply(transform.Value, _transformation.Value));
        }

        public void Scale(double x, double y)
        {
            var transform = new ScaleTransform {ScaleX = x, ScaleY = y};
            _transformation = new MatrixTransform(Matrix.Multiply(transform.Value, _transformation.Value));
        }

        public void Transform(Matrix matrix)
        {
            Matrix multiply = Matrix.Multiply(matrix, _transformation.Value);
            _transformation = new MatrixTransform(multiply);
        }

        public void SetTransform(Matrix matrix)
        {
            _transformation = new MatrixTransform(matrix);
        }

        public void SetTransform(MatrixTransform transformation)
        {
            _transformation = transformation;
        }

        public MatrixTransform GetTransform()
        {
            return _transformation.Clone();
        }

        #endregion

        #region Text

        public void StrokeText(string text, double x, double y, string font, string textAlign, string textBaseLine)
        {
            _path.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            // the text to be converted to geometry
            DrawText(font, text, x, y, textAlign, textBaseLine, _path.Stroke);
            _isEmpty = false;
            CommitToGeometryAndInit();
        }

        public void FillText(string text, double x, double y, string font, string textAlign, string textBaseLine)
        {
            _path.Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            // the text to be converted to geometry
            DrawText(font, text, x, y, textAlign, textBaseLine, _path.Fill);
            _isEmpty = false;
            CommitToGeometryAndInit();
        }

        private void DrawText(string font, string text, double x, double y, string textAlign, string textBaseLine,
                              Brush brush)
        {
            throw new NotImplementedException();
        }


        public TextMetrics MeasureText(string text, string font)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utils

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        private double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (float) (180/Math.PI)*radians;
            return (degrees);
        }

        private Point[] TransformPoints(params Point[] points)
        {
            var transformedPoints = new Point[points.Length];
            int i = 0;

            foreach (Point point in points)
            {
                transformedPoints[i++] = _transformation.Transform(point);
            }
            return transformedPoints;
        }

        #endregion
    }
}