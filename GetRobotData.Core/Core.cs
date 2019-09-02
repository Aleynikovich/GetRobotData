using System;
using System.IO;
using GetRobotData.Core.Internals;
using KukaRoboter.OnlineServicesFacade;


namespace GetRobotData.Core
{
    internal class Core
    {

        private static void Main()
        {

            #region FileParameters

            try
            {
                KrcFile mada = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$machine.dat");
                KrcParameter trafoName = new KrcParameter("trafoName", mada, "$TRAFONAME[]=\"#", "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                KrcFile robcor = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$robcor.dat");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                KrcFile config = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\System\", "$config.dat");
                KrcParameter loadData = new KrcParameter("loadData", config, "LOAD_DATA[16]", "{M -1.00000,CM {X 0.0,Y 0.0,Z 0.0,A 0.0,B 0.0,C 0.0},J {X 0.0,Y 0.0,Z 0.0}}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

           
            

            #endregion

            #region RegistryParameters

            //TODO: axisDifference

            KrcParameter version = new KrcParameter(krcParameterName: "Version", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version");
            KrcParameter robRunTime = new KrcParameter(krcParameterName: "RobotRuntime", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter robotName = new KrcParameter(krcParameterName: "Robotname", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter serialNumber = new KrcParameter(krcParameterName: "SerialNumber", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");

            #endregion


            //KRC .dll actions

            //Create backup
            //TODO: 1- Create folder with robot serial number and save the backup there. 2- Save RDC data method
            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Realizando backup...");
            ArchiveFacade archiver = new ArchiveFacade();
            archiver.ArchiveAll(robotName.GetValue() + ".zip", true);
            Console.Write(" Hecho! \n");
            Console.ReadLine();

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Extrayendo datos del backup...");
            using (var unzip = new Unzip(robotName.GetValue() + ".zip"))
            {
                unzip.Extract("am.ini", "am.ini");
                unzip.Extract(@"C\KRC\Roboter\Rdc\" + serialNumber.GetValue() + @".cal", serialNumber.GetValue() + @".cal");
            }

            Console.Write(" Hecho! \n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Abriendo archivo am.ini");
            using (StreamWriter w = File.AppendText("am.ini"))
            {
                w.WriteLine("endoffile");
            }
            Console.Write(" Hecho!\n");

            KrcFile am = new KrcFile(null, "am.ini");
            KrcParameter techPacks = new KrcParameter("techPacks", am , "[TechPacks]", "endoffile");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Limpiando archivos temporales...");
            System.IO.File.Delete("am.ini");
            Console.Write(" Hecho! \n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Creando directorio" + serialNumber.GetValue() + "en la raiz");
            System.IO.Directory.CreateDirectory(@"..\" + serialNumber.GetValue());
            Console.Write(" Hecho!\n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Moviendo backup al directorio creado");
            System.IO.File.Move(robotName.GetValue() + ".zip", @"..\" +  serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip");
            Console.Write(" Hecho! \n");


            #region WriteAllToCsv

            #endregion

            #region Debugging 

            Console.ReadLine();

            #endregion

        }
    }
}
