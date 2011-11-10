using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
// using SharpCanvas.Tests.Forms;
using SharpCanvas.Media;

namespace SharpCanvas.Tests.Media
{
    public class Animation : ITestCategory, IAnimation
    {
        private readonly int interval;
        private readonly Storyboard storyboard = new Storyboard();
        private readonly HTMLElement surface;
        private FlyingDragon dragon;

        public Animation(HTMLElement panel, int interval)
        {
            surface = panel;
            this.interval = interval;
            IsStarted = false;
            //this.label = label;
        }

        public bool IsStarted { get; set; }

        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample28);
            return tests;
        }

        public string Name
        {
            get { return "Animation"; }
        }

        #endregion
        
        private string sample28(ICanvasRenderingContext2D ctx)
        {
            Thread.Sleep(interval);
            dragon = new FlyingDragon(surface);
            // Create the animation targetting the StrokeDashOffset property
            var anima = new Int32Animation
                            {
                                Duration = new Duration(TimeSpan.FromSeconds(20)),
                                FillBehavior = FillBehavior.Stop,
                                From = 0,
                                By = 1,
                                To = 1000
                            };
            // DesiredFrameRate can help reduce CPU cost and improve smoothness.
            // It can also cause beating if used improperly. Use with caution!
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 120 });
            Timeline.SetDesiredFrameRate(anima, 20); // 20 fps

            // Final per-figure setup
            Storyboard.SetTarget(anima, dragon);
            //Storyboard.SetDesiredFrameRate(anima, 10);
            Storyboard.SetTargetProperty(anima, new PropertyPath(FlyingDragon.IndexProperty));
            storyboard.Children.Add(anima);
            // Begin the animation
            storyboard.Begin();

            IsStarted = true;
            return string.Empty;
        }
    
        public void Stop()
        {
            storyboard.Stop();
            storyboard.Children.Clear();
            IsStarted = false;
            Thread.Sleep(interval);
        }
    }
}