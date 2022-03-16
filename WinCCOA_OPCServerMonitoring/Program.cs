using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinCCOA_OPCServerMonitoring
{

    //Блять иди нахуй короче черт. Заебал сука. Хуже бабы. То да то нет. Иди в пизду короче.
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static OpcChecker _opcChecker;

        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            if (!Environment.UserInteractive)
            {
                _logger.Info("======= Запуск приложения в режиме службы =======");
                using (var service = new Service.WinCCOAOpcServerMonitoringService())
                    ServiceBase.Run(service);
            }
            else
            {
                _logger.Info("======= Запуск приложения в режиме десктопного приложения =======");
                _logger.Info("Нажмите <Q> для выхода");

                Start();
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q)
                    Stop();
            }
        }

        public static void Start()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            _opcChecker = new OpcChecker(_logger);
            _opcChecker.Start();
        }

        public static void Stop()
        {
            _logger.Info("Stopping application");
            _opcChecker.Stop();
            _opcChecker = null;
            _logger.Info("Stopping application finished");
        }
    }
}
