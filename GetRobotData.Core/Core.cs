using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetRobotData.Core
{
    class Core
    {
        static void Main(string[] args)
        {
            //KrcFile mada = new KrcFile(@"C:\Users\XYZ\source\repos\GetRobotData\TestFiles\","$machine.dat");
            //KrcFile robcor = new KrcFile(@"C:\Users\XYZ\source\repos\GetRobotData\TestFiles\", "$robcor.dat");
            //KrcFile config = new KrcFile(@"C:\Users\XYZ\source\repos\GetRobotData\TestFiles\", "$config.dat");
            //KrcParameter trafoName = new KrcParameter("trafoName", mada, "$TRAFONAME[]=\"#", "\"");
            KrcParameter version = new KrcParameter(krcParameterName: "Version", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\Version");
            KrcParameter robruntime = new KrcParameter(krcParameterName:"RobotRuntime", registryPath: @"HKEY_LOCAL_MACHINE\SOFTWARE\KUKA Roboter GmbH\RobotData");
            Console.WriteLine(version.GetValue());
            Console.WriteLine(robruntime.GetValue());
            Console.ReadLine();
        }
    }
}
