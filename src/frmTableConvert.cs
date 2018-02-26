using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PervasiveSQLToOracle
{
    public partial class frmTableConvert : Form
    {
        public frmTableConvert()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            String SQL = txtIn.Text.ToUpper();
            string tableName = txtTableName.Text.ToUpper();
            //Check the user has entered the correct table name
            if (SQL.Contains(tableName) == false)
            {
                DialogResult dlgRes = MessageBox.Show("Did you enter the correct Table Name: " + txtTableName.Text + " ?"  + Environment.NewLine + "It doesn't exist in the Tabnle Definition script you pasted in!", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                //Exit if user needs to correct the table name
               if (dlgRes == DialogResult.No ) {return;}
            }

            
            //insert drop table
            SQL = PervasiveToOracle.DropObject(enums.ObjectType.table, tableName) + SQL;

            //convert datatypes
            SQL = PervasiveToOracle.ConvertDataTypes(SQL);

            //remove parameter quotes
            SQL = TableOps.RemoveQuotesAroundParameters(SQL);

            SQL = SQL + TableOps.GetTableSpaceDefinition();

            //remove primary keys
            SQL = TableOps.RemovePrimaryKeys(SQL, tableName);

            //Grant CRUD Access
            if (chkGrantAccess.Checked) SQL = PervasiveToOracle.GrantCRUDAccess(SQL, tableName);

            txtOut.Text = SQL;

            txtOut.SelectAll();


        }

    }
}
