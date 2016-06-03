using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ASW.Framework.Core
{
    public interface IASWCenter : IGrainWithIntegerKey
    {
        Task AddTeam(Guid id);
        Task AddTeam(ITeam team);
        Task RemoveTeam(Guid id);

        Task AddCase(ApplicationCase appCase);

        Task<IList<string>> GetCaseIds(Guid teamId);

        Task<WebRequestJob> GetJob(string caseId, ASWJob currentJob);

        Task<string> GetCaseInfo();
        Task<string> GetTeamInfo();

        Task Report(string info);
    }
}
