using NUnit.Framework;
using SharpCanvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Base class for unified tests that run against multiple canvas backends.
    /// Tests derived from this class will automatically run against all registered context providers.
    /// </summary>
    [TestFixtureSource(nameof(GetContextProviders))]
    public abstract class UnifiedTestBase
    {
        protected ICanvasContextProvider Provider { get; set; } = null!;
        protected ICanvasRenderingContext2D Context { get; private set; } = null!;

        public UnifiedTestBase(ICanvasContextProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Override to specify custom canvas dimensions. Default is 200x200.
        /// </summary>
        protected virtual (int width, int height) CanvasDimensions => (200, 200);

        /// <summary>
        /// Override to enable saving test output images for debugging. Default is false.
        /// </summary>
        protected virtual bool SaveTestImages => false;

        /// <summary>
        /// Returns all available context providers to test against.
        /// Currently only Skia is supported in cross-platform scenarios.
        /// </summary>
        public static IEnumerable<ICanvasContextProvider> GetContextProviders()
        {
            yield return new SkiaContextProvider();

            // System.Drawing context provider would be added here for Windows-only testing
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return new SystemDrawingContextProvider();
            }
        }

        [SetUp]
        public void SetUp()
        {
            var (width, height) = CanvasDimensions;
            Context = Provider.CreateContext(width, height);
        }

        [TearDown]
        public void TearDown()
        {
            if (SaveTestImages && TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var fileName = $"{testName}_{Provider.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var outputPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestOutputs", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
                Provider.SaveToPng(Context, outputPath);
                TestContext.WriteLine($"Test output saved to: {outputPath}");
            }

            Provider.DisposeContext(Context);
        }

        /// <summary>
        /// Helper method to get a pixel color from the canvas.
        /// </summary>
        protected (byte r, byte g, byte b, byte a) GetPixel(int x, int y)
        {
            return Provider.GetPixel(Context, x, y);
        }

        /// <summary>
        /// Helper method to assert that a pixel matches an expected color with tolerance.
        /// </summary>
        protected void AssertPixelColor(int x, int y, byte expectedR, byte expectedG, byte expectedB, byte expectedA, int tolerance = 2)
        {
            var (r, g, b, a) = GetPixel(x, y);

            Assert.That(Math.Abs(r - expectedR), Is.LessThanOrEqualTo(tolerance),
                $"Red channel mismatch at ({x}, {y}). Expected: {expectedR}, Actual: {r}");
            Assert.That(Math.Abs(g - expectedG), Is.LessThanOrEqualTo(tolerance),
                $"Green channel mismatch at ({x}, {y}). Expected: {expectedG}, Actual: {g}");
            Assert.That(Math.Abs(b - expectedB), Is.LessThanOrEqualTo(tolerance),
                $"Blue channel mismatch at ({x}, {y}). Expected: {expectedB}, Actual: {b}");
            Assert.That(Math.Abs(a - expectedA), Is.LessThanOrEqualTo(tolerance),
                $"Alpha channel mismatch at ({x}, {y}). Expected: {expectedA}, Actual: {a}");
        }

        /// <summary>
        /// Helper to assert that a pixel is transparent (alpha near 0).
        /// </summary>
        protected void AssertPixelTransparent(int x, int y, int tolerance = 2)
        {
            var (_, _, _, a) = GetPixel(x, y);
            Assert.That(a, Is.LessThanOrEqualTo(tolerance),
                $"Expected transparent pixel at ({x}, {y}), but alpha was {a}");
        }

        /// <summary>
        /// Helper to assert that a pixel is opaque (alpha near 255).
        /// </summary>
        protected void AssertPixelOpaque(int x, int y, int tolerance = 2)
        {
            var (_, _, _, a) = GetPixel(x, y);
            Assert.That(a, Is.GreaterThanOrEqualTo(255 - tolerance),
                $"Expected opaque pixel at ({x}, {y}), but alpha was {a}");
        }
    }
}
