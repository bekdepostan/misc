using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public enum JobResults { None, Succeed, Failed, Excepted }
    public enum JobTerminationTypes { RepeatTillSucceedOrExcepted, RepeatTillExceed }

    public interface IASWJob : IGrainWithIntegerKey
    {
        Task<string> GetCode();
        Task SetCode(string code);

        Task<string> GetGroupId();
        Task SetGroupId(string groupId);

        Task<JobTerminationTypes> GetTerminationType();
        Task SetTerminationType(JobTerminationTypes type);

        Task<TimeSpan> GetRepeatInterval();
        Task SetRepeatInterval(TimeSpan interval);

        Task<int> GetRepeatExceed();
        Task SetRepeatExceed(int exceed);
    }
}
