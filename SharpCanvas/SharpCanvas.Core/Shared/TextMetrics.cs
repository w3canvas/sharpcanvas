namespace SharpCanvas
{
    public struct TextMetrics
    {
        public TextMetrics(int width, int height) : this()
        {
            this.width = width;
            this.height = height;
        }

        public double width { get; set; }
        public double height { get; set; } // Non-standard

        public double actualBoundingBoxLeft { get; set; }
        public double actualBoundingBoxRight { get; set; }
        public double actualBoundingBoxAscent { get; set; }
        public double actualBoundingBoxDescent { get; set; }
        public double fontBoundingBoxAscent { get; set; }
        public double fontBoundingBoxDescent { get; set; }
        public double emHeightAscent { get; set; }
        public double emHeightDescent { get; set; }
        public double hangingBaseline { get; set; }
        public double alphabeticBaseline { get; set; }
        public double ideographicBaseline { get; set; }
    }
}
