using System;
using System.IO;
using GetRobotData.Core.Internals;
using JetBrains.Annotations;
using KukaRoboter.OnlineServicesFacade;
using Microsoft.Win32;

namespace GetRobotData.Core
{
    class KukaRobot
    {
        public string TrafoName;
        public int    SerialNumber;
        public string RobotName;
        public string Version;
        public int    RobRunTime;
        public string TechPacks;
        public string[] LoadData = new string[16];

        public KukaRobot()
        {
            ArchiveFacade a = new ArchiveFacade();
            if (File.Exists(@"D:\BackupAll.zip"))
            {
                if (File.Exists(@"D:\BackupAllOld.zip"))
                {
                    File.Delete(@"D:\BackupAllOld.zip");
                }

                File.Move(@"D:\BackupAll.zip", @"D:\BackupAllOld.zip");
            }

            a.ArchiveAll(@"D:\BackupAll.zip");

            using (var unzip = new Unzip(@"D:\BackupAll.zip"))
            {
                unzip.Extract("am.ini", "am.ini");
            }

            TrafoName    = Cross3.SyncVar.ShowVar("$trafoname[]");
            SerialNumber = Convert.ToInt32(Cross3.SyncVar.ShowVar("$kr_serialno"));
            RobotName    = Cross3.SyncVar.ShowVar("$ROBNAME[]");
            Version      = Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version", "Version", "Version not found"));
            RobRunTime   = Convert.ToInt32(Cross3.SyncVar.ShowVar("$robruntime"));
            TechPacks    = StringManipulation.GetBetween(File.ReadAllText("am.ini"), "[TechPacks]");

            try
            {
                for (var i = 1; i < 16; i++)
                {
                    if (Cross3.SyncVar.ShowVar($"LOAD_DATA[{i}].M") != Convert.ToString("-1.00000"))
                    {
                        this.LoadData[i] = $"TOOL {i}: " + Cross3.SyncVar.ShowVar($"LOAD_DATA[{i}]" + Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            //Directory.CreateDirectory($@"E:\{SerialNumber}");

            PrintProperties();
        }

        public void PrintProperties()
        {
            foreach (var prop in GetType().GetProperties())
            {
                Console.WriteLine("[{0}]\n{1}\n", prop.Name, prop.GetValue(this, null));
            }

            foreach (var field in GetType().GetFields())
            {
                Console.WriteLine("[{0}]\n{1}\n", field.Name, field.GetValue(this));
            }
        }
    }


    internal class Core
    {
        private static void Main()
        {
            KukaRobot roboter = new KukaRobot();
            Console.ReadLine();
        }
    }
}