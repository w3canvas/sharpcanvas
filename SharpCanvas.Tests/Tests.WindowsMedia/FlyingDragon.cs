using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using SharpCanvas.Media;

namespace SharpCanvas.Tests.Media
{
    public class FlyingDragon : DependencyObject
    {
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            "IndexProperty",
            typeof (int),
            typeof (FlyingDragon),
            new FrameworkPropertyMetadata(1,
                                          FrameworkPropertyMetadataOptions.AffectsRender, OnIndexChanged
                )
            );

        private static readonly object sync = new object();
        private static int i;

        private readonly HTMLElement container;
        public CanvasRenderingContext2D _ctx;

        public FlyingDragon(HTMLElement container)
        {
            this.container = container;
            _ctx = new CanvasRenderingContext2D(container, this, true);
            _ctx.translate(280, 200);
        }

        public int Index
        {
            get { return (int) GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        private void fillCircle(CanvasRenderingContext2D ctx, double r)
        {
            //ctx.beginPath();
            ctx.moveTo(r, 0);
            ctx.fillStyle = "rgb(245,50,50)";
            ctx.arc(0, 0, r, 0, Math.PI*2, true);
            ctx.fill();
        }

        private void branch(CanvasRenderingContext2D ctx, double r, double d, double t, double a)
        {
            ctx.save();
            ctx.rotate(t*a);
            ctx.translate(0, -r*(1 + d));
            wing(ctx, r*d, a);
            ctx.restore();
        }

        private void feather(CanvasRenderingContext2D ctx, double r)
        {
            if (r < 3) return;
            double d = 0.85;
            ctx.rotate(-0.03*Math.PI);
            ctx.translate(0, -r*(1 + d));
            fillCircle(ctx, r);
            feather(ctx, r*d);
        }

        private void wing(CanvasRenderingContext2D ctx, double r, double a)
        {
            if (r < 2.9) return;
            fillCircle(ctx, r);
            branch(ctx, r, 0.9561, 0.03*Math.PI, a);
            ctx.save();
            ctx.rotate(0.55*Math.PI);
            feather(ctx, 0.8*r);
            ctx.restore();
        }

        private void tail(CanvasRenderingContext2D ctx, double s, double a)
        {
            if (s < 0.5) return;
            double d = 0.98; // decay
            fillCircle(ctx, s);
            ctx.rotate(-0.15*a);
            ctx.translate(0, s*(1 + d));
            tail(ctx, s*d, a);
        }

        private void head(CanvasRenderingContext2D ctx)
        {
            fillCircle(ctx, 22);

            // mouth
            ctx.save();
            ctx.translate(-15, -3);
            //ctx.beginPath();
            ctx.fillStyle = "white";
            ctx.arc(0, 0, 10, 0, Math.PI*2, true);
            ctx.fill();
            ctx.restore();

            // eye
            ctx.translate(9, -4);
            //ctx.beginPath();
            ctx.fillStyle = "black";
            ctx.arc(0, 0, 5, 0, Math.PI*2, true);
            ctx.fill();

            // horn
            ctx.translate(6, -8);
            ctx.rotate(0.6*Math.PI);
            wing(ctx, 5.5, 1.8);
        }

        private void neck(CanvasRenderingContext2D ctx, double s)
        {
            if (s < 10)
            {
                head(ctx);
                return;
            }

            double d = 0.85;
            fillCircle(ctx, s);

            ctx.save();
            ctx.rotate(-Math.PI/2);
            ctx.translate(0, s);
            fillCircle(ctx, s/2);
            ctx.restore();

            ctx.rotate(-0.15);
            ctx.translate(0, -s*(1 + d));
            neck(ctx, s*d);
        }

        public void flyMe(CanvasRenderingContext2D ctx, int i)
        {
            container.Children.Clear();

            ctx.save();
            ctx.translate(0, Math.Cos(i*0.1)*40);

            double a = Math.Sin(i*0.1);

            // right wing
            ctx.save();
            ctx.rotate(Math.PI*0.4);
            wing(ctx, 8, a);

            ctx.commit();

            ctx.restore();

            // left wing
            ctx._path.GetNumberOfCommits();
            ctx._path.GetNumberOfPaths();

            ctx.save();
            ctx.scale(-1, 1);
            ctx.rotate(Math.PI*0.4);
            wing(ctx, 8, a);

            ctx.commit();

            ctx.restore();

            Debugger.Log(0, "Timing", "Left wing, Commits:" + ctx._path.GetNumberOfCommits());
            Debugger.Log(0, "Timing", "Left wing, Paths:" + ctx._path.GetNumberOfPaths());

            // tail
            ctx.save();
            tail(ctx, 10, Math.Sin(i*0.05));
            ctx.commit();
            ctx.restore();


            // head
            neck(ctx, 12);
            ctx.commit();
            ctx.restore();
        }

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTime s = DateTime.Now;
            if (Monitor.TryEnter(sync))
            {
                ((FlyingDragon) d).flyMe(((FlyingDragon) d)._ctx, i++);
                Monitor.Exit(sync);
            }

            DateTime t = DateTime.Now;
            Debugger.Log(0, "Timing", "flyMe: " + (t - s).TotalMilliseconds + " ms\r\n");
        }
    }
}