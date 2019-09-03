using System;
using System.Runtime.InteropServices;
using PrimaryInterOp.Cross3Krc;

namespace GetRobotData.Core.Internals
{
    class Cross3
    {
        private static T GetService<T>(string strServiceId, string strClientId)
        {
            try
            {
                var factory = new KrcServiceFactory();

                var service = factory.GetService(strServiceId, strClientId);
                if (service == null)
                {
                    throw new InvalidOperationException($"No Cross Interface. serviceId \"{strServiceId}\"");
                }
                Marshal.ReleaseComObject(factory);

                return (T)service;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetType() + ": " + ex.Message);
            }
        }

        private static ICKSyncVar2 _syncVar;

        public static ICKSyncVar2 SyncVar => _syncVar ?? (_syncVar = GetService<ICKSyncVar2>("WBC_KrcLib.SyncVar", "TestSyncVar"));
    }
}
