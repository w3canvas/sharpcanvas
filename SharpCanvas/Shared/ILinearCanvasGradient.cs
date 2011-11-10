namespace SharpCanvas
{
    public interface ILinearCanvasGradient
    {
        /// <summary>
        /// If multiple stops are added at the same offset on a gradient, they must be placed in the order added, with the first one closest to the start of the
        /// gradient, and each subsequent one infinitesimally further along towards the end point (in effect causing all but the first and last stop added at each point
        /// to be ignored).
        /// ...Between each such stop, the colors and the alpha component must be linearly interpolated over the RGBA space without premultiplying the alpha value to 
        /// find the color to use at that offset. Before the first stop, the color must be the color of the first stop. After the last stop, the color must be the color
        /// of the last stop. When there are no stops, the gradient is transparent black.
        /// </summary>
        /// <param name="offset">Must be between 0..1</param>
        /// <param name="color">Valid Color</param>
        void addColorStop(double offset, string color);

        object GetBrush();
    }
}