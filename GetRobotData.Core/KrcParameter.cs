using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace GetRobotData.Core
{
    public class KrcParameter
    {
        public string Name;
        public string Value;
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
            this.StartSearchLimit = endSearchLimit;
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
                return (string) Registry.GetValue(RegistryPath, Name, "not found");
            }

            else
            {
                return "No value found";
            }
        }
    }
}
