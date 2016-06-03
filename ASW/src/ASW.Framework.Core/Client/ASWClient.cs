using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class ASWClient : IDisposable
    {
        private readonly object _locker = new object();
        private bool _hasInited = false;
        private bool _isRunning = false;
        private bool _localMode = false;

        private IASWCenter _center = null;
        private ITeam _team = null;
        private SortedList<string, IList<ASWWorker>> _workers = new SortedList<string, IList<ASWWorker>>();
        private const int THREAD_PER_CASE = 3;

        public ASWClient(Guid teamId, IASWCenter center)
        {
            _localMode = true;
            _center = center;
            if (_center == null)
            {
                return;
            }

            _team = new Team(teamId);
            _hasInited = true;

        }
        public ASWClient(Guid teamId, string configFile)
        {
            _localMode = false;
            GrainClient.Initialize(configFile);
            _center = GrainClient.GrainFactory.GetGrain<IASWCenter>(0);
            if (_center == null)
            {
                return;
            }

            _team = GrainClient.GrainFactory.GetGrain<ITeam>(teamId);
            if (_team == null || _team.GetPrimaryKey() == Guid.Empty)
            {
                return;
            }

            //Guider.Start();

            _hasInited = true;
        }

        public void Start()
        {
            lock (_locker)
            {
                if (_hasInited)
                {
                    _isRunning = true;
                    _team.SignIn(_center, _localMode);
                    var caseIds = _center.GetCaseIds(_team.GetId().Result).Result;
                    foreach (var cId in caseIds)
                    {
                        var workList = new List<ASWWorker>();
                        for (int i = 0; i < THREAD_PER_CASE; i++)
                        {
                            var w = new ASWWorker(cId, this);
                            w.Index = i;
                            workList.Add(w);
                            Task.Factory.StartNew(w.Start);
                        }

                        if (!_workers.ContainsKey(cId))
                        {
                            _workers.Add(cId, workList);
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            lock (_locker)
            {
                if (_hasInited)
                {
                    foreach (var lst in _workers.Values)
                    {
                        foreach(var w in lst)
                        {
                            w.Stop();
                        }
                    }
                    _team.SignOff();
                    _isRunning = false;
                }
            }
        }

        internal Task<WebRequestJob> GetJob(string caseId, ASWJob currentJob)
        {
            lock (_locker)
            {
                return _center.GetJob(caseId, currentJob);
            }
        }

        internal void Report(string caseId)
        {
            lock (_locker)
            {
                //Console.WriteLine("update " + caseId);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
