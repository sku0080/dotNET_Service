using System.ServiceProcess;

namespace dotNET_Service
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceDotNet()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
