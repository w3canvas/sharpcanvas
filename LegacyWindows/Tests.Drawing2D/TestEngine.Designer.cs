namespace SharpCanvas.Tests.Forms
{
    partial class TestEngine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.trvTests = new System.Windows.Forms.TreeView();
            this.ctResults = new System.Windows.Forms.SplitContainer();
            this.pctOriginal = new System.Windows.Forms.PictureBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.StatusStrip();
            this.lblFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTestName = new System.Windows.Forms.ToolStripStatusLabel();
            this.ctResults.Panel2.SuspendLayout();
            this.ctResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctOriginal)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvTests
            // 
            this.trvTests.Location = new System.Drawing.Point(12, 12);
            this.trvTests.Name = "trvTests";
            this.trvTests.Size = new System.Drawing.Size(285, 558);
            this.trvTests.TabIndex = 0;
            this.trvTests.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvTests_NodeMouseClick);
            // 
            // ctResults
            // 
            this.ctResults.Location = new System.Drawing.Point(303, 33);
            this.ctResults.Name = "ctResults";
            // 
            // ctResults.Panel1
            // 
            this.ctResults.Panel1.AccessibleRole = System.Windows.Forms.AccessibleRole.Clock;
            // 
            // ctResults.Panel2
            // 
            this.ctResults.Panel2.Controls.Add(this.pctOriginal);
            this.ctResults.Size = new System.Drawing.Size(723, 537);
            this.ctResults.SplitterDistance = 377;
            this.ctResults.TabIndex = 1;
            // 
            // pctOriginal
            // 
            this.pctOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pctOriginal.Location = new System.Drawing.Point(0, 0);
            this.pctOriginal.Name = "pctOriginal";
            this.pctOriginal.Size = new System.Drawing.Size(342, 537);
            this.pctOriginal.TabIndex = 0;
            this.pctOriginal.TabStop = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(304, 14);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(61, 13);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "Test Result";
            // 
            // lblOriginal
            // 
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.Location = new System.Drawing.Point(684, 13);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(42, 13);
            this.lblOriginal.TabIndex = 3;
            this.lblOriginal.Text = "Original";
            // 
            // pnlStatus
            // 
            this.pnlStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTestName,
            this.lblFps});
            this.pnlStatus.Location = new System.Drawing.Point(0, 560);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1038, 22);
            this.pnlStatus.TabIndex = 4;
            this.pnlStatus.Text = "Status";
            // 
            // lblFps
            // 
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(26, 17);
            this.lblFps.Text = "fps:";
            // 
            // lblTestName
            // 
            this.lblTestName.Name = "lblTestName";
            this.lblTestName.Size = new System.Drawing.Size(42, 17);
            this.lblTestName.Text = "Name:";
            // 
            // TestEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 582);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.ctResults);
            this.Controls.Add(this.trvTests);
            this.Name = "TestEngine";
            this.Text = "TestEngine";
            this.ctResults.Panel2.ResumeLayout(false);
            this.ctResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctOriginal)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trvTests;
        private System.Windows.Forms.SplitContainer ctResults;
        private System.Windows.Forms.PictureBox pctOriginal;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.StatusStrip pnlStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblTestName;
        private System.Windows.Forms.ToolStripStatusLabel lblFps;
    }
}