using System;
using System.Collections.Generic;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests
{
    public class Images : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample17);
            tests.Add(sample7);
            tests.Add(sample6);
            tests.Add(sample5);
            tests.Add(GrayscaleImage);
            tests.Add(PatternAndTransformation);
            return tests;
        }

        public string Name
        {
            get { return "Images"; }
        }

        #endregion

        private string sample17(ICanvasRenderingContext2D ctx)
        {
            // create new image object to use as pattern
            IImageData img = new ImageData();
            img.src = @"wallpaper.png";

            // create pattern
            object ptrn = ctx.createPattern(img, "repeat");
            ctx.fillStyle = ptrn;
            ctx.fillRect(0, 0, 150, 150);
            return @"Originals\Images\sample17.png";
        }

        private string PatternAndTransformation(ICanvasRenderingContext2D ctx)
        {
            // create new image object to use as pattern
            IImageData img = new ImageData();
            img.src = @"wallpaper.png";

            // create pattern
            object ptrn = ctx.createPattern(img, "repeat");
            ctx.translate(50, 0);
            ctx.rotate(Math.PI*2/(6));
            ctx.fillStyle = ptrn;
            ctx.fillRect(0, 0, 150, 150);
            return @"Originals\Images\PatternAndTransformation.png";
        }

        private string sample7(ICanvasRenderingContext2D ctx)
        {
            var img = new ImageData();
            img.src = @"rhino.jpg";
            var imgFrame = new ImageData();
            imgFrame.src = @"picture_frame.png";
            ctx.drawImage(img, 33, 71, 104, 124, 21, 20, 87, 104);
            //ctx.commit();
            ctx.drawImage(imgFrame, 0, 0, 0, 0);
            //ctx.commit();
            return @"Originals\Images\sample7.png";
        }

        private string sample6(ICanvasRenderingContext2D ctx)
        {
            var img = new ImageData();
            img.src = @"rhino.jpg";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ctx.drawImage(img, j*50, i*38, 50, 38);
                }
            }
            return @"Originals\Images\sample6.png";
        }

        private string sample5(ICanvasRenderingContext2D ctx)
        {
            var img = new ImageData();
            img.src = @"backdrop.png";
            ctx.drawImage(img, 0, 0, 0, 0);
            ctx.beginPath();
            ctx.moveTo(30, 96);
            ctx.lineTo(70, 66);
            ctx.lineTo(103, 76);
            ctx.lineTo(170, 15);
            ctx.stroke();
            return @"Originals\Images\sample5.png";
        }

        private string GrayscaleImage(ICanvasRenderingContext2D ctx)
        {
            var img = new ImageData();
            img.src = "pet.jpg";
            grayscale(img, ctx);
            return @"Originals\Images\grayscale.png";
        }

        private void grayscale(ImageData image, ICanvasRenderingContext2D ctx)
        {
            ctx.drawImage(image, 0, 0);
            uint imgWidth = image.width;
            uint imgHeight = image.height;
            var imageData = (ImageData) ctx.getImageData(0, 0, imgWidth, imgHeight);
            var data = (byte[])imageData.data;
            for (ulong i = 0; i < imageData.height; i++)
            {
                for (ulong j = 0; j < imageData.width; j++)
                {
                    ulong index = (i*4)*imageData.width + (j*4);
                    var red = data[index];
                    var green = data[index + 1];
                    var blue = data[index + 2];
                    var alpha = data[index + 3];
                    var average = (byte) ((red + green + blue)/3);
                    data[index] = average;
                    data[index + 1] = average;
                    data[index + 2] = average;
                    data[index + 3] = alpha;
                }
            }
            ctx.putImageData(imageData, imageData.width + 5, 0, 0, 0, imageData.width, imageData.height);
        }
    }
}