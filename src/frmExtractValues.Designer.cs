namespace PervasiveSQLToOracle
{
    partial class frmExtractValues
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExtractValues));
            this.label1 = new System.Windows.Forms.Label();
            this.txtIn = new System.Windows.Forms.TextBox();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtPopulateScript = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 427);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Table Name:";
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
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(83, 427);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(193, 20);
            this.txtTableName.TabIndex = 10;
            this.txtTableName.Text = "TargetSystem";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(747, 427);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(110, 42);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Get INSERT Script";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(5, 7);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtIn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtPopulateScript);
            this.splitContainer1.Size = new System.Drawing.Size(857, 403);
            this.splitContainer1.SplitterDistance = 428;
            this.splitContainer1.TabIndex = 7;
            // 
            // txtPopulateScript
            // 
            this.txtPopulateScript.Location = new System.Drawing.Point(3, 3);
            this.txtPopulateScript.Multiline = true;
            this.txtPopulateScript.Name = "txtPopulateScript";
            this.txtPopulateScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPopulateScript.Size = new System.Drawing.Size(417, 397);
            this.txtPopulateScript.TabIndex = 13;
            // 
            // frmExtractValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 477);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmExtractValues";
            this.Text = "Extract Values Script";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIn;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtPopulateScript;
    }
}