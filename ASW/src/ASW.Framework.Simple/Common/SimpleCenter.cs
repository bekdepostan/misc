using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace ASW.Framework.Simple
{
    public partial class SimpleCenter : Grain, IASWCenter
    {
        private static object _locker = new object();
        private ASWContext _db = new ASWContext();
        private SortedList<string, ApplicationCase> _applicationCases = new SortedList<string, ApplicationCase>();
        private SortedList<Guid, ITeam> _teams = new SortedList<Guid, ITeam>();
        private SortedList<Guid, IList<string>> _teamsAndApps = new SortedList<Guid, IList<string>>();
        private SimpleCenterInfo _centerInfo;

        public SimpleCenter()
        {
            LoadApplicationCase();
            _centerInfo = new SimpleCenterInfo(_applicationCases, _teams, _teamsAndApps);
        }

        #region application / team add/remove
        private void LoadApplicationCase()
        {
            //load applications
            var apps = _db.ApplicationCases.ToList();
            foreach (var app in apps)
            {
                app.Card = _db.Cards.FirstOrDefault(x => x.Id.Equals(app.CardId));
                AddCase(app);
            }
        }

        public Task AddCase(ApplicationCase appCase)
        {
            if (appCase != null && !_applicationCases.ContainsKey(appCase.Username))
            {
                _applicationCases.Add(appCase.Id, appCase);
            }
            return TaskDone.Done;
        }

        public Task AddTeam(Guid id)
        {
            if (id != null && !_teams.ContainsKey(id))
            {
                _teams.Add(id, GrainFactory.GetGrain<ITeam>(id));
            }
            return ArrangeApplications();
        }

        public Task AddTeam(ITeam team)
        {
            var id = team?.GetId().Result;
            if (id != null && !_teams.ContainsKey(id.Value))
            {
                _teams.Add(id.Value, team);
            }
            return ArrangeApplications();
        }

        public Task RemoveTeam(Guid id)
        {
            if (id != null && _teams.ContainsKey(id))
            {
                _teams.Remove(id);
            }
            return ArrangeApplications();
        }
        #endregion

        private Task ArrangeApplications()
        {
            _teamsAndApps = new SortedList<Guid, IList<string>>();
            foreach (var t in _teams.Keys)
            {
                _teamsAndApps.Add(t, _applicationCases.Keys);
            }
            return TaskDone.Done;
        }

        public Task<IList<string>> GetCaseIds(Guid teamId)
        {
            IList<ApplicationCase> apps = new List<ApplicationCase>();

            if (!_teamsAndApps.ContainsKey(teamId))
            {
                return Task.FromResult<IList<string>>(null);
            }

            return Task.FromResult(_teamsAndApps[teamId]);
        }

        public Task<WebRequestJob> GetJob(string caseId, ASWJob currentJob)
        {
            if (!_applicationCases.ContainsKey(caseId))
            {
                return Task.FromResult<WebRequestJob>(null);
            }

            var appCase = _applicationCases[caseId];
            if (!appCase.Enabled)
            {
                return Task.FromResult<WebRequestJob>(null);
            }
            else if (appCase.SchemeType == ApplicationCase.SchemeTypes.WHVCN)
            {
                return Task.FromResult(WhvScheme.GetNextJob(appCase, currentJob, this));
            }
            else
            {
                return Task.FromResult<WebRequestJob>(null);
            }
        }

        public ApplicationCase UpdateApplication(ApplicationCase appCase)
        {
            lock (_locker)
            {
                _db.ApplicationCases.Attach(appCase);
                _db.Entry(appCase).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                _applicationCases[appCase.Id] = appCase;
                return appCase;
            }
        }

        public Task<string> GetCaseInfo()
        {
            if (_centerInfo != null)
            {
                var result = JsonConvert.SerializeObject(_centerInfo.CaseInfos);
                return Task.FromResult(result);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public Task<string> GetTeamInfo()
        {
            if (_centerInfo != null)
            {
                var result = JsonConvert.SerializeObject(_centerInfo.TeamInfos);
                return Task.FromResult(result);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public Task Report(string info)
        {
            if (_centerInfo != null)
            {
                _centerInfo.Report(info);
            }
            return TaskDone.Done;
        }
    }
}
