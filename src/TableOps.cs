using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PervasiveSQLToOracle
{
    public class TableOps : PervasiveToOracle 
    {
        /// <summary>
        /// Removes the quotes around parameters.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns></returns>
        public static string RemoveQuotesAroundParameters(string SQL)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            string[] strArr = SQL.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            char cQuote = (char)34;

            for (int i = 0; i < strArr.Length; i++)
            {
                string[] strSubArr = strArr[i].Trim().Split(new char[] { ' ' });
                for (int j = 0; j < strSubArr.Length; j++)
                {
                    //Remove the quotes
                    strSubArr[j] = strSubArr[j].Replace(cQuote.ToString(), " ");
                }
                strArr[i] = string.Join(" ", strSubArr);
                //replace two spaces with one space
                strArr[i] = strArr[i].Replace("  ", " ");
                strArr[i] = strArr[i].Replace(" ,", ",");

                //Break if we hit the BODY of a stored proc or function
                if (strArr[i].Contains("BEGIN")) break;
            }

            return string.Join("\r\n", strArr);
        }

        /// <summary>
        /// Removes the primary keys.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string RemovePrimaryKeys(string SQL, string tableName)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            ArrayList strArr = new ArrayList();
            strArr.AddRange(SQL.Split(delimiters, StringSplitOptions.RemoveEmptyEntries));

            for (int i = 0; i < strArr.Count - 1; i++)
            {
                if (strArr[i].ToString().Contains("PRIMARY KEY"))
                {
                    //wipe the line
                    strArr[i] = string.Empty;
                    //append the PrimaryKeyDefinition
                    strArr.Add(GetPrimaryKeyDefinition(tableName));
                    //remove ending comma from previous line
                    if (i > 0 && strArr[i - 1].ToString().Substring(strArr[i - 1].ToString().Length - 1, 1) == ",")
                    {
                        strArr[i - 1] = strArr[i - 1].ToString().Trim(new char[] { ',' });
                    }
                }
            }
            string[] stringArray = (string[])strArr.ToArray(typeof(string));
            return string.Join("\r\n", stringArray);
        }

        /// <summary>
        /// Gets the table space definition.
        /// </summary>
        /// <returns></returns>
        public static string GetTableSpaceDefinition()
        {
            string s = Environment.NewLine + "TABLESPACE CIMS_D" + Environment.NewLine;
            s += "PCTFREE 10" + Environment.NewLine;
            s += "INITRANS 2" + Environment.NewLine;
            s += "MAXTRANS 255" + Environment.NewLine;
            s += "STORAGE" + Environment.NewLine;
            s += "(" + Environment.NewLine;
            s += "    INITIAL 1M" + Environment.NewLine;
            s += "    NEXT 1M" + Environment.NewLine;
            s += "    MINEXTENTS 1" + Environment.NewLine;
            s += "    MAXEXTENTS UNLIMITED" + Environment.NewLine;
            s += "    PCTINCREASE 0" + Environment.NewLine;
            s += ");" + Environment.NewLine;
            return s;
        }

        /// <summary>
        /// Gets the primary key definition.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string GetPrimaryKeyDefinition(string tableName)
        {
            string s = "-- Create/Recreate primary, unique and foreign key constraints" + Environment.NewLine;
            s += "ALTER TABLE " + tableName + Environment.NewLine;
            s += "ADD CONSTRAINT " + tableName + "ID_PK PRIMARY KEY (" + tableName + "ID)" + Environment.NewLine;

            s += "-- Create primary key sequence" + Environment.NewLine;
            s += "DROP SEQUENCE " + tableName + "_Seq;" + Environment.NewLine;
            s += "CREATE SEQUENCE " + tableName + "_Seq MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;" + Environment.NewLine;
            s += "" + Environment.NewLine;
            s += "-- Create trigger for primary key" + Environment.NewLine;
            s += "CREATE OR REPLACE TRIGGER " + tableName + "_Insert_Trigger BEFORE INSERT ON " + tableName + " FOR EACH ROW" + Environment.NewLine;
            s += "BEGIN" + Environment.NewLine;
            s += "  SELECT " + tableName + "_Seq.nextval INTO :NEW." + tableName + "ID FROM dual;" + Environment.NewLine;
            s += "END;" + Environment.NewLine;

            return s;
        }
    }
}
