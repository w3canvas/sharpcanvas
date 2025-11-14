using NUnit.Framework;
using System;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Comprehensive tests for the arcTo() method across all canvas backends.
    /// Tests verify correct implementation of the HTML5 Canvas arcTo() specification.
    /// </summary>
    [TestFixture]
    public class ArcToTests : UnifiedTestBase
    {
        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_BasicCorner_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Draw a rounded corner
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.arcTo(100, 100, 100, 50, 20);
                Context.strokeStyle = "black";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have a line from start to tangent point
                AssertPixelColor(75, 100, 0, 0, 0, 255, tolerance: 5);

                // Should have arc at the corner
                AssertPixelColor(95, 95, 0, 0, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_ZeroRadius_DrawsStraightLine(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Zero radius should draw straight lines
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.arcTo(100, 100, 100, 50, 0);
                Context.strokeStyle = "red";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have straight line to the corner point
                AssertPixelColor(75, 100, 255, 0, 0, 255, tolerance: 5);
                AssertPixelColor(100, 100, 255, 0, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_CollinearPoints_DrawsStraightLine(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // All three points on a straight line
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.arcTo(100, 100, 150, 100, 20);
                Context.strokeStyle = "blue";
                Context.lineWidth = 2;
                Context.stroke();

                // Should draw straight line
                AssertPixelColor(75, 100, 0, 0, 255, 255, tolerance: 5);
                AssertPixelColor(100, 100, 0, 0, 255, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_NegativeRadius_ThrowsError(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Negative radius should throw
                Context.beginPath();
                Context.moveTo(50, 100);

                Assert.Throws<Exception>(() =>
                {
                    Context.arcTo(100, 100, 100, 50, -20);
                });
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_RightAngleCorner_90Degrees_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Create a right-angle rounded corner
                Context.beginPath();
                Context.moveTo(50, 50);
                Context.lineTo(50, 100);
                Context.arcTo(50, 150, 100, 150, 25);
                Context.lineTo(150, 150);
                Context.strokeStyle = "green";
                Context.lineWidth = 2;
                Context.stroke();

                // Verify vertical line
                AssertPixelColor(50, 75, 0, 128, 0, 255, tolerance: 5);

                // Verify rounded corner
                AssertPixelColor(55, 135, 0, 128, 0, 255, tolerance: 5);

                // Verify horizontal line
                AssertPixelColor(125, 150, 0, 128, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_LargeRadius_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Large radius for smooth curve
                Context.beginPath();
                Context.moveTo(30, 100);
                Context.arcTo(100, 100, 100, 30, 50);
                Context.strokeStyle = "purple";
                Context.lineWidth = 3;
                Context.stroke();

                // Should have visible arc
                AssertPixelColor(75, 100, 128, 0, 128, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_SmallRadius_DrawsTightCorner(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Small radius for tight corner
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.arcTo(100, 100, 100, 50, 5);
                Context.strokeStyle = "orange";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have line and tight arc
                AssertPixelColor(75, 100, 255, 165, 0, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_SameStartAndP1_HandlesGracefully(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Current point same as p1
                Context.beginPath();
                Context.moveTo(100, 100);
                Context.arcTo(100, 100, 150, 150, 20);
                Context.strokeStyle = "cyan";
                Context.lineWidth = 2;
                Context.stroke();

                // Should handle gracefully (spec says add line to p1)
                // Most implementations will just move to p1
                AssertPixelColor(100, 100, 0, 255, 255, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_SameP1AndP2_HandlesGracefully(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // P1 and P2 are the same
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.arcTo(100, 100, 100, 100, 20);
                Context.strokeStyle = "magenta";
                Context.lineWidth = 2;
                Context.stroke();

                // Should draw line to p1
                AssertPixelColor(75, 100, 255, 0, 255, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_WithTransform_AppliesTransformCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Apply transform and draw arcTo
                Context.translate(50, 50);
                Context.scale(0.5, 0.5);
                Context.beginPath();
                Context.moveTo(0, 100);
                Context.arcTo(100, 100, 100, 0, 40);
                Context.strokeStyle = "red";
                Context.lineWidth = 4;
                Context.stroke();

                // Should be transformed
                AssertPixelColor(60, 75, 255, 0, 0, 255, tolerance: 10);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_MultipleConsecutiveCalls_CreatesRoundedPath(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Create a rounded rectangle using multiple arcTo calls
                Context.beginPath();
                Context.moveTo(50, 70);
                Context.lineTo(50, 130);
                Context.arcTo(50, 150, 70, 150, 20);
                Context.lineTo(130, 150);
                Context.arcTo(150, 150, 150, 130, 20);
                Context.lineTo(150, 70);
                Context.arcTo(150, 50, 130, 50, 20);
                Context.lineTo(70, 50);
                Context.arcTo(50, 50, 50, 70, 20);
                Context.closePath();
                Context.strokeStyle = "black";
                Context.lineWidth = 2;
                Context.stroke();

                // Verify corners are rounded
                AssertPixelColor(50, 100, 0, 0, 0, 255, tolerance: 5);  // Left edge
                AssertPixelColor(150, 100, 0, 0, 0, 255, tolerance: 5); // Right edge
                AssertPixelColor(100, 50, 0, 0, 0, 255, tolerance: 5);  // Top edge
                AssertPixelColor(100, 150, 0, 0, 0, 255, tolerance: 5); // Bottom edge
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_ObtuseAngle_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Obtuse angle (> 90 degrees)
                Context.beginPath();
                Context.moveTo(50, 50);
                Context.lineTo(100, 100);
                Context.arcTo(120, 120, 170, 70, 30);
                Context.strokeStyle = "brown";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have visible arc at obtuse angle
                AssertPixelColor(100, 100, 165, 42, 42, 255, tolerance: 10);
            }
            finally
            {
                TearDown();
            }
        }

        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void ArcTo_AcuteAngle_DrawsCorrectly(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Acute angle (< 90 degrees)
                Context.beginPath();
                Context.moveTo(50, 100);
                Context.lineTo(90, 100);
                Context.arcTo(100, 100, 100, 90, 10);
                Context.strokeStyle = "navy";
                Context.lineWidth = 2;
                Context.stroke();

                // Should have tight arc
                AssertPixelColor(85, 100, 0, 0, 128, 255, tolerance: 5);
            }
            finally
            {
                TearDown();
            }
        }
    }
}
