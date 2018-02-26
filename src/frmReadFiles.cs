using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PervasiveSQLToOracle
{
    public partial class frmReadFiles : Form
    {
        public frmReadFiles()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgRes =  dlg.ShowDialog();

            if (dlgRes == DialogResult.Cancel) return;

            txtFolderPath.Text = dlg.SelectedPath;
        }
    
        private string ReadFile(string fileName)
        {
            StreamReader s = File.OpenText(fileName);
            string read = s.ReadToEnd();
            string fileContents = read.ToString();
            s.Close();
            return fileContents;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(txtFolderPath.Text);

            FileInfo[] files = dir.GetFiles("*." + txtExtFilter.Text);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbDropProcs = new StringBuilder();
            foreach (FileInfo fileinfo in files)
            {
                string fileContents = ReadFile(fileinfo.FullName);
                sbDropProcs.Append("DROP PROCEDURE " + GetProcedureName(fileContents) +  ";" + Environment.NewLine);
                sb.Append(fileContents);
                sb.Append(Environment.NewLine +   "----------------------------" + Environment.NewLine);
            }
            txtOutput.Text = sbDropProcs.ToString();
            txtOutput.Text = txtOutput.Text + Environment.NewLine + Environment.NewLine +   "----------------------------" + Environment.NewLine + sb.ToString();
        }

        private string GetProcedureName(string sproc)
        {
            string storedProcName = string.Empty;
            int posOfCreate = sproc.ToUpper().IndexOf("CREATE");
            if (posOfCreate > 0 || sproc.ToUpper().Contains("CREATE"))
            {
                string signatureline = sproc.Replace("\"", string.Empty);
                //Remove the CREATE word
                signatureline = signatureline.Substring(0, posOfCreate).Trim() + " " + signatureline.Substring(posOfCreate + 6).Trim();
                signatureline = signatureline.Substring(0, signatureline.IndexOf("\r\n"));
                storedProcName = signatureline.Replace("PROCEDURE", string.Empty);
                storedProcName = storedProcName.Replace("(", string.Empty).Trim();
            }
            return storedProcName;
        }

    }
}
