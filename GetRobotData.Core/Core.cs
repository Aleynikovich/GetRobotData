using System;
using System.IO;
using GetRobotData.Core.Internals;
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
        public string LoadData;
        private const string BackupDir = @"D:\BackupAll.zip";

        public KukaRobot()
        {
            ArchiveFacade a = new ArchiveFacade();

            if (File.Exists(BackupDir))
            {
                if (File.Exists(BackupDir + ".old"))
                {
                    File.Delete(BackupDir + ".old");
                }

                File.Move(BackupDir, BackupDir + ".old");
            }

            try
            {
                a.ArchiveAll(BackupDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Fallo al crear backup");
                throw;
            }

            try
            {
                using (var unzip = new Unzip(BackupDir))
                {
                    unzip.Extract("am.ini", "am.ini");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Fallo al extraer backup");
                throw;
            }

            try
            {
                TrafoName    = Cross3.SyncVar.ShowVar("$trafoname[]");
                SerialNumber = Convert.ToInt32(Cross3.SyncVar.ShowVar("$kr_serialno"));
                RobotName    = Cross3.SyncVar.ShowVar("$ROBNAME[]");
                Version      = Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version", "Version", "Version not found"));
                RobRunTime   = Convert.ToInt32(Cross3.SyncVar.ShowVar("$robruntime"));
                TechPacks    = StringManipulation.GetBetween(File.ReadAllText("am.ini"), "[TechPacks]");

                for (var i = 1; i < 16; i++)
                {
                    if (Cross3.SyncVar.ShowVar($"LOAD_DATA[{i}].M") != Convert.ToString("-1.00000"))
                    {
                        LoadData += $"TOOL {i}: " + Cross3.SyncVar.ShowVar($"LOAD_DATA[{i}]" + Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error al asignar valores del robot, revisar que Cross3 este activo");
                throw;
            }

            try
            {
                Directory.CreateDirectory($@"E:\{SerialNumber}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Revisar que el USB esta montado en E:\\");
                throw;
            }

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