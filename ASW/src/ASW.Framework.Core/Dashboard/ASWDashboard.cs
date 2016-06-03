using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class ASWDashboard
    {
        private readonly object _locker = new object();
        private bool _localMode = false;

        private IASWCenter _center = null;

        public ASWDashboard(IASWCenter center)
        {
            _localMode = true;
            _center = center;
        }
        public ASWDashboard(string configFile)
        {
            _localMode = false;
            GrainClient.Initialize(configFile);
            _center = GrainClient.GrainFactory.GetGrain<IASWCenter>(0);
        }

        public Task<string> GetCaseInfo()
        {
            lock (_locker)
            {
                return _center.GetCaseInfo();
            }
        }
    }
}
