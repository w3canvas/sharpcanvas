namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Contains all necessary information to process events
    /// </summary>
    public class EventArgument
    {
        public EventArgument()
        {
        }

        public EventArgument(int pageX, int pageY)
        {
            this.pageX = pageX;
            this.pageY = pageY;
        }

        public EventArgument(int pageX, int pageY, int keyCode)
        {
            this.pageX = pageX;
            this.pageY = pageY;
            this.keyCode = keyCode;
        }

        public int pageX { get; set; }
        public int pageY { get; set; }
        public int keyCode { get; set; }
    }
}