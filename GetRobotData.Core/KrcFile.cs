using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetRobotData.Core
{
    public class KrcFile
    {
        public string Path;
        public string Name;

        public KrcFile(string krcFilepath, string krcFileName)
        {
            this.Path = krcFilepath;
            this.Name = krcFileName;
        }
    }

    public class KrcParameter
    {
        public string Name;
        public string Value;


    }
}
