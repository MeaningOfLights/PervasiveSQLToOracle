using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PervasiveSQLToOracle
{
    public class SprocOps : PervasiveToOracle 
    {
        /// <summary>
        /// Converts insert sproc's.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        internal static string convertInsertSproc(string sprocName, string[] sprocSqlLines)
        {
            sprocSqlLines = RemoveInputParameterPrecision( sprocSqlLines);

            sprocSqlLines = MoveOutputParametersOutKeyword(sprocName, sprocSqlLines);

            sprocSqlLines = SwapLinesContaining(sprocSqlLines, "SYSDATE", "BEGIN");

            sprocSqlLines = AppendSprocNameToInsertValues(sprocName, sprocSqlLines);

            sprocSqlLines = ConvertReturnValues(sprocName, sprocSqlLines);

            string sprocConverted = string.Join("\r\n", sprocSqlLines);
            sprocConverted = VariableAssignmentUsesColons(sprocConverted);
            return sprocConverted + Environment.NewLine + "COMMIT;" + Environment.NewLine + "END;";
        }

        /// <summary>
        /// Converts delete sproc's.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        internal static string convertDeleteSproc(string sprocName, string[] sprocSqlLines)
        {
            sprocSqlLines = RemoveInputParameterPrecision( sprocSqlLines);

            sprocSqlLines = AppendSprocNameToWhereValue(sprocName, sprocSqlLines);

            string sprocConverted = string.Join("\r\n", sprocSqlLines);
            return sprocConverted + ";" + Environment.NewLine + "COMMIT;" + Environment.NewLine +  "END;";
        }

        /// <summary>
        /// Converts select sproc's.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        internal static string convertSelectSproc(string sprocName, string[] sprocSqlLines)
        {
            sprocSqlLines = RemoveInputParameterPrecision( sprocSqlLines);

            //Remove the Return outputs
            sprocSqlLines = RemoveLines(sprocSqlLines, "RETURNS",")");

            //Put in the RETURN cursor parameter
            sprocSqlLines = PutInReturnCursorParameter(sprocSqlLines);

            sprocSqlLines = AppendSprocNameToWhereValue(sprocName, sprocSqlLines);

            
            string sprocConverted = string.Join("\r\n", sprocSqlLines);

            sprocConverted = sprocConverted.Replace("BEGIN", "selectCursor tSelectCursor;" + Environment.NewLine + "BEGIN");
           
            sprocConverted = sprocConverted .Replace("BEGIN", "BEGIN" + Environment.NewLine + "OPEN selectCursor FOR");
            return sprocConverted + ";" + Environment.NewLine + "results := selectCursor;" + Environment.NewLine + "END;";
        }


        /// <summary>
        /// Converts update sproc's.
        /// </summary>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        internal static string convertUpdateSproc(string sprocName, string[] sprocSqlLines)
        {
            sprocSqlLines = RemoveInputParameterPrecision( sprocSqlLines);

            sprocSqlLines = MoveOutputParametersOutKeyword(sprocName, sprocSqlLines);

            sprocSqlLines = SwapLinesContaining(sprocSqlLines, "SYSDATE", "BEGIN");

            sprocSqlLines = AppendSprocNameToUpdateValues(sprocName, sprocSqlLines);
            
            sprocSqlLines = AppendNoRowsUpdatedCheck(sprocSqlLines);

            
            string sprocConverted = string.Join("\r\n", sprocSqlLines);
            sprocConverted = VariableAssignmentUsesColons(sprocConverted);
            return sprocConverted + Environment.NewLine + "PUT RETURN PARAMETERS HERE" + Environment.NewLine + "COMMIT;" + Environment.NewLine + "END;";
        }

        private static string VariableAssignmentUsesColons(string sproc)
        {
            sproc = sproc.Replace("DATE = SYSDATE","DATE := SYSDATE");
            return sproc;
        }
        /// <summary>
        /// Appends the cursor result parameter.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] AppendCursorResultParameter(string sprocName, string[] sprocSqlLines)
        {
            string sproc = string.Join(" ", sprocSqlLines);

            int posOfFirstClosingBracket = sproc.IndexOf(")");

            string resOutput = Environment.NewLine + ",results IN OUT tSelectCursor) IS" + Environment.NewLine + "selectCursor tSelectCursor;";
            sproc = sproc.Substring(0, posOfFirstClosingBracket) + resOutput + sproc.Substring(posOfFirstClosingBracket + 1);
            return SplitMultiLines(sproc);
        }

        /// <summary>
        /// Swaps lines containing the first val with the second val.
        /// </summary>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <param name="firstLine">The first line.</param>
        /// <param name="secondLine">The second line.</param>
        /// <returns></returns>
        private static string[] SwapLinesContaining(string[] sprocSqlLines, string firstLine, string secondLine)
        {
            int firstLineNumber = 0;
            int secondLineNumber = 0;
            firstLine = firstLine.ToUpper();
            secondLine = secondLine.ToUpper();

            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains(firstLine))
                {
                    firstLineNumber = i;
                }
                if (sprocSqlLines[i].ToUpper().Contains(secondLine))
                {
                    secondLineNumber = i;
                }
                if (firstLineNumber > 0 && secondLineNumber > 0) { break; }
            }

            if (firstLineNumber == 0 || secondLineNumber == 0) { return sprocSqlLines; }
            string tempLine = sprocSqlLines[firstLineNumber];
            sprocSqlLines[firstLineNumber] = sprocSqlLines[secondLineNumber];
            sprocSqlLines[secondLineNumber] = tempLine;
            return sprocSqlLines;

        }

        /// <summary>
        /// Appends the sproc name to insert values.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] AppendSprocNameToInsertValues(string sprocName, string[] sprocSqlLines)
        {
            bool InValuesSection = false;
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains("VALUES"))
                {
                    i += 1;
                    InValuesSection = true;
                }
                if (sprocSqlLines[i].ToUpper().Contains(")") && InValuesSection == true)
                {
                    //The end of the insert values sectio of the insert sproc
                    break;
                }
                if (InValuesSection && !sprocSqlLines[i].ToLower().Contains("currentdatetime"))
                {
                    sprocSqlLines[i] = sprocName + "." + sprocSqlLines[i].Trim();
                }

            }
            return sprocSqlLines;
        }

        /// <summary>
        /// Appends the sproc name to update values.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] AppendSprocNameToUpdateValues(string sprocName, string[] sprocSqlLines)
        {
            bool InValuesSection = false;
            for (int i = 1; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains("UPDATE") && !sprocSqlLines[i].ToUpper().Contains("--"))
                {
                    InValuesSection = true;
                }
                
                if (InValuesSection && sprocSqlLines[i].Contains("=") && !sprocSqlLines[i].ToLower().Contains("currentdatetime"))
                {
                    string[] strSubArr = sprocSqlLines[i].Trim().Split(new char[] { ' ' });
                    for (int j = 0; j < strSubArr.Length; j++)
                    {
                        if (strSubArr[j] == "=")
                        {
                            //insert the Sproc name before the Update Values, so its recognised as a input parameter
                            strSubArr[j + 1] = sprocName + "." + strSubArr[j + 1].Trim();
                            sprocSqlLines[i] = string.Join(" ", strSubArr);
                            break;
                        }
                    }
                }
                else if (sprocSqlLines[i].ToUpper().Contains("WHERE"))
                {
                    //Break when we reach the WHERE clause
                    break;
                }

            }
            return sprocSqlLines;
        }

         private static string[] PutInReturnCursorParameter(string[] sprocSqlLines)
        {
            //Put in the RETURN cursor parameter

            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains(")"))
                {
                    sprocSqlLines[i] = sprocSqlLines[i].Replace(")", string.Empty);
                    //if there is at least one parameter add in a comma to seperate the parameters
                    if (sprocSqlLines[i].Trim().Length > 1)
                    {
                        sprocSqlLines[i] = sprocSqlLines[i] + "," + Environment.NewLine + "results IN OUT tSelectCursor)";
                    }
                    else
                    {
                        sprocSqlLines[i] = "(" + "results IN OUT tSelectCursor)";
                    }
                    break;
                }
            }
            //put back into an array after adding the Environment.NewLine
            return SplitMultiLines(string.Join(Environment.NewLine, sprocSqlLines));
         }

        /// <summary>
        /// Appends the sproc name to where value.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] AppendSprocNameToWhereValue(string sprocName, string[] sprocSqlLines)
        {
            bool InValuesSection = false;
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains("WHERE"))
                {
                    InValuesSection = true;
                }
                if (sprocSqlLines[i].Contains("=") && InValuesSection == true)
                {
                    string[] strSubArr = sprocSqlLines[i].Trim().Split(new char[] { ' ' });
                    for (int j = 0; j < strSubArr.Length; j++)
                    {
                        if (strSubArr[j] == "=")
                        {
                            strSubArr[j + 1] = sprocName + "." + strSubArr[j + 1].Trim();
                            sprocSqlLines[i] = string.Join(" ", strSubArr);
                            //remove the trailing semi colon
                            sprocSqlLines[i] = sprocSqlLines[i].Replace(";",string.Empty );
                            break;
                        }
                    }
                }
            }
            return sprocSqlLines;
        }
        


        /// <summary>
        /// Converts the return values.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] ConvertReturnValues(string sprocName, string[] sprocSqlLines)
        {
            string outPutParameters = string.Empty;
            string outPutValues = string.Empty;

            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains("SET"))
                {
                    string[] strSubArr = sprocSqlLines[i].Trim().Split(new char[] { ' ' });
                    outPutParameters += strSubArr[1].Trim() + ",";
                    outPutValues += strSubArr[3].Trim().Replace(";", string.Empty) + ",";
                    sprocSqlLines[i] = string.Empty;
                }
            }
            if (outPutParameters.Length > 0)
            {
                outPutParameters = outPutParameters.Substring(0, outPutParameters.Length - 1);
                outPutValues = outPutValues.Substring(0, outPutValues.Length - 1);
                sprocSqlLines[sprocSqlLines.Length - 1] = "RETURNING " + outPutValues + Environment.NewLine + " INTO " + outPutParameters + ";";
            }
            //Remove empty lines
            return SplitMultiLines(string.Join(Environment.NewLine, sprocSqlLines));
        }



        /// <summary>
        /// Removes the input parameter precision.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        public static string[] RemoveInputParameterPrecision(string[] sprocSqlLines)
        {
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                bool endWithComma = false;
                if (sprocSqlLines[i].ToUpper().Contains("VARCHAR2"))
                {
                    //if (sprocSqlLines[i].Trim().EndsWith(",")) endWithComma = true;
                    int posOfvarchar = sprocSqlLines[i].ToUpper().IndexOf("VARCHAR2");
                    //Trim off the Varchar(precision)
                    int posOfClosingBracket = sprocSqlLines[i].ToUpper().IndexOf(")", posOfvarchar);
                    sprocSqlLines[i] = sprocSqlLines[i].Substring(0, posOfvarchar + 8) + sprocSqlLines[i].Substring(posOfClosingBracket + 1);
                }
                else if (sprocSqlLines[i].ToUpper().Contains("NUMBER(1)"))
                {
                    if (sprocSqlLines[i].Trim().EndsWith(",")) endWithComma = true;
                    string[] strSubArr = sprocSqlLines[i].Trim().Split(new char[] { ' ' });
                    sprocSqlLines[i] = strSubArr[0].Trim() + " BOOLEAN"; // +strSubArr[1].Trim();
                }
                if (endWithComma) sprocSqlLines[i] += ",";
            }

            return sprocSqlLines;
        }


        /// <summary>
        /// Moves the output parameters out keyword.
        /// </summary>
        /// <param name="sprocName">Name of the sproc.</param>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] MoveOutputParametersOutKeyword(string sprocName, string[] sprocSqlLines)
        {
            for (int i = 0; i < sprocSqlLines.Length; i++)
            {
                if (sprocSqlLines[i].ToUpper().Contains("OUT"))
                {
                    string[] strSubArr = sprocSqlLines[i].Trim().Split(new char[] { ' ' });
                    sprocSqlLines[i] = strSubArr[1].Trim() + " " + strSubArr[0].Trim();
                    for (int j = 2; j < strSubArr.Length; j++)
                    {
                        sprocSqlLines[i] += " " + strSubArr[j].Trim();
                    }
                    sprocSqlLines[i] = sprocSqlLines[i].Trim();
                }
            }
            return sprocSqlLines;
        }

        /// <summary>
        /// Appends the no rows updated check.
        /// </summary>
        /// <param name="sprocSqlLines">The sproc SQL lines.</param>
        /// <returns></returns>
        private static string[] AppendNoRowsUpdatedCheck(string[] sprocSqlLines)
        {
            string s = "-- If no rows updated, throw an error" + Environment.NewLine;
            s += "IF SQL%ROWCOUNT = 0 THEN" + Environment.NewLine;
            s += "raise_application_error(-20001, 'Row has been deleted or updated by another user');" + Environment.NewLine;
            s += "END IF;" + Environment.NewLine;
            int lnCount = sprocSqlLines.Length;
            sprocSqlLines[lnCount-1] += Environment.NewLine + s;
            return sprocSqlLines;
        }


    }
}
