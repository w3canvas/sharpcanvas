using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpCanvas.Tests.Media
{
    public class HTMLElement : Canvas
    {
        private readonly DrawingVisual backgroundVisual = new DrawingVisual();

        public HTMLElement()
        {
            Background = new VisualBrush(backgroundVisual);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //CanvasRenderingContext2D context = new CanvasRenderingContext2D(drawingContext);
            //context.fillRect(10, 10, 80, 80);
            base.OnRender(drawingContext);
            //this.InvalidateVisual();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            InvalidateVisual();
        }

        public void Update(ImageSource img)
        {
            using (DrawingContext dc = backgroundVisual.RenderOpen())
            {
                dc.DrawImage(img, new Rect(0, 0, img.Width, img.Height));
            }
        }

        //public static readonly DependencyProperty AquariumGraphicProperty = DependencyProperty.Register(
        //    "AquariumGraphic",
        //    typeof (double),
        //    typeof (HTMLElement),
        //    new FrameworkPropertyMetadata((double)0.0,
        //                                  FrameworkPropertyMetadataOptions.AffectsRender
        //        )
        //    );

        //public double AquariumGraphic
        //{
        //    get { return (double)GetValue(AquariumGraphicProperty); }
        //    set { SetValue(AquariumGraphicProperty, value); }
        //}
    }
}