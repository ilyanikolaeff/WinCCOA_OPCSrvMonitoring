using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WinCCOA_OPCServerMonitoring.Service
{
    [RunInstaller(true)]
    public partial class AppAsServiceInstaller : System.Configuration.Install.Installer
    {
        public AppAsServiceInstaller()
        {
            InitializeComponent();

            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            serviceInstaller.DisplayName = "WinCCOA_OpcServerStatusMonitoring";
            serviceInstaller.ServiceName = "WinCCOA_OpcServerStatusMonitoring";
            serviceInstaller.Description = "Service for monitoring status OPC server and write result to WinCCOA";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
