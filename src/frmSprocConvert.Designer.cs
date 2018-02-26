namespace PervasiveSQLToOracle
{
    partial class frmSprocConvert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSprocConvert));
            this.btnGo = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.txtIn = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(757, 429);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(110, 42);
            this.btnGo.TabIndex = 7;
            this.btnGo.Text = "Translate Sproc";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtOut
            // 
            this.txtOut.Location = new System.Drawing.Point(3, 3);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOut.Size = new System.Drawing.Size(419, 397);
            this.txtOut.TabIndex = 2;
            // 
            // txtIn
            // 
            this.txtIn.Location = new System.Drawing.Point(3, 3);
            this.txtIn.Multiline = true;
            this.txtIn.Name = "txtIn";
            this.txtIn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIn.Size = new System.Drawing.Size(422, 397);
            this.txtIn.TabIndex = 1;
            this.txtIn.Text = resources.GetString("txtIn.Text");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(13, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtIn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtOut);
            this.splitContainer1.Size = new System.Drawing.Size(857, 403);
            this.splitContainer1.SplitterDistance = 428;
            this.splitContainer1.TabIndex = 6;
            // 
            // frmSprocConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 480);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmSprocConvert";
            this.Text = "Pervasive Stored Procedures To Oracle Packages ";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.TextBox txtIn;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}