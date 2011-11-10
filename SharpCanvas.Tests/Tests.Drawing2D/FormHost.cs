using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpCanvas.Forms;

namespace SharpCanvas.Tests.Forms
{
    public partial class FormHost : Form
    {
        private readonly List<TestCase> tests = new List<TestCase>();
        private int testIndex;

        public FormHost()
        {
            InitializeComponent();

            //tests.Add(sample1);
            //tests.Add(sample2);
            //tests.Add(sample3);
            //tests.Add(sample4);
            //tests.Add(sample5);
            //tests.Add(sample6);
            //tests.Add(sample7);
            //tests.Add(sample8);
            //tests.Add(sample9);
            //tests.Add(sample10);
            //tests.Add(sample11);
            //tests.Add(sample12);
            //tests.Add(sample13);
            //tests.Add(sample14);
            ////tests.Add(sample15);
            ////tests.Add(sample16);
            //tests.Add(sample17);
            //tests.Add(sample18);
            //tests.Add(sample19);
            //tests.Add(sample20);
            //tests.Add(sample21);
            //tests.Add(sample22);
            //tests.Add(sample23);
            //tests.Add(sample24);
            //tests.Add(sample25);
            //tests.Add(sample26);
            //tests.Add(sample27);

            //tests.Clear();
            //tests.Add(sample28);
        }


        private void FormHost_Paint(object sender, PaintEventArgs e)
        {
        }

        private void ExecuteTest(int testIndex)
        {
            Graphics g = CreateGraphics();
            g.Clear(Color.White);
            var ctx = new CanvasRenderingContext2D(g, null, new Pen(Color.Black, 1), new Fill(Color.Black), false);
            tests[testIndex](ctx);
            Text = "Test #" + (testIndex + 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (testIndex + 1 == tests.Count)
                testIndex = -1;
            ExecuteTest(++testIndex);
            Text = "Test #" + (testIndex + 1);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (testIndex == 0)
                testIndex = tests.Count;
            ExecuteTest(--testIndex);
            Text = "Test #" + (testIndex + 1);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var tg = new TestGenerator();
            Graphics g = CreateGraphics();
            TestHandler generatedTest = tg.GenerateTest((int) g.VisibleClipBounds.Width,
                                                        (int) g.VisibleClipBounds.Height);
            //start measure here
            generatedTest(g, new Pen(Color.Black, 1), new Fill(Color.Black));
            //end measure here and display results
        }

        #region Nested type: TestCase

        private delegate void TestCase(CanvasRenderingContext2D ctx);

        #endregion
    }
}