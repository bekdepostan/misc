using ASW.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Host.WinService
{
    public partial class AWSHostService : ServiceBase
    {
        public AWSHostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Info("ASW Host Start...");
            ASWHost.Init("orleansconfiguration.xml");
            ASWHost.Start();
        }

        protected override void OnStop()
        {
            ASWHost.Stop();
        }
    }
}
