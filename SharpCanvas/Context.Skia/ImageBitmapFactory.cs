#nullable enable
using SkiaSharp;
using SharpCanvas.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Options for creating an ImageBitmap
    /// </summary>
    public class ImageBitmapOptions
    {
        /// <summary>
        /// Specifies whether the image should be decoded with premultiplied alpha.
        /// Default: "default"
        /// </summary>
        public string premultiplyAlpha { get; set; } = "default";

        /// <summary>
        /// Specifies whether the image should be decoded with color space conversion.
        /// Default: "default"
        /// </summary>
        public string colorSpaceConversion { get; set; } = "default";

        /// <summary>
        /// Specifies whether the image should be resized.
        /// Default: "none"
        /// </summary>
        public string resizeQuality { get; set; } = "low";

        /// <summary>
        /// The width to resize the image to
        /// </summary>
        public int? resizeWidth { get; set; }

        /// <summary>
        /// The height to resize the image to
        /// </summary>
        public int? resizeHeight { get; set; }

        /// <summary>
        /// Specifies the orientation of the image
        /// </summary>
        public string imageOrientation { get; set; } = "none";
    }

    /// <summary>
    /// Factory for creating ImageBitmap instances
    /// </summary>
    public static class ImageBitmapFactory
    {
        /// <summary>
        /// Creates an ImageBitmap from various image sources.
        /// </summary>
        public static ImageBitmap createImageBitmap(object source, ImageBitmapOptions? options = null)
        {
            SKBitmap? bitmap = null;

            // Handle different source types
            if (source is SKBitmap skBitmap)
            {
                bitmap = skBitmap.Copy();
            }
            else if (source is SKImage skImage)
            {
                bitmap = SKBitmap.FromImage(skImage);
            }
            else if (source is ImageBitmap imageBitmap)
            {
                var sourceBitmap = imageBitmap.GetBitmap();
                if (sourceBitmap != null)
                {
                    bitmap = sourceBitmap.Copy();
                }
            }
            else if (source is IImage image)
            {
                var imageObj = image.getImage();
                if (imageObj is SKBitmap skb)
                {
                    bitmap = skb.Copy();
                }
            }
            else if (source is IHTMLCanvasElement canvas)
            {
                var context = canvas.getCanvas();
                var bytes = context.GetBitmap();
                bitmap = SKBitmap.Decode(bytes);
            }
            else if (source is OffscreenCanvas offscreenCanvas)
            {
                // Use transferToImageBitmap internally
                return offscreenCanvas.transferToImageBitmap();
            }
            else if (source is ImageData imageData && imageData.data is byte[] data)
            {
                var info = new SKImageInfo((int)imageData.width, (int)imageData.height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                bitmap = new SKBitmap(info);
                var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var ptr = gcHandle.AddrOfPinnedObject();
                    bitmap.InstallPixels(info, ptr);
                    // Make a copy since we're unpinning
                    bitmap = bitmap.Copy();
                }
                finally
                {
                    gcHandle.Free();
                }
            }
            else if (source is byte[] bytes)
            {
                bitmap = SKBitmap.Decode(bytes);
            }
            else if (source is Stream stream)
            {
                bitmap = SKBitmap.Decode(stream);
            }
            else if (source is string path)
            {
                if (File.Exists(path))
                {
                    bitmap = SKBitmap.Decode(path);
                }
                else
                {
                    throw new ArgumentException("File not found: " + path);
                }
            }

            if (bitmap == null)
            {
                throw new ArgumentException("Unsupported image source type");
            }

            // Apply options if provided
            if (options != null)
            {
                bitmap = ApplyOptions(bitmap, options);
            }

            return new ImageBitmap(bitmap);
        }

        /// <summary>
        /// Creates an ImageBitmap from a source with a specific rectangle
        /// </summary>
        public static ImageBitmap createImageBitmap(object source, int sx, int sy, int sw, int sh, ImageBitmapOptions? options = null)
        {
            // First create the full bitmap
            var fullBitmap = createImageBitmap(source, null);
            var sourceBitmap = fullBitmap.GetBitmap();

            if (sourceBitmap == null)
            {
                throw new InvalidOperationException("Failed to create source bitmap");
            }

            // Extract the region
            var subset = new SKRectI(sx, sy, sx + sw, sy + sh);
            var croppedBitmap = new SKBitmap(sw, sh);

            if (!sourceBitmap.ExtractSubset(croppedBitmap, subset))
            {
                // If ExtractSubset fails, manually copy the region
                using (var canvas = new SKCanvas(croppedBitmap))
                {
                    var sourceRect = new SKRect(sx, sy, sx + sw, sy + sh);
                    var destRect = new SKRect(0, 0, sw, sh);
                    canvas.DrawBitmap(sourceBitmap, sourceRect, destRect);
                }
            }

            // Dispose the full bitmap since we only need the cropped version
            fullBitmap.close();

            // Apply options if provided
            if (options != null)
            {
                croppedBitmap = ApplyOptions(croppedBitmap, options);
            }

            return new ImageBitmap(croppedBitmap);
        }

        private static SKBitmap ApplyOptions(SKBitmap bitmap, ImageBitmapOptions options)
        {
            SKBitmap result = bitmap;

            // Handle resizing
            if (options.resizeWidth.HasValue || options.resizeHeight.HasValue)
            {
                int targetWidth = options.resizeWidth ?? bitmap.Width;
                int targetHeight = options.resizeHeight ?? bitmap.Height;

                var samplingOptions = options.resizeQuality.ToLower() switch
                {
                    "high" => new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear),
                    "medium" => new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None),
                    "low" => new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None),
                    _ => new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None)
                };

                var resized = bitmap.Resize(new SKImageInfo(targetWidth, targetHeight), samplingOptions);
                if (resized != null)
                {
                    if (result != bitmap)
                    {
                        result.Dispose();
                    }
                    result = resized;
                }
            }

            // Handle alpha premultiplication
            if (options.premultiplyAlpha == "premultiply")
            {
                var info = result.Info.WithAlphaType(SKAlphaType.Premul);
                var premultiplied = new SKBitmap(info);
                result.CopyTo(premultiplied, info.ColorType);
                if (result != bitmap)
                {
                    result.Dispose();
                }
                result = premultiplied;
            }
            else if (options.premultiplyAlpha == "none")
            {
                var info = result.Info.WithAlphaType(SKAlphaType.Unpremul);
                var unpremultiplied = new SKBitmap(info);
                result.CopyTo(unpremultiplied, info.ColorType);
                if (result != bitmap)
                {
                    result.Dispose();
                }
                result = unpremultiplied;
            }

            return result;
        }
    }
}
