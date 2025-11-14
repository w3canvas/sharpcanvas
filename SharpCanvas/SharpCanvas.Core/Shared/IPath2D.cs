using System.Runtime.InteropServices;

namespace SharpCanvas.Shared
{
    /// <summary>
    /// Path2D interface provides methods for creating and manipulating paths.
    /// Path2D objects can be used with fill(), stroke(), clip() and other path-based operations.
    /// </summary>
    [Guid("9A8C3D2E-1B4F-4C5A-8D9E-2F3A4B5C6D7E"),
     InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IPath2D
    {
        [DispId(1)]
        void moveTo([In] double x, [In] double y);

        [DispId(2)]
        void lineTo([In] double x, [In] double y);

        [DispId(3)]
        void quadraticCurveTo([In] double cpx, [In] double cpy, [In] double x, [In] double y);

        [DispId(4)]
        void bezierCurveTo([In] double cp1x, [In] double cp1y, [In] double cp2x, [In] double cp2y,
                           [In] double x, [In] double y);

        [DispId(5)]
        void arcTo([In] double x1, [In] double y1, [In] double x2, [In] double y2, [In] double radius);

        [DispId(6)]
        void arc([In] double x, [In] double y, [In] double r, [In] double startAngle, [In] double endAngle,
                 [In] bool anticlockwise = false);

        [DispId(7)]
        void rect([In] double x, [In] double y, [In] double w, [In] double h);

        [DispId(8)]
        void ellipse([In] double x, [In] double y, [In] double radiusX, [In] double radiusY,
                     [In] double rotation, [In] double startAngle, [In] double endAngle,
                     [In] bool anticlockwise = false);

        [DispId(9)]
        void roundRect([In] double x, [In] double y, [In] double w, [In] double h, [In] object radii);

        [DispId(10)]
        void closePath();

        [DispId(11)]
        void addPath([In] object path, [In] object? transform = null);
    }
}
