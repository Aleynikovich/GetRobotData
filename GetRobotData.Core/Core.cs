using System;
using GetRobotData.Core.Internals;
using KukaRoboter.OnlineServicesFacade;
using Libs;


namespace GetRobotData.Core
{
    internal class Core
    {
        private static void Main()
        {

            #region Files


            KrcFile mada = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$machine.dat");
            KrcFile robcor = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$robcor.dat");
            KrcFile config = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\System\", "$config.dat");

            #endregion

            #region Parameters

            //TODO: Find Software options, loadData, axisDifference

            KrcParameter trafoName = new KrcParameter("trafoName", mada, "$TRAFONAME[]=\"#", "\"");


            KrcParameter version = new KrcParameter(krcParameterName: "Version", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version");
            KrcParameter robRunTime = new KrcParameter(krcParameterName: "RobotRuntime", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter robotName = new KrcParameter(krcParameterName: "Robotname", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter serialNumber = new KrcParameter(krcParameterName: "SerialNumber", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");

            #endregion


            //KRC .dll actions

            //Create backup
            //TODO: Create folder with robot serial number and save the backup there.
            Console.WriteLine("Realizando backup...");
            ArchiveFacade archiver = new ArchiveFacade();
            archiver.ArchiveAll(robotName.GetValue() + ".zip", true);
            Console.WriteLine("Backup finalizado!");
            Console.ReadLine();

            Console.WriteLine("Extrayendo datos del backup...");
            using (var unzip = new Unzip(robotName.GetValue() + ".zip"))
            {
                unzip.Extract("am.ini", "am.ini");
            }
            Console.WriteLine("OK!");


            #region Debugging 

            Console.WriteLine(version.GetValue());
            Console.WriteLine(trafoName.GetValue());
            Console.WriteLine(Convert.ToInt32(robRunTime.GetValue()) / 60 + " h");
            Console.WriteLine(robotName.GetValue());
            Console.WriteLine(serialNumber.GetValue());
            Console.ReadLine();

            #endregion

        }
    }
}
