﻿using System;
using System.IO;
using GetRobotData.Core.Internals;
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
            //KrcFile robcor = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$robcor.dat");
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
            if (File.Exists(robotName.GetValue() + ".zip"))
            {
                Console.WriteLine("Ya existe {0}, se renombra por {1}", robotName.GetValue() + ".zip", robotName.GetValue() + "Old.zip");
                File.Move(robotName.GetValue() + ".zip", robotName.GetValue()  + "Old.zip");
            }
            if (File.Exists(@"D:\BackupAll.zip"))
            {
                Console.WriteLine("Ya existe {0}, se renombra por {1}", @"D:\BackupAll.zip", @"D:\BackupAllOld.zip");
                File.Move(@"D:\BackupAll.zip", @"D:\BackupAllOld.zip");
            }

            ArchiveFacade archiver = new ArchiveFacade();
            archiver.ArchiveAll(robotName.GetValue() + ".zip");

            if (!File.Exists(robotName.GetValue() + ".zip"))
            {
                if (File.Exists(@"D:\BackupAll.zip"))
                {
                    File.Move(@"D:\BackupAll.zip", robotName.GetValue() + ".zip");
                }
            }

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
            using (StreamWriter w = File.AppendText("am.ini"))
            {
                w.WriteLine("endoffile");
            }
            KrcFile am = new KrcFile(null, "am.ini");

            KrcParameter techPacks = new KrcParameter("techPacks", am , "[TechPacks]", "endoffile");

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Limpiando archivos temporales...");
            if (File.Exists("am.ini"))
            {
                File.Delete("am.ini");
            }
            if (File.Exists(serialNumber.GetValue() + @".cal"))
            {
                File.Delete(serialNumber.GetValue() + @".cal");
            }


            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Creando directorio " + serialNumber.GetValue() + " en la raiz...");
            System.IO.Directory.CreateDirectory(@"..\" + serialNumber.GetValue());

            Console.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm: ") + "Moviendo backup al directorio creado...");
            if (File.Exists(@"..\" + serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip"))
            {
                Console.Write(" AVISO: Backup existente, sobreescribiendo...");
                File.Delete(@"..\" + serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip");
            }
            File.Move(robotName.GetValue() + ".zip", @"..\" +  serialNumber.GetValue() + @"\" + robotName.GetValue() + ".zip");


            #region WriteAllToCsv



            #endregion

            #region Debugging 

            Console.WriteLine(techPacks.GetValue());
            Console.WriteLine(version.GetValue());
            Console.WriteLine(serialNumber.GetValue());
            Console.WriteLine(robotName.GetValue());
            Console.WriteLine(trafoName.GetValue());
            Console.WriteLine(robRunTime.GetValue());
            Console.WriteLine(loadData.GetValue());

            Console.ReadLine();

            #endregion

        }
    }
}
