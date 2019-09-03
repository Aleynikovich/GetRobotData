using System;
using System.Linq;
using GetRobotData.Core.Internals;
using KukaRoboter.OnlineServicesFacade;
using Microsoft.Win32;

namespace GetRobotData.Core
{

    internal class Core
    {

        private static void Main()
        {

            ArchiveFacade a = new ArchiveFacade();
            a.ArchiveAll(@"D:\BackupAll.zip");

            using (var unzip = new Unzip(@"D:\BackupAll.zip"))
            {
                unzip.Extract("am.ini", "am.ini");
            }

            Console.WriteLine("Creando robot");

            KukaRobot k = new KukaRobot
            (
                trafoName: Cross3.SyncVar.ShowVar("$trafoname[]"),
                serialNumber: Convert.ToInt32(Cross3.SyncVar.ShowVar("$kr_serialno")),
                robotName: Cross3.SyncVar.ShowVar("$ROBNAME[]"),
                version: Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version", "Version", "Version not found")),
                robRunTime: Convert.ToInt32(Cross3.SyncVar.ShowVar("$robruntime")),
                techPacks: StringManipulation.GetBetween(System.IO.File.ReadAllText("am.ini"), "[TechPacks]"),
                loadData: Cross3.SyncVar.ShowVar($"LOAD_DATA[]")
            );
            
            Console.ReadKey();
        }
    }
}
