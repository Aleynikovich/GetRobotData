using System;
using Libs;
using Microsoft.Win32;

namespace GetRobotData.Core
{
    public class KrcParameter
    {
        public string Name;
        public KrcFile SourceFile;
        public string StartSearchLimit, EndSearchLimit;
        public string RegistryPath;

        public KrcParameter
        (
            string krcParameterName,
            KrcFile sourceFile = null,
            string startSearchLimit = null,
            string endSearchLimit = null,
            string registryPath = null
        )
        {
            this.Name = krcParameterName;
            this.SourceFile = sourceFile;
            this.StartSearchLimit = startSearchLimit;
            this.EndSearchLimit = endSearchLimit;
            this.RegistryPath = registryPath;
        }

        public string GetValue()
        {
            if (SourceFile != null)
            {
                return Libs.StringManipulation.GetBetween(SourceFile.Read(), StartSearchLimit, EndSearchLimit);
            }

            else if (RegistryPath != null)
            {
                return Convert.ToString(Registry.GetValue(RegistryPath, Name, "not found")) ;
            }

            else
            { 
                return "ERROR: No source has been defined for " + this.Name;
            }
        }
    }
}
