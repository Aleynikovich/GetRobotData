using System;
using System.IO;
using GetRobotData.Core.Internals;
using KukaRoboter.BackupManagerService.Configuration;
using KukaRoboter.BackupManagerService.ErrorHandling;
using KukaRoboter.BackupManagerService.Implementation;
using KukaRoboter.BackupManagerService.Interfaces;
using KukaRoboter.OnlineServicesFacade;
using KukaRoboter.OnlineServicesFacade.Extensions;


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

            //TODO: axisDifference

            KrcParameter version = new KrcParameter(krcParameterName: "Version", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version");
            KrcParameter robRunTime = new KrcParameter(krcParameterName: "RobotRuntime", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter robotName = new KrcParameter(krcParameterName: "Robotname", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter serialNumber = new KrcParameter(krcParameterName: "SerialNumber", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter loadData = new KrcParameter("loadData", config, "LOAD_DATA[16]", "{M -1.00000,CM {X 0.0,Y 0.0,Z 0.0,A 0.0,B 0.0,C 0.0},J {X 0.0,Y 0.0,Z 0.0}}");
            KrcParameter trafoName = new KrcParameter("trafoName", mada, "$TRAFONAME[]=\"#", "\"");

            #endregion


            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Realizando backup...");
            ArchiveFacade archiver = new ArchiveFacade();
            archiver.ArchiveAll(robotName.GetValue() + ".zip");
            Console.Write(" Hecho! \n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Extrayendo datos del backup...");
            using (var unzip = new Unzip(robotName.GetValue() + ".zip"))
            {
                unzip.Extract("am.ini", "am.ini");
                try
                {
                    bool isRdcDataPresent = false;
                    foreach (var fileName in unzip.FileNames)
                    {
                        Console.WriteLine(fileName);
                        System.Threading.Thread.Sleep(5);
                        if (fileName == "C/KRC/Roboter/Rdc/" + serialNumber.GetValue() + @".cal")
                        {
                            isRdcDataPresent = true;
                        }
                    }

                    if (isRdcDataPresent)
                    {
                        unzip.Extract(@"C\KRC\Roboter\Rdc\" + serialNumber.GetValue() + @".cal", serialNumber.GetValue() + @".cal");
                        KrcFile calFile = new KrcFile(null, serialNumber.GetValue() + @".cal");
                        KrcParameter axis1Diff = new KrcParameter("axis1Diff", calFile, "[CalibrationDifference]\nAxis1=", "Axis2");
                    }
                    else
                    {
                        Console.WriteLine(" Datos de RDC no presentes en el backup!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }

            Console.Write(" Hecho! \n");

            using (StreamWriter w = File.AppendText("am.ini"))
            {
                w.WriteLine("endoffile");
            }

            KrcFile am = new KrcFile(null, "am.ini");
            KrcParameter techPacks = new KrcParameter("techPacks", am , "[TechPacks]", "endoffile");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Limpiando archivos temporales...");
            System.IO.File.Delete("am.ini");
            Console.Write(" Hecho! \n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Creando directorio " + serialNumber.GetValue() + " en la raiz...");
            System.IO.Directory.CreateDirectory(@"..\" + serialNumber.GetValue());
            Console.Write(" Hecho!\n");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Moviendo backup al directorio creado...");
            if (File.Exists(@"..\" + serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip"))
            {
                Console.Write(" ...Backup existente, sobreescribiendo...");
                File.Delete(@"..\" + serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip");
            }

            File.Move(robotName.GetValue() + ".zip", @"..\" +  serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip");
            Console.Write(" Hecho! \n");


            #region WriteAllToCsv



            #endregion

            #region Debugging 

            Console.ReadLine();

            #endregion

        }
    }
}
