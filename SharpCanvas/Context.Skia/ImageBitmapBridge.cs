using SkiaSharp;
using System;
using System.IO;

#if WINDOWS
using SystemBitmap = System.Drawing.Bitmap;
using System.Drawing.Imaging;
#endif

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Provides compatibility bridges between ImageBitmap and other bitmap formats.
    /// Allows conversion between modern ImageBitmap (SkiaSharp) and legacy System.Drawing.Bitmap.
    /// </summary>
    public static class ImageBitmapBridge
    {
#if WINDOWS
        /// <summary>
        /// Converts a System.Drawing.Bitmap to an ImageBitmap (SkiaSharp)
        /// </summary>
        public static ImageBitmap FromSystemBitmap(SystemBitmap systemBitmap)
        {
            if (systemBitmap == null)
            {
                throw new ArgumentNullException(nameof(systemBitmap));
            }

            using (var memoryStream = new MemoryStream())
            {
                // Save System.Drawing.Bitmap to stream
                systemBitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                // Load into SkiaSharp bitmap
                var skBitmap = SKBitmap.Decode(memoryStream);
                if (skBitmap == null)
                {
                    throw new InvalidOperationException("Failed to decode bitmap");
                }

                return new ImageBitmap(skBitmap);
            }
        }

        /// <summary>
        /// Converts an ImageBitmap to a System.Drawing.Bitmap
        /// </summary>
        public static SystemBitmap ToSystemBitmap(ImageBitmap imageBitmap)
        {
            if (imageBitmap == null)
            {
                throw new ArgumentNullException(nameof(imageBitmap));
            }

            var skBitmap = imageBitmap.GetBitmap();
            if (skBitmap == null)
            {
                throw new InvalidOperationException("ImageBitmap has no underlying bitmap");
            }

            using (var image = SKImage.FromBitmap(skBitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var memoryStream = new MemoryStream())
            {
                data.SaveTo(memoryStream);
                memoryStream.Position = 0;
                return new SystemBitmap(memoryStream);
            }
        }
#endif

        /// <summary>
        /// Creates an ImageBitmap from raw RGBA pixel data
        /// </summary>
        public static ImageBitmap FromRawPixels(byte[] pixels, int width, int height)
        {
            if (pixels == null)
            {
                throw new ArgumentNullException(nameof(pixels));
            }

            if (pixels.Length != width * height * 4)
            {
                throw new ArgumentException("Pixel array size must match width * height * 4 (RGBA)");
            }

            var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            var bitmap = new SKBitmap(info);

            var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
            try
            {
                var ptr = gcHandle.AddrOfPinnedObject();
                bitmap.InstallPixels(info, ptr);
                // Make a copy since we're unpinning
                var copy = bitmap.Copy();
                if (copy == null)
                {
                    throw new InvalidOperationException("Failed to copy bitmap");
                }
                return new ImageBitmap(copy);
            }
            finally
            {
                gcHandle.Free();
            }
        }

        /// <summary>
        /// Extracts raw RGBA pixel data from an ImageBitmap
        /// </summary>
        public static byte[] ToRawPixels(ImageBitmap imageBitmap)
        {
            if (imageBitmap == null)
            {
                throw new ArgumentNullException(nameof(imageBitmap));
            }

            var skBitmap = imageBitmap.GetBitmap();
            if (skBitmap == null)
            {
                throw new InvalidOperationException("ImageBitmap has no underlying bitmap");
            }

            var width = skBitmap.Width;
            var height = skBitmap.Height;
            var pixels = new byte[width * height * 4];

            var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
            try
            {
                var ptr = gcHandle.AddrOfPinnedObject();
                var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                skBitmap.PeekPixels().ReadPixels(info, ptr, info.RowBytes, 0, 0);
            }
            finally
            {
                gcHandle.Free();
            }

            return pixels;
        }

        /// <summary>
        /// Creates an ImageBitmap from a file path
        /// </summary>
        public static ImageBitmap FromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Image file not found", path);
            }

            var skBitmap = SKBitmap.Decode(path);
            if (skBitmap == null)
            {
                throw new InvalidOperationException($"Failed to decode image from {path}");
            }

            return new ImageBitmap(skBitmap);
        }

        /// <summary>
        /// Saves an ImageBitmap to a file
        /// </summary>
        public static void SaveToFile(ImageBitmap imageBitmap, string path, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
        {
            if (imageBitmap == null)
            {
                throw new ArgumentNullException(nameof(imageBitmap));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var skBitmap = imageBitmap.GetBitmap();
            if (skBitmap == null)
            {
                throw new InvalidOperationException("ImageBitmap has no underlying bitmap");
            }

            using (var image = SKImage.FromBitmap(skBitmap))
            using (var data = image.Encode(format, quality))
            using (var stream = File.OpenWrite(path))
            {
                data.SaveTo(stream);
            }
        }
    }
}
