using System;
using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public class Gradients : ITestCategory
    {
        #region ITestCategory Members

        public List<TestCase> GetTestCases()
        {
            var tests = new List<TestCase>();
            tests.Add(sample29);
            tests.Add(sample30);
            tests.Add(sample31);
            tests.Add(sample32);
            tests.Add(sample15);
            tests.Add(sample16);
            return tests;
        }

        public string Name
        {
            get { return "Gradients"; }
        }

        #endregion

        private string sample16(ICanvasRenderingContext2D ctx)
        {
            // Create gradients
            var radgrad = (ILinearCanvasGradient) ctx.createRadialGradient(45, 45, 10, 52, 50, 30);
            radgrad.addColorStop(0, "#A7D30C");
            radgrad.addColorStop(0.9f, "#019F62");
            radgrad.addColorStop(1, "rgba(1,159,98,0)");

            var radgrad2 = (ILinearCanvasGradient) ctx.createRadialGradient(105, 105, 20, 112, 120, 50);
            radgrad2.addColorStop(0, "#FF5F98");
            radgrad2.addColorStop(0.75f, "#FF0188");
            radgrad2.addColorStop(1, "rgba(255,1,136,0)");

            var radgrad3 = (ILinearCanvasGradient) ctx.createRadialGradient(95, 15, 15, 102, 20, 40);
            radgrad3.addColorStop(0, "#00C9FF");
            radgrad3.addColorStop(0.8f, "#00B5E2");
            radgrad3.addColorStop(1, "rgba(0,201,255,0)");

            var radgrad4 = (ILinearCanvasGradient) ctx.createRadialGradient(0, 150, 50, 0, 140, 90);
            radgrad4.addColorStop(0, "#F4F201");
            radgrad4.addColorStop(0.8f, "#E4C700");
            radgrad4.addColorStop(1, "rgba(228,199,0,0)");

            // draw shapes
            ctx.fillStyle = radgrad4;
            ctx.fillRect(0, 0, 150, 150);
            ctx.fillStyle = radgrad3;
            ctx.fillRect(0, 0, 150, 150);
            ctx.fillStyle = radgrad2;
            ctx.fillRect(0, 0, 150, 150);
            ctx.fillStyle = radgrad;
            ctx.fillRect(0, 0, 150, 150);
            return @"Originals\Gradients\sample16.png";
        }

        private string sample15(ICanvasRenderingContext2D ctx)
        {
            // Create gradients
            var lingrad = (ILinearCanvasGradient) ctx.createLinearGradient(0, 0, 0, 150);
            lingrad.addColorStop(0, "#00ABEB");
            lingrad.addColorStop(0.5, "#fff");
            lingrad.addColorStop(0.5, "#26C000");
            lingrad.addColorStop(1, "#fff");

            var lingrad2 = (ILinearCanvasGradient) ctx.createLinearGradient(0, 50, 0, 95);
            lingrad2.addColorStop(0.5, "#000");
            lingrad2.addColorStop(1, "rgba(0,0,0,0)");

            ctx.fillStyle = lingrad;
            ctx.fillRect(10, 10, 130, 130);
            ctx.strokeStyle = lingrad2;
            ctx.strokeRect(50, 50, 50, 50);
            return @"Originals\Gradients\sample15.png";
        }

        private string sample32(ICanvasRenderingContext2D ctx)
        {
            var lingrad2 = (ILinearCanvasGradient) ctx.createLinearGradient(50, 50, 100, 100);
            lingrad2.addColorStop(0.5, "#000");
            lingrad2.addColorStop(0.7, "red");
            lingrad2.addColorStop(1, "rgba(0,0,0,0)");
            ctx.strokeStyle = lingrad2;

            ctx.beginPath();
            ctx.arc(75, 75, 50, 0, Math.PI*2, true); // Outer circle
            ctx.moveTo(110, 75);
            ctx.arc(75, 75, 35, 0, Math.PI, false); // Mouth (clockwise)
            ctx.moveTo(65, 65);
            ctx.arc(60, 65, 5, 0, Math.PI*2, true); // Left eye
            ctx.moveTo(95, 65);
            ctx.arc(90, 65, 5, 0, Math.PI*2, true); // Right eye
            //50, 50, 100, 100


            ctx.stroke();

            return @"Originals\Gradients\sample32.png";
        }

        private string sample31(ICanvasRenderingContext2D ctx)
        {
            // Create gradients
            var lingrad = (ILinearCanvasGradient) ctx.createLinearGradient(0, 50, 0, 100);
            lingrad.addColorStop(0, "#00ABEB");
            lingrad.addColorStop(0.5, "#fff");
            lingrad.addColorStop(0.5, "#26C000");
            lingrad.addColorStop(1, "#fff");

            var lingrad2 = (ILinearCanvasGradient) ctx.createLinearGradient(0, 175, 220, 175);
            lingrad2.addColorStop(0.5, "#000");
            lingrad2.addColorStop(0.7, "red");
            lingrad2.addColorStop(1, "rgba(0,0,0,0)");

            // draw shapes
            ctx.fillStyle = lingrad;
            ctx.fillRect(0, 0, 390, 390);
            ctx.strokeStyle = lingrad2;
            ctx.strokeRect(100, 100, 150, 150);

            return @"Originals\Gradients\sample31.png";
        }

        private string sample30(ICanvasRenderingContext2D ctx)
        {
            // Create gradients
            var lingrad = (ILinearCanvasGradient) ctx.createLinearGradient(0, 0, 0, 450);
            lingrad.addColorStop(0, "#00ABEB");
            lingrad.addColorStop(0.5, "#fff");
            lingrad.addColorStop(0.5, "#26C000");
            lingrad.addColorStop(1, "#fff");

            var lingrad2 = (ILinearCanvasGradient) ctx.createLinearGradient(220, 200, 150, 175);
            lingrad2.addColorStop(0.5, "#000");
            lingrad2.addColorStop(0.7, "red");
            lingrad2.addColorStop(1, "rgba(0,0,0,0)");

            // draw shapes
            ctx.fillStyle = lingrad;
            ctx.fillRect(0, 0, 390, 390);
            ctx.strokeStyle = lingrad2;
            ctx.strokeRect(100, 100, 150, 150);

            return @"Originals\Gradients\sample30.png";
        }

        private string sample29(ICanvasRenderingContext2D ctx)
        {
            // Create gradients
            var lingrad = (ILinearCanvasGradient) ctx.createLinearGradient(0, 0, 0, 450);
            lingrad.addColorStop(0, "#00ABEB");
            lingrad.addColorStop(0.5, "#fff");
            lingrad.addColorStop(0.5, "#26C000");
            lingrad.addColorStop(1, "#fff");

            var lingrad2 = (ILinearCanvasGradient) ctx.createLinearGradient(150, 175, 220, 200);
            lingrad2.addColorStop(0.5, "#000");
            lingrad2.addColorStop(0.7, "red");
            lingrad2.addColorStop(1, "rgba(0,0,0,0)");

            // assign gradient to fill
            ctx.fillStyle = lingrad;
            // draw shape
            ctx.fillRect(0, 0, 390, 390);
            // assign gradient to stroke
            ctx.strokeStyle = lingrad2;
            // draw shape
            ctx.strokeRect(100, 100, 150, 150);

            return @"Originals\Gradients\sample29.png";
        }
    }
}