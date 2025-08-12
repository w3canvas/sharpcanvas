namespace SharpCanvas.Shared
{
    public class DOMMatrix
    {
        public double a { get; set; } // m11
        public double b { get; set; } // m12
        public double c { get; set; } // m21
        public double d { get; set; } // m22
        public double e { get; set; } // dx
        public double f { get; set; } // dy

        public DOMMatrix(double a = 1, double b = 0, double c = 0, double d = 1, double e = 0, double f = 0)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }
    }
}
