namespace SharpCanvas
{
    public interface IPathCanvasGradient
    {
        void addColorStop(float offset, string color);
        object GetBrush();
    }
}