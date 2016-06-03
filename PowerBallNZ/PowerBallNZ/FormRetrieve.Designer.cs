namespace PowerBallNZ
{
    partial class FormRetrieve
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
            this.lblRegion = new System.Windows.Forms.Label();
            this.numFrom = new System.Windows.Forms.NumericUpDown();
            this.numTo = new System.Windows.Forms.NumericUpDown();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.staGeneral = new System.Windows.Forms.ToolStripStatusLabel();
            this.staCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.staDetail = new System.Windows.Forms.ToolStripStatusLabel();
            this.progRetrieve = new System.Windows.Forms.ToolStripProgressBar();
            this.txtMain = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTo)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(13, 13);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(41, 13);
            this.lblRegion.TabIndex = 0;
            this.lblRegion.Text = "Region";
            // 
            // numFrom
            // 
            this.numFrom.Location = new System.Drawing.Point(60, 11);
            this.numFrom.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrom.Name = "numFrom";
            this.numFrom.Size = new System.Drawing.Size(82, 20);
            this.numFrom.TabIndex = 1;
            this.numFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numTo
            // 
            this.numTo.Location = new System.Drawing.Point(148, 11);
            this.numTo.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numTo.Name = "numTo";
            this.numTo.Size = new System.Drawing.Size(82, 20);
            this.numTo.TabIndex = 2;
            this.numTo.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(236, 8);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(75, 23);
            this.btnRetrieve.TabIndex = 3;
            this.btnRetrieve.Text = "Retrieve";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staGeneral,
            this.staCount,
            this.staDetail,
            this.progRetrieve});
            this.statusStrip.Location = new System.Drawing.Point(0, 408);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(699, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // staGeneral
            // 
            this.staGeneral.Name = "staGeneral";
            this.staGeneral.Size = new System.Drawing.Size(62, 17);
            this.staGeneral.Text = "staGeneral";
            this.staGeneral.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // staCount
            // 
            this.staCount.Name = "staCount";
            this.staCount.Size = new System.Drawing.Size(55, 17);
            this.staCount.Text = "staCount";
            this.staCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // staDetail
            // 
            this.staDetail.Name = "staDetail";
            this.staDetail.Size = new System.Drawing.Size(52, 17);
            this.staDetail.Text = "staDetail";
            this.staDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progRetrieve
            // 
            this.progRetrieve.Name = "progRetrieve";
            this.progRetrieve.Size = new System.Drawing.Size(200, 16);
            this.progRetrieve.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // txtMain
            // 
            this.txtMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMain.Location = new System.Drawing.Point(0, 37);
            this.txtMain.Multiline = true;
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(699, 371);
            this.txtMain.TabIndex = 5;
            // 
            // FormRetrieve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 430);
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.numTo);
            this.Controls.Add(this.numFrom);
            this.Controls.Add(this.lblRegion);
            this.Name = "FormRetrieve";
            this.Text = "Retrieve From NZCity";
            this.Load += new System.EventHandler(this.FormRetrieve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTo)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.NumericUpDown numFrom;
        private System.Windows.Forms.NumericUpDown numTo;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel staGeneral;
        private System.Windows.Forms.ToolStripStatusLabel staCount;
        private System.Windows.Forms.ToolStripStatusLabel staDetail;
        private System.Windows.Forms.ToolStripProgressBar progRetrieve;
        private System.Windows.Forms.TextBox txtMain;
    }
}

