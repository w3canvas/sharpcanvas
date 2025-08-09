using System;
using System.Drawing;

namespace SharpCanvas.Common
{
    public class LineEquation
    {
        //contains values of parameters for next type of line equation:
        // y = k * x + b
        public float B;
        //we should hadle two cases when the line is perpendicular to OX or parallel to it
        public bool IsXConstant;
        public bool IsYConstant;
        public float K;
        public float Value;

        public LineEquation(float k, float b)
        {
            K = k;
            B = b;
            IsYConstant = false;
            IsXConstant = false;
        }

        public LineEquation(bool isXConstant, bool isYConstant, float v)
        {
            IsXConstant = isXConstant;
            IsYConstant = isYConstant;
            Value = v;
        }

        public PointF getPointWithX(float x)
        {
            if (!IsXConstant)
            {
                if (IsYConstant)
                    return new PointF(x, Value);
                return new PointF(x, K*x + B);
            }
            throw new Exception("X is constant");
        }

        public PointF getPointWithY(float y)
        {
            //x = (y-b) / k
            if (!IsYConstant)
            {
                if (IsXConstant)
                    return new PointF(Value, y);
                return new PointF((y - B)/K, y);
            }
            throw new Exception("Y is constant");
        }
    }
}
