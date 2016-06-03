using Orleans;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public enum JobTypes { Normal, Finish }
    public enum JobTerminationTypes { RepeatTillSucceedOrExcepted, RepeatTillExcess }
    public enum JobResults { None, Succeed, Failed, RepeatNeeded, Excepted }
    [Serializable]
    public abstract class ASWJob
    {
        public string Name { get; protected set; }
        public string Code { get; set; }
        public JobTypes JobType { get; set; } = JobTypes.Normal;
        public JobResults JobResult { get; set; } = JobResults.None;

        public JobTerminationTypes TerminationType { get; set; } = JobTerminationTypes.RepeatTillSucceedOrExcepted;
        public TimeSpan RepeatInterval { get; set; } = new TimeSpan(0, 0,0);
        public int RepeatExceed { get; set; } = 10;

        public ASWJob(string name, string code)
        {
            Name = name;
            Code = code;
        }

        #region parameter
        public Dictionary<string, string> Parameters { get; protected set; } = new Dictionary<string, string>();
        public void SetParameter(Dictionary<string, string> parameters)
        {
            foreach (var p in parameters)
            {
                if (parameters.ContainsKey(p.Key))
                {
                    Parameters[p.Key] = p.Value;
                }
                else
                {
                    Parameters.Add(p.Key, p.Value);
                }
            }
        }
        public void SetParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (Parameters.ContainsKey(name))
            {
                Parameters[name] = value;
            }
            else
            {
                Parameters.Add(name, value);
            }
        }
        #endregion

        #region result data
        public Dictionary<string, string> JobResultData { get; protected set; } = new Dictionary<string, string>();
        public bool HasResultData
        {
            get
            {
                return JobResultData != null && JobResultData.Count > 0;
            }
        }

        public void AddResultData(string name, string value)
        {
            if (JobResultData == null || string.IsNullOrEmpty(name))
            {
                return;
            }

            JobResultData[name] = value;
        }
        public void AddResultData(Dictionary<string, string> data)
        {
            if (data == null)
            {
                return;
            }
            foreach (var d in data)
            {
                AddResultData(d.Key, d.Value);
            }
        }

        #endregion
    }
}