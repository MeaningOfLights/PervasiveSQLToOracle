using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PervasiveSQLToOracle
{
    public class PervasiveToOracle
    {
        /// <summary>
        /// Splits multiline text into a string array.
        /// </summary>
        /// <param name="multiLineText">The multi line text.</param>
        /// <returns></returns>
        public static string[] SplitMultiLines(string multiLineText)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            return multiLineText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Converts the package of stored procs.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns></returns>
        public static string ConvertPackageOfStoredProcs(string SQL)
        {
            StringBuilder sbOutput = new StringBuilder();

            //Split the SQL into the various sprocs that will make up this package
            //Remove the NoRowsUpdatedCheck
            SQL = string.Join(Environment.NewLine, RemoveLines(SplitMultiLines(SQL), "IF @@ROWCOUNT = 0 THEN", "END IF"));

            //Remove Double Spaces
            SQL = string.Join(Environment.NewLine,RemoveDoubleSpaces(SplitMultiLines(SQL)));

            if (!CheckValidInput(SQL))
            {
                return string.Empty ;
            }

            string[] sprocs = Regex.Split(SQL,"END");

            foreach (string individualSproc in sprocs)
            {
                //convert the data types
                string sproc = individualSproc;
                sproc = ConvertDataTypes(sproc);

                //split the sproc up into lines, delimited by line feeds
                string[] strArr = SplitMultiLines(sproc);

                int i = 0;
                //find the line containing CREATE
                foreach (string line in strArr)
                {
                    int posOfCreate = line.ToUpper().IndexOf("CREATE");
                    if (posOfCreate > 0 || line.ToUpper().Contains("CREATE"))
                    {
                        string signatureline = line.Replace("\"", string.Empty);
                        //Remove the CREATE word
                        signatureline = signatureline.Substring(0, posOfCreate).Trim() + " " + signatureline.Substring(posOfCreate + 6).Trim();
                        strArr[i] = signatureline.Trim();

                        string storedProcName = signatureline.Replace("PROCEDURE", string.Empty);
                        storedProcName = storedProcName.Replace("(", string.Empty).Trim();

                        //Call the methods designed for converting particular CRUD operation via sprocs
                        //Note: This solution is not smart enough for multiple CRUD ops in one SPROC (eg an UPDATE then a DELETE in one Sproc)
                        if (storedProcName.ToLower().Substring(0, 3) == "add")
                        {
                            sproc = SprocOps.convertInsertSproc(storedProcName, strArr);
                        }
                        else if (storedProcName.ToLower().Substring(0, 6) == "delete")
                        {
                            sproc = SprocOps.convertDeleteSproc(storedProcName, strArr);
                        }
                        else if (storedProcName.ToLower().Substring(0, 3) == "get")
                        {
                            sproc = SprocOps.convertSelectSproc(storedProcName, strArr);
                        }
                        else if (storedProcName.ToLower().Substring(0, 6) == "update")
                        {
                            sproc = SprocOps.convertUpdateSproc(storedProcName, strArr);
                        }
                        else
                        {
                            throw new Exception("Found line with CREATE but it doesn't contain a: add, update, insert or delete Procedure");
                        }

                        //Put in the IS statement
                        sproc = InsertISStatement(sproc);

                        //save the converted sproc in a stringbuilder
                        sbOutput.Append(sproc + Environment.NewLine + Environment.NewLine);

                        //Skip to converting next Sproc
                        break;
                    }
                    i++;
                }

            }
            return PolishSyntaxErrors(sbOutput.ToString());
        }


        public static bool CheckValidInput(string SQL)
        {
            bool valid = true;

           string[] strArr = SplitMultiLines(SQL);

           for (int i = 1; i < strArr.Length; i++)
           {
               //Check all Insert statements have input parameters seperated by a carriage return
               if (strArr[i].Contains("VALUES ("))
               {
                   
                   if (!strArr[i].Trim().EndsWith("("))
                   {
                       MessageBox.Show("Insert parameters must be on seperate lines:" + Environment.NewLine + Environment.NewLine + strArr[i]);
                       return false;
                   }
               }
           }


            return valid;
        }

        public static string PolishSyntaxErrors(string SQL)
        {
            SQL = SQL.Replace(";;", ";");
            //SQL = SQL.Replace(");", ")");
            return SQL;
        }

        public static string MigrateDataScript(string SQL, string TableName)
        {
            int posOfCreateTable = SQL.IndexOf("CREATE TABLE");
            string s = SQL.Substring(posOfCreateTable + 12);

            string[] strArr = SplitMultiLines(s);
            strArr[0] = string.Empty;
            strArr = SprocOps.RemoveInputParameterPrecision(strArr);
            int i = 1;
            for ( i=1 ; i < strArr.Length; i++)
            {
                string columnName = RemoveDataTypes(strArr[i]).Replace(",",string.Empty).Trim();
                if (strArr[i].Contains("NUMBER") || strArr[i].Contains("BOOLEAN") || strArr[i].Contains("IDENTITY"))
                {
                    strArr[i - 1] += "' + ";
                    strArr[i] = "IsNULL(CONVERT(" + columnName + ", SQL_VARCHAR),'NULL') + ',";
                }
                else if (strArr[i].Contains("DATE"))
                {
                    strArr[i - 1] += "' + ";
                    //strArr[i] = "to_date(" + columnName + " AS CHAR(19))  + ''', ";
                    strArr[i] = "'to_date(''' + CAST(COALESCE(" + columnName + ",'1900/01/01 12:00:00')" + " AS CHAR(19)) + ''',''yyyy/mm/dd HH24:MI:SS'')' + ',";
                }                     
                else if (strArr[i].Contains("PRIMARY KEY") || strArr[i].Contains(")"))
                {
                    strArr[i] = string.Empty;
                }
                else
                {
                    strArr[i - 1] += "''' + ";
                    strArr[i] = "IsNULL(REPLACE(" + columnName + ", '''', ''''''),'NULL')  + ''', ";
                }
            }

            strArr[0] = string.Empty;

            for (i = strArr.Length-1; i > 0; i--)
            {
                if (strArr[i].Length > 0 )
                {
                    strArr[i] = strArr[i].Replace(" + ''', ", string.Empty);
                    strArr[i] += " + ''');' AS VARCHAR(4000)) AS SQL";
                    strArr[i] += Environment.NewLine + "FROM " + (char)34 + TableName + (char)34;
                    break;
                }
            }

            s = "SELECT CAST('INSERT INTO " + TableName + " VALUES (' + " + Environment.NewLine;
            s += string.Join(Environment.NewLine, SplitMultiLines(string.Join(Environment.NewLine, strArr)));
            s = s.Replace("', + ''');' AS VARCHAR(4000))", "');' AS VARCHAR(4000))");
            return s;

        }




        private static string InsertISStatement(string sproc)
        {
            int posOfFristClosingBracket = sproc.IndexOf(")");

            sproc = sproc.Substring(0, posOfFristClosingBracket + 1) + " IS " + sproc.Substring(posOfFristClosingBracket + 1);

            sproc = sproc.Replace("IS ;", "IS ");
            
            return sproc;
        }

        private static string[] RemoveDoubleSpaces(string[] sprocSqlLines)
        {
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                do
                {
                    sprocSqlLines[i] = sprocSqlLines[i].Replace("\t", " "); 
                    sprocSqlLines[i] = sprocSqlLines[i].Replace("  ", " ");
                } while (sprocSqlLines[i].Contains("  "));
            }
            return sprocSqlLines;
        }

        /// <summary>
        /// Removes lines.
        /// </summary>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <param name="wordThatLineStartContains">The word that line start contains.</param>
        /// <param name="wordThatLineEndContains">The word that line end contains.</param>
        /// <returns></returns>
        public static string[] RemoveLines(string[] sprocSqlLines, string wordThatLineStartContains, string wordThatLineEndContains)
        {
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains(wordThatLineStartContains))
                {
                    while (!sprocSqlLines[i].Contains(wordThatLineEndContains))
                    {
                        sprocSqlLines[i] = string.Empty;
                        i++;
                    }
                    sprocSqlLines[i] = string.Empty;
                    break;
                }
            }
            //Remove emplty lines
            return PervasiveToOracle.SplitMultiLines(string.Join(Environment.NewLine, sprocSqlLines));
        }

        /// <summary>
        /// Drops the object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <returns></returns>
        public static string DropObject(enums.ObjectType objectType, string objectName)
        {
            string objectTypeName = string.Empty;
            if ((int)objectType == 0)
            {
                objectTypeName = objectType.ToString();
            }
            if (string.IsNullOrEmpty(objectTypeName)) throw new Exception("You must specify a valid Object Type to Drop");

            string s = "-- Create " + objectTypeName + Environment.NewLine;
            s += "DROP " + objectTypeName.ToUpper() + " "  + objectName + ";" + Environment.NewLine;
            return s;
        }


        public static string RemoveDataTypes(string SQL)
        {
            SQL = SQL.Replace(":", string.Empty);
            SQL = SQL.Replace("VARCHAR2", string.Empty);
            SQL = SQL.Replace("INTNUMBEREGER", string.Empty);
            SQL = SQL.Replace("NUMBER(9)",string.Empty );
            SQL = SQL.Replace("NUMBER", string.Empty);
            SQL = SQL.Replace(" DATE", string.Empty); //important to leave the space
            SQL = SQL.Replace("CASE", string.Empty);
            SQL = SQL.Replace("SYSDATE", string.Empty);
            SQL = SQL.Replace("NOT NULL", string.Empty);
            SQL = SQL.Replace("DECLARE", string.Empty);
            SQL = SQL.Replace("BOOLEAN", string.Empty);
            
            return SQL;
        }


        /// <summary>
        /// Converts the data types.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns></returns>
        public static  string ConvertDataTypes(string SQL)
        {
            SQL = SQL.Replace(":", "");
            SQL = SQL.Replace(" CHAR", " VARCH**2"); 
            SQL = SQL.Replace("VARCHAR", "VARCHAR2");
            SQL = SQL.Replace(" VARCH**2", "VARCH2"); 
            SQL = SQL.Replace("INTEGER", "NUMBER");
            SQL = SQL.Replace("IDENTITY DEFAULT '0'", "NUMBER(9) NOT NULL");
            SQL = SQL.Replace("DATETIME", "DATE");
            SQL = SQL.Replace("CASE", "");
            SQL = SQL.Replace("NOW()", "SYSDATE");
            SQL = SQL.Replace("Now()", "SYSDATE");
            SQL = SQL.Replace("DECLARE", "");
            
            //There are no Booleans in Oracle Tables, eg SQL = SQL.Replace("BIT", "BOOLEAN"); wont work 
            string[] strArr = SplitMultiLines(SQL);
            for (int i = 0; i < strArr.Length; i++)
            {

                //Remove the IN's unless its INSERT
                if (strArr[i].Length > 3 && strArr[i].Trim().Substring(0, 3) == "IN ")
                {
                    strArr[i] = strArr[i].Trim().Substring(3);
                }
                else if (strArr[i].Length > 4 && strArr[i].Trim().Substring(0, 4) == "(IN ")
                {
                    strArr[i] = "(" + strArr[i].Substring(4).Trim();
                }

                if (strArr[i].Contains("BIT"))
                {
                    string[] strSubArr = strArr[i].Trim().Split(new char[] { ' ' });
                    strArr[i] = strSubArr[0] + " NUMBER(1) NOT NULL check (" + strSubArr[0] + " in (1,0)),";
                }

            }

            return string.Join("\r\n", strArr);
        }

        /// <summary>
        /// Grants the CRUD access.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <returns></returns>
        public static string GrantCRUDAccess(string SQL, string objectName)
        {
            //append the access to the user accounts
            SQL += Environment.NewLine + "-- Give access to the user accounts" + Environment.NewLine;
            SQL += "GRANT SELECT, INSERT, UPDATE, DELETE ON " + objectName + " TO CIMS_User;";
            return SQL;
        }
    }
}
