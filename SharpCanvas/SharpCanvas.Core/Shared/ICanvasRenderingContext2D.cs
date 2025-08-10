using System.Runtime.InteropServices;

// using FilterSequence.FilterSet;

namespace SharpCanvas
{
    // FIXME: is this used, and should the typo be fixed?
    public delegate void OnPartialDrawHanlder();

    /// <summary>
    /// This is a .NET translation of the canvas rendering context 2D. Parameter types have been tranlated to
    /// corresponding .NET equivalents. Optional parameters (do not exist in .NET but be passed in Type.Missing)
    /// have been removed and properties translated to .NET properties.
    /// </summary>
    [Guid("2F98211C-7A71-4588-8D4A-AD83CA80BAE7"),
     InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICanvasRenderingContext2D
    {
        // Common

        [DispId(1)]
        object prototype();

        [DispId(2)]
        object __proto__ { get; }

        // State
        [DispId(5)]
        void save();

        [DispId(6)]
        void restore();

        // Transforms
        [DispId(10)]
        void scale([In] double x, [In] double y);

        [DispId(11)]
        void rotate([In] double angle);

        [DispId(12)]
        void translate([In] double x, [In] double y);

        [DispId(13)]
        void transform([In] double m11, [In] double m12, [In] double m21, [In] double m22, [In] double dx,
                       [In] double dy);

        [DispId(14)]
        void setTransform([In] double m11, [In] double m12, [In] double m21, [In] double m22, [In] double dx,
                          [In] double dy);

        // properties
        [DispId(20)]
        double globalAlpha { get; set; }

        [DispId(21)]
        string globalCompositeOperation { get; set; }

        [DispId(22)]
        object strokeStyle { get; set; }

        [DispId(23)]
        object fillStyle { get; set; }

        [DispId(24)]
        double lineWidth { get; set; }

        [DispId(25)]
        string lineCap { get; set; }

        [DispId(26)]
        string lineJoin { get; set; }

        [DispId(27)]
        double miterLimit { get; set; }

        // shadow properties
        [DispId(28)]
        double shadowOffsetX { get; set; }

        [DispId(29)]
        double shadowOffsetY { get; set; }

        [DispId(30)]
        double shadowBlur { get; set; }

        [DispId(31)]
        string shadowColor { get; set; }

        // rectangles
        [DispId(50)]
        void clearRect([In] double x, [In] double y, [In] double w, [In] double h);

        [DispId(51)]
        void fillRect([In] double x, [In] double y, [In] double w, [In] double h);

        [DispId(52)]
        void strokeRect([In] double x, [In] double y, [In] double w, [In] double h);

        // path API
        [DispId(60)]
        void beginPath();

        [DispId(61)]
        void closePath();

        [DispId(62)]
        void moveTo([In] double x, [In] double y);

        [DispId(63)]
        void lineTo([In] double x, [In] double y);

        [DispId(64)]
        void quadraticCurveTo([In] double cpx, [In] double cpy, [In] double x, [In] double y);

        [DispId(65)]
        void bezierCurveTo([In] double cp1x, [In] double cp1y, [In] double cp2x, [In] double cp2y, [In] double x,
                           [In] double y);

        [DispId(66)]
        void arcTo([In] double x1, [In] double y1, [In] double x2, [In] double y2, [In] double radius);

        [DispId(67)]
        void arc([In] double x, [In] double y, [In] double r, [In] double startAngle, [In] double endAngle,
                 [In] bool clockwise);

        [DispId(68)]
        void rect([In] double x, [In] double y, [In] double w, [In] double h);

        // core drawIng
        [DispId(70)]
        void fill();

        [DispId(71)]
        void stroke();

        [DispId(72)]
        void clip();

        // text API
        [DispId(80)]
        string font { get; set; }

        [DispId(81)]
        string textAlign { get; set; }

        [DispId(82)]
        string textBaseLine { get; set; }

        [DispId(83)]
        void fillText([In] string text, [In] double x, [In] double y);

        [DispId(84)]
        void strokeText([In] string text, [In] double x, [In] double y);

        //[DispId(85)] void MeasureText([In] string text, [Out] ICanvasTextMetrics** pResult);

        // images
        [DispId(90)]
        void drawImage([In] object image, [In] double sx, [In] double sy, [In] double sw, [In] double sh, [In] double dx,
                       [In] double dy, [In] double dw, [In] double dh);

        [DispId(91)]
        void drawImage([In] object pImg, [In] double dx, [In] double dy, [In] double dw, [In] double dh);

        [DispId(92)]
        void drawImage([In] object pImg, [In] double dx, [In] double dy);

        // poInt-membership test
        [DispId(100)]
        bool isPointInPath([In] double x, [In] double y);

        // pixel manipulation
        [DispId(101)]
        object createLinearGradient([In] double x0, [In] double y0, [In] double x1, [In] double y1);

        [DispId(102)]
        object createPattern([In] object pImg, [In] string repeat);

        [DispId(103)]
        object createRadialGradient([In] double x0, [In] double y0, [In] double r0, [In] double x1, [In] double y1,
                                    [In] double r1);

        [DispId(104)]
        object measureText([In] string text);

        [DispId(105)]
        object getImageData([In] double sx, [In] double sy, [In] double sw, [In] double sh);

        [DispId(106)]
        object createImageData([In] double sw, [In] double sh);

        [DispId(107)]
        void putImageData([In] object pData, [In] double dx, [In] double dy);

        [DispId(108)]
        void putImageData([In] object imagedata, [In] double dx, [In] double dy, [In] double dirtyX, [In] double dirtyY,
                          [In] double dirtyWidth, [In] double dirtyHeight);


        [DispId(111)]
        object createFilterChain();

        /// <summary>
        /// WPF: commits current geometry set to the surface
        /// WinForm: do nothing
        /// </summary>
        [DispId(112)]
        void commit();

        /// <summary>
        /// back-reference to the canvas
        /// </summary>
        [DispId(113)]
        object canvas { get;}

        /// <summary>
        /// Provides with latest image from the Canvas
        /// </summary>
        /// <returns></returns>
        byte[] GetBitmap();

        /// <summary>
        /// Change size of canvas and underlying controls.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void ChangeSize(int width, int height, bool reset);

        /// <summary>
        /// Return current height of the surface
        /// </summary>
        /// <returns></returns>
        int GetHeight();

        /// <summary>
        /// Return current width of the surface
        /// </summary>
        /// <returns></returns>
        int GetWidth();

        /// <summary>
        /// Indicates wherever it visible or not (use in WinForm env.)
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Occurs when some part of image was commited to the surface
        /// </summary>
        event OnPartialDrawHanlder OnPartialDraw;
    }
}