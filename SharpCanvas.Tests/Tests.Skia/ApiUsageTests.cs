using NUnit.Framework;
using SharpCanvas.Shared;
using SharpCanvas.Context.Skia;
using SkiaSharp;

namespace SharpCanvas.Tests.Skia
{
    public class ApiUsageTests
    {
        [Test]
        public void TestPutImageData()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.Clear(SKColors.White);

                // Create pixel data for a 20x20 green square
                var imageDataInfo = new SKImageInfo(20, 20, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                var bytes = new byte[imageDataInfo.BytesSize];
                for (int i = 0; i < bytes.Length; i += 4)
                {
                    bytes[i] = 0;     // R
                    bytes[i + 1] = 255; // G
                    bytes[i + 2] = 0;   // B
                    bytes[i + 3] = 255; // A
                }

                // Create a bitmap from the pixel data and draw it
                var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var ptr = gcHandle.AddrOfPinnedObject();
                    using (var bitmap = new SKBitmap())
                    {
                        bitmap.InstallPixels(imageDataInfo, ptr);
                        surface.Canvas.DrawBitmap(bitmap, 10, 10);
                    }
                }
                finally
                {
                    gcHandle.Free();
                }

                var resultBitmap = new SKBitmap(info);
                surface.ReadPixels(resultBitmap.Info, resultBitmap.GetPixels(), resultBitmap.RowBytes, 0, 0);

                // Check a pixel where the image was drawn
                Assert.That(resultBitmap.GetPixel(15, 15), Is.EqualTo(new SKColor(0, 255, 0, 255)));
                // Check a pixel outside the image
                Assert.That(resultBitmap.GetPixel(40, 40), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestChangeSize()
        {
            var info = new SKImageInfo(50, 50);
            using (var surface = SKSurface.Create(info))
            {
                // Draw something on the original surface
                surface.Canvas.Clear(SKColors.Red);

                // Create a new, larger surface
                var newInfo = new SKImageInfo(100, 100, info.ColorType, info.AlphaType);
                using (var newSurface = SKSurface.Create(newInfo))
                {
                    newSurface.Canvas.Clear(SKColors.White);
                    // Copy the content
                    using (var snapshot = surface.Snapshot())
                    {
                        newSurface.Canvas.DrawImage(snapshot, 0, 0);
                    }

                    // Verify the content
                    var bitmap = new SKBitmap(newInfo);
                    newSurface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
                    Assert.That(bitmap.GetPixel(25, 25), Is.EqualTo(SKColors.Red));
                    Assert.That(bitmap.GetPixel(75, 75), Is.EqualTo(SKColors.White));
                }
            }
        }

    }
}
