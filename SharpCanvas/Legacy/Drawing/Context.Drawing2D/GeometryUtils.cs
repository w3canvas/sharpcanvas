using System;
using System.Drawing;

namespace SharpCanvas.Context.Drawing2D
{
    public static class GeometryUtils
    {
        /// <summary>
        /// Determine does sector and line intersects
        /// </summary>
        /// <param name="p1">sector's first point</param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        public static bool SectorAndLineIntersects(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float denominator = (p4.Y - p3.Y)*(p2.X - p1.X) - (p4.X - p3.X)*(p2.Y - p1.Y);
            if (denominator == 0)
                return false; //lines are parrallel
            float Ua = ((p4.X - p3.X)*(p1.Y - p3.Y) - (p4.Y - p3.Y)*(p1.X - p3.X))/denominator;
            float Ub = ((p2.X - p1.X)*(p1.Y - p3.Y) - (p2.Y - p1.Y)*(p1.X - p3.X))/denominator;
            if (Ua >= 0 && Ua <= 1)
                return true; //intersection point is on the sector (p1,p2)
            return false;
        }

        /// <summary>
        /// Get intersection point of two lines
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        public static PointF GetIntersectionPoint(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float denominator = (p4.Y - p3.Y)*(p2.X - p1.X) - (p4.X - p3.X)*(p2.Y - p1.Y);
            float Ua = ((p4.X - p3.X)*(p1.Y - p3.Y) - (p4.Y - p3.Y)*(p1.X - p3.X))/denominator;
            float Ub = ((p2.X - p1.X)*(p1.Y - p3.Y) - (p2.Y - p1.Y)*(p1.X - p3.X))/denominator;
            float x = p1.X + Ua*(p2.X - p1.X);
            float y = p1.Y + Ua*(p2.Y - p1.Y);
            return new PointF(x, y);
        }

        public static float Distance(PointF a, PointF b)
        {
            return (float) Math.Sqrt((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y));
        }

        public static LineEquation BuildLineEqualtionByTwoPoints(PointF A, PointF B)
        {
            //y = k * x + b
            float determinator = (A.X - B.X);
            if (determinator == 0)
                return new LineEquation(true, false, B.X);
            float k = (A.Y - B.Y)/determinator;
            if (k == 0)
                return new LineEquation(false, true, B.Y);
            float b = (-1*B.X*(A.Y - B.Y) + determinator*B.Y)/determinator;
            return new LineEquation(k, b);
        }

        public static LineEquation BuildPerpendicularEqualtionByTwoPoints(PointF A, PointF B)
        {
            //we should build equation of segment first by using two given points
            //y = k * x + b
            float determinator = (A.X - B.X);
            if (determinator == 0)
                return new LineEquation(false, true, B.Y);
            float k = (A.Y - B.Y)/determinator;
            float b = (-1*B.X*(A.Y - B.Y) + determinator*B.Y)/determinator;
            //now we have to define equation of the perpendicular which intersect segment in point B
            //y = (-1 / k) * x + d
            if (k == 0)
                //
                return new LineEquation(true, false, B.X);
            float a = -1/k;
            ;
            //B.Y = a * B.X + d
            float d = B.Y - a*B.X;
            //so here is the new equation
            //y = a * x  + d
            return new LineEquation(a, d);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="A">First point of the line</param>
        /// <param name="B">Second point of the line</param>
        /// <param name="P">Point on the perpendicular to find</param>
        /// <returns></returns>
        public static LineEquation BuildPerpendicularEqualtionByLineAndPoint(PointF A, PointF B, PointF P)
        {
            //we should build equation of segment first by using two given points
            //y = k * x + b
            float determinator = (A.X - B.X);
            if (determinator == 0)
                return new LineEquation(false, true, P.Y);
            float k = (A.Y - B.Y)/determinator;
            float b = (-1*B.X*(A.Y - B.Y) + determinator*B.Y)/determinator;
            //now we have to define equation of the perpendicular which intersect segment in point B
            //y = (-1 / k) * x + d
            if (k == 0)
                return new LineEquation(true, false, P.X);
            float a = -1/k;
            ;
            //B.Y = a * B.X + d
            float d = P.Y - a*P.X;
            //so here is the new equation
            //y = a * x  + d
            return new LineEquation(a, d);
        }

        public static bool LineAndLineIntersects(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float denominator = (p4.Y - p3.Y)*(p2.X - p1.X) - (p4.X - p3.X)*(p2.Y - p1.Y);
            if (denominator == 0)
                return false; //lines are parrallel
            return true;
        }

        public static float ConvertRadiansToDegrees(double radians)
        {
            return (float) (radians * 180 / Math.PI);
        }
    }
}
