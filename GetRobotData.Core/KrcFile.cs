using System;
using System.Collections.Generic;
using System.IO;
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

        public string Read()
        {
            return File.ReadAllText(this.Path + this.Name);
        }
    }

}
