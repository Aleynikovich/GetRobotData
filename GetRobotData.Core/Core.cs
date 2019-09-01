using System;


namespace GetRobotData.Core
{
    internal class Core
    {
        private static void Main()
        {

            
            KrcFile mada = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$machine.dat");
            KrcFile robcor = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\Mada\", "$robcor.dat");
            KrcFile config = new KrcFile(@"C:\KRC\ROBOTER\KRC\R1\System\", "$config.dat");


            KrcParameter trafoName = new KrcParameter("trafoName", mada, "$TRAFONAME[]=\"#", "\"");


            KrcParameter version = new KrcParameter(krcParameterName: "Version", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version");
            KrcParameter robRunTime = new KrcParameter(krcParameterName: "RobotRuntime", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter robotName = new KrcParameter(krcParameterName:"Robotname", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            KrcParameter serialNumber = new KrcParameter(krcParameterName: "SerialNumber", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");

            

            Console.WriteLine(version.GetValue());
            Console.WriteLine(trafoName.GetValue());
            Console.WriteLine(Convert.ToInt32(robRunTime.GetValue()) / 60 + " h");
            Console.WriteLine(robotName.GetValue());
            Console.WriteLine(serialNumber.GetValue());
            Console.ReadLine();
        }
    }
}
