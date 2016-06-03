using ASW.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Simple
{
    #region info class
    [Serializable]
    public class CaseInfo
    {
        public CaseInfo(ApplicationCase appCase)
        {
            Id = appCase.Id;
            Username = appCase.Username;
            AppId = appCase.AppId;
            State = appCase.State;
            Scheme = appCase.SchemeType;
            Card = appCase.CardId;
            Enabled = appCase.Enabled;
            UpdatedTime = appCase.UpdatedTime.ToString("HH:mm:ss DD MMM");
        }
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string AppId { get; private set; }
        public string State { get; private set; }
        public string Scheme { get; private set; }
        public string Card { get; private set; }
        public bool Enabled { get; private set; }
        public string UpdatedTime { get; private set; }
    }
    [Serializable]
    public class TeamInfo
    {
        public TeamInfo(ITeam team)
        {
            Id = team.GetId().ToString();
            Name = team.GetName().Result;
        }
        public string Id { get; private set; }
        public string Name { get; private set; }
    }
    #endregion
    [Serializable]
    public class SimpleCenterInfo
    {
        private SortedList<string, ApplicationCase> _applicationCases = new SortedList<string, ApplicationCase>();
        private SortedList<Guid, ITeam> _teams = new SortedList<Guid, ITeam>();
        private SortedList<Guid, IList<string>> _teamsAndApps = new SortedList<Guid, IList<string>>();

        public SimpleCenterInfo(SortedList<string, ApplicationCase> appCases, SortedList<Guid, ITeam> teams,
            SortedList<Guid, IList<string>> teamsAndApps)
        {
            _applicationCases = appCases;
            _teams = teams;
            _teamsAndApps = teamsAndApps;
        }

        public IList<CaseInfo> CaseInfos
        {
            get
            {
                var infos = new List<CaseInfo>();
                if (_applicationCases != null)
                {
                    foreach (var c in _applicationCases.Values)
                    {
                        infos.Add(new CaseInfo(c));
                    }
                }
                return infos;
            }
        }

        public IList<TeamInfo> TeamInfos
        {
            get
            {
                var infos = new List<TeamInfo>();
                if (_teams != null)
                {
                    foreach (var t in _teams.Values)
                    {
                        var tInfo = new TeamInfo(t);
                        infos.Add(tInfo);
                    }
                }

                return infos;
            }
        }

        public void Report(string info)
        {

        }
    }
}
