using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace PervasiveSQLToOracle
{
    public partial class frmSprocConvert : Form
    {
        public frmSprocConvert()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            txtOut.Text = PervasiveToOracle.ConvertPackageOfStoredProcs(txtIn.Text);
        }

    }
  
}
