namespace PervasiveSQLToOracle
{
    partial class frmTableConvert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableConvert));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtIn = new System.Windows.Forms.TextBox();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.chkGrantAccess = new System.Windows.Forms.CheckBox();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
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
            this.splitContainer1.TabIndex = 1;
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
            // txtOut
            // 
            this.txtOut.Location = new System.Drawing.Point(3, 3);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOut.Size = new System.Drawing.Size(419, 397);
            this.txtOut.TabIndex = 2;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(742, 441);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(110, 42);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Translate Table";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkGrantAccess
            // 
            this.chkGrantAccess.AutoSize = true;
            this.chkGrantAccess.Checked = true;
            this.chkGrantAccess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGrantAccess.Location = new System.Drawing.Point(15, 436);
            this.chkGrantAccess.Name = "chkGrantAccess";
            this.chkGrantAccess.Size = new System.Drawing.Size(95, 17);
            this.chkGrantAccess.TabIndex = 3;
            this.chkGrantAccess.Text = "CRUD Access";
            this.chkGrantAccess.UseVisualStyleBackColor = true;
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(87, 470);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(193, 20);
            this.txtTableName.TabIndex = 4;
            this.txtTableName.Text = "TargetSystem";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 470);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Table Name:";
            // 
            // frmTableConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 502);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.chkGrantAccess);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableConvert";
            this.Text = "Pervasive Table To Oracle";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtIn;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox chkGrantAccess;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label label1;
    }
}

