using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class WebRequestJobAction
    {
        public string Name { get; private set; }
        public WebContentMatcher Matcher { get; private set; }
        public WebContentRetriever Retrievers { get; private set; }
        public JobResults JobResult { get; private set; } = JobResults.None;

        public WebRequestJobAction(string name, JobResults jobResult,
                                    IList<WebContentMatcher> matchers, IList<WebContentRetriever> retrivers)
        {
            Name = name;
            JobResult = jobResult;
            Matcher = matcher;
            Retrievers = retrivers;
        }

        public bool IsApplicable(string content)
        {
            return Matcher.Match(content);
        }

        public Dictionary<string, string> RetrievedData
        {
            get
            {
                return Retriever?.RetrivedData;
            }
        }
    }
}
