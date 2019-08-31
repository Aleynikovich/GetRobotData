using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringManipulation
{

    public class StringManipulation
    { 
        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start, StringComparison.Ordinal);

                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "Limit strings not found";
            }
        }
    }
   
}
