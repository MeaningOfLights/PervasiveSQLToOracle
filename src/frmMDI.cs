using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PervasiveSQLToOracle
{
    public partial class frmMDI : Form
    {
        public frmMDI()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Create a new instance of the MDI child template form
            frmTableConvert chForm = new frmTableConvert();

            //Set parent form for the child window 
            chForm.MdiParent = this;

            //Display the child window
            chForm.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Create a new instance of the MDI child template form
            frmSprocConvert chForm = new frmSprocConvert();

            //Set parent form for the child window 
            chForm.MdiParent = this;

            //Display the child window
            chForm.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Create a new instance of the MDI child template form
            frmExtractValues chForm = new frmExtractValues();

            //Set parent form for the child window 
            chForm.MdiParent = this;

            //Display the child window
            chForm.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Create a new instance of the MDI child template form
            frmReadFiles chForm = new frmReadFiles();

            //Set parent form for the child window 
            chForm.MdiParent = this;

            //Display the child window
            chForm.Show();
        }
    }
}
