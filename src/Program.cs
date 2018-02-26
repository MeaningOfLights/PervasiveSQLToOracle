/// <summary>
///Created By: Jeremy Thompson (p686873d)
///Created Date: 12/1/2010
///Description: 
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PervasiveSQLToOracle
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMDI ());
        }

        static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show("Global Exception Handler caught : " + e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace,Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

    }
}
