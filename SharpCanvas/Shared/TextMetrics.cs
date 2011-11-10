namespace SharpCanvas
{
    public struct TextMetrics
    {
        public TextMetrics(int width, int height) : this()
        {
            this.width = width;
            this.height = height;
        }

        public int width { get; set; }
        public int height { get; set; }
    }
}