using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinCCOA_OPCServerMonitoring.Service
{
    partial class WinCCOAOpcServerMonitoringService : ServiceBase
    {
        public WinCCOAOpcServerMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Starting application as service");
            Program.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            Program.Stop();
        }
    }
}
