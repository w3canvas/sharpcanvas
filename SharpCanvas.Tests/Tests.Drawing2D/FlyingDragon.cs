using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Timer=System.Timers.Timer;
using SharpCanvas.Tests;
using SharpCanvas.Forms;

namespace SharpCanvas.Tests.Forms
{
    public class FlyingDragon
    {
        private static readonly object sync = new object();
        private static int framesCount;
        private readonly CanvasRenderingContext2D _ctx;
        private readonly Graphics _g;

        private readonly BufferedGraphicsContext GraphicManager;

        private readonly int interval;
        private readonly BufferedGraphics ManagedBackBuffer;
        private Timer timer;

        public FlyingDragon(Graphics g, int width, int height, Pen stroke, Fill fill, int interval)
        {
            _g = g;
            GraphicManager = BufferedGraphicsManager.Current;
            GraphicManager.MaximumBuffer =
                new Size(width + 1, height + 1);
            ManagedBackBuffer =
                GraphicManager.Allocate(_g, new Rectangle(0, 0, width, height));
            _ctx = new CanvasRenderingContext2D(ManagedBackBuffer.Graphics, null, stroke, fill, false);
            this.interval = interval;
        }

        private void fillCircle(CanvasRenderingContext2D ctx, double r)
        {
            ctx.beginPath();
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
            ctx.beginPath();
            ctx.fillStyle = "white";
            ctx.arc(0, 0, 10, 0, Math.PI*2, true);
            ctx.fill();
            ctx.restore();

            // eye
            ctx.translate(9, -4);
            ctx.beginPath();
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

        private void fly(CanvasRenderingContext2D ctx, double i, ToolStripStatusLabel label)
        {
            ElapsedEventHandler inner = delegate
                                            {
                                                bool isLocked = !Monitor.TryEnter(sync);
                                                if (!isLocked)
                                                {
                                                    i++;

                                                    ctx.fillStyle = "white";
                                                    ctx.fillRect(-1500, -1500, 3000, 3000);

                                                    ctx.save();
                                                    ctx.translate(0, Math.Cos(i*0.1)*40);

                                                    double a = Math.Sin(i*0.1);

                                                    // right wing
                                                    ctx.save();
                                                    ctx.rotate(Math.PI*0.4);
                                                    wing(ctx, 18, a);
                                                    ctx.restore();

                                                    // left wing
                                                    ctx.save();
                                                    ctx.scale(-1, 1);
                                                    ctx.rotate(Math.PI*0.4);
                                                    wing(ctx, 18, a);
                                                    ctx.restore();

                                                    // tail
                                                    ctx.save();
                                                    tail(ctx, 20, Math.Sin(i*0.05));
                                                    ctx.restore();

                                                    // head
                                                    neck(ctx, 22);

                                                    ctx.restore();

                                                    ManagedBackBuffer.Render(_g);
                                                    _g.Flush(FlushIntention.Sync);
                                                    framesCount++;
                                                    Monitor.Exit(sync);
                                                }
                                            };
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += inner;
            timer.Enabled = true;

            var counter = new Timer();
            counter.Interval = 1000;
            counter.Elapsed += delegate
                                   {
                                       label.Text = "fps: " + framesCount + " / " + 1000/interval;
                                       lock (sync)
                                       {
                                           framesCount = 0;
                                       }
                                   };
            counter.Enabled = true;
        }

        public void stop()
        {
            timer.Stop();
            timer.Enabled = false;
        }

        public void draw(ToolStripStatusLabel label)
        {
            _ctx.translate(280, 200);
            _ctx.scale(0.4, 0.4);

            fly(_ctx, 1, label);
        }

        #region Nested type: Fly

        private delegate void Fly(object source, ElapsedEventArgs e);

        #endregion
    }
}