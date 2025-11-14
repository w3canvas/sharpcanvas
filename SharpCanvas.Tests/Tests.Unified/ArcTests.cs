using NUnit.Framework;
using System;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Comprehensive tests for the arc() method across all canvas backends.
    /// Tests verify correct implementation of the HTML5 Canvas arc() specification.
    /// </summary>
    [TestFixture]
    public class ArcTests : UnifiedTestBase
    {
        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_SimpleCircle_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw a simple circle
                Context.beginPath();
                Context.arc(100, 100, 50, 0, 2 * Math.PI, false);
                Context.fillStyle = "red";
                Context.fill();

                // Verify center is filled
                AssertPixelColor(100, 100, 255, 0, 0, 255, tolerance: 5);

                // Verify edge points are filled
                AssertPixelColor(100, 50, 255, 0, 0, 255, tolerance: 5);  // Top
                AssertPixelColor(100, 150, 255, 0, 0, 255, tolerance: 5); // Bottom
                AssertPixelColor(50, 100, 255, 0, 0, 255, tolerance: 5);  // Left
                AssertPixelColor(150, 100, 255, 0, 0, 255, tolerance: 5); // Right

                // Verify outside is transparent
                AssertPixelTransparent(10, 10);
                AssertPixelTransparent(190, 190);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_HalfCircle_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw a half circle (top half)
                Context.beginPath();
                Context.arc(100, 100, 50, Math.PI, 2 * Math.PI, false);
                Context.fillStyle = "blue";
                Context.fill();

                // Top half should be filled
                AssertPixelColor(100, 75, 0, 0, 255, 255, tolerance: 5);

                // Bottom half should be transparent
                AssertPixelTransparent(100, 125);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_QuarterCircle_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw a quarter circle (right side)
                Context.beginPath();
                Context.arc(100, 100, 50, -Math.PI / 2, 0, false);
                Context.fillStyle = "green";
                Context.fill();

                // Right top quadrant should be filled
                AssertPixelColor(125, 75, 0, 128, 0, 255, tolerance: 5);

                // Left side should be transparent
                AssertPixelTransparent(75, 100);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_Anticlockwise_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw arc anticlockwise
                Context.beginPath();
                Context.arc(100, 100, 50, 0, Math.PI, true);
                Context.fillStyle = "purple";
                Context.fill();

                // Top half should be filled (anticlockwise from 0 to PI)
                AssertPixelColor(100, 75, 128, 0, 128, 255, tolerance: 5);

                // Bottom half should be transparent
                AssertPixelTransparent(100, 125);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_FullCirclePlusSome_DrawsFullCircle(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Arc larger than 2*PI should draw a full circle
                Context.beginPath();
                Context.arc(100, 100, 50, 0, 3 * Math.PI, false);
                Context.fillStyle = "orange";
                Context.fill();

                // All quadrants should be filled
                AssertPixelColor(100, 50, 255, 165, 0, 255, tolerance: 5);  // Top
                AssertPixelColor(100, 150, 255, 165, 0, 255, tolerance: 5); // Bottom
                AssertPixelColor(50, 100, 255, 165, 0, 255, tolerance: 5);  // Left
                AssertPixelColor(150, 100, 255, 165, 0, 255, tolerance: 5); // Right
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_ZeroRadius_ThrowsOrHandlesGracefully(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Zero radius should not crash
                Context.beginPath();
                Context.arc(100, 100, 0, 0, 2 * Math.PI, false);
                Context.stroke();

                // Should not throw, canvas should remain mostly transparent
                AssertPixelTransparent(100, 100);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_WithExistingPath_ConnectsToStartPoint(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Start with a line, then add arc
                Context.beginPath();
                Context.moveTo(50, 50);
                Context.lineTo(100, 50);
                Context.arc(100, 100, 50, -Math.PI / 2, Math.PI / 2, false);
                Context.strokeStyle = "black";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have a connecting line from (100, 50) to arc start
                AssertPixelColor(100, 50, 0, 0, 0, 255, tolerance: 5);
                AssertPixelColor(100, 75, 0, 0, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_EmptyPath_MovesToStartPoint(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Arc on empty path should implicitly moveTo the start point
                Context.beginPath();
                Context.arc(100, 100, 50, 0, Math.PI / 2, false);
                Context.strokeStyle = "red";
                Context.lineWidth = 2;
                Context.stroke();

                // Arc should start at (150, 100) and not have a line to origin
                AssertPixelColor(150, 100, 255, 0, 0, 255, tolerance: 5);
                AssertPixelColor(100, 150, 255, 0, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_StartAngleGreaterThanEndAngle_Clockwise_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Start > End with clockwise = false should go the "long way"
                Context.beginPath();
                Context.arc(100, 100, 50, Math.PI, Math.PI / 2, false);
                Context.fillStyle = "cyan";
                Context.fill();

                // Should fill the larger arc (270 degrees)
                AssertPixelColor(50, 100, 0, 255, 255, 255, tolerance: 5);  // Left
                AssertPixelColor(100, 150, 0, 255, 255, 255, tolerance: 5); // Bottom
                AssertPixelColor(100, 50, 0, 255, 255, 255, tolerance: 5);  // Top
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_NegativeRadius_ThrowsError(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Negative radius should throw according to HTML5 spec
                Assert.Throws<Exception>(() =>
                {
                    Context.arc(100, 100, -50, 0, Math.PI, false);
                });
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_WithTransform_AppliesTransformCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Apply a translation and draw arc
                Context.translate(50, 50);
                Context.beginPath();
                Context.arc(50, 50, 30, 0, 2 * Math.PI, false);
                Context.fillStyle = "magenta";
                Context.fill();

                // Arc should be at (100, 100) due to translation
                AssertPixelColor(100, 100, 255, 0, 255, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void Arc_StrokedNotFilled_DrawsOutlineOnly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw stroked circle
                Context.beginPath();
                Context.arc(100, 100, 50, 0, 2 * Math.PI, false);
                Context.strokeStyle = "black";
                Context.lineWidth = 2;
                Context.stroke();

                // Edge should be black
                AssertPixelColor(150, 100, 0, 0, 0, 255, tolerance: 5);

                // Center should be transparent
                AssertPixelTransparent(100, 100);
            }
            finally
            {
                TearDown();
            }
        }
    }
}
