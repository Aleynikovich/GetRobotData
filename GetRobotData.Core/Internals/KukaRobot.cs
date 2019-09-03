using System;

namespace GetRobotData.Core.Internals
{
    class KukaRobot
    {
        public string TrafoName;
        public int SerialNumber;
        public string RobotName;
        public string Version;
        public int RobRunTime;
        public string TechPacks;
        public string LoadData;

        public KukaRobot(string trafoName, int serialNumber, string robotName, string version, int robRunTime, string techPacks, string loadData)
        {
            TrafoName = trafoName;
            SerialNumber = serialNumber;
            RobotName = robotName;
            Version = version;
            RobRunTime = robRunTime;
            TechPacks = techPacks;
            LoadData = loadData;

            PrintProperties();
        }

        public void PrintProperties()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                Console.WriteLine(prop.Name + ": " + prop.GetValue(this, null));
            }

            foreach (var field in this.GetType().GetFields())
            {
                Console.WriteLine(field.Name + ": " + field.GetValue(this));
            }
        }
    }
}
