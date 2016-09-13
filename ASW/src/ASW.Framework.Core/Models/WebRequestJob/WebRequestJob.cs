using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;
using System.Net;
using System.Web;

namespace ASW.Framework.Core
{
    [Serializable]
    public sealed class WebRequestJob : ASWJob
    {
        #region static
        public static readonly WebRequestJob EmptyJob = new WebRequestJob(null, null);
        #endregion

        //constant
        public WebRequestJob(string name, string code) : base(name, code)
        {
            Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0");
            Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            Headers.Add("Accept-Language", "en-US,en;q=0.5");
            Headers.Add("Accept-Encoding", "gzip, deflate");
        }

        public string BaseUrl { get; set; }
        public string Resource { get; set; }
        public RestSharp.Method Method { get; set; } = RestSharp.Method.POST;

        public JobResults Do(string content)
        {
            ResponseContent = content;

            if (!HasRetriever && !HasMatcher)
            {
                Logger.Info("no retriever and matcher");
                Logger.Info(content);
                return JobResults.Excepted;
            }

            if (HasRetriever)
            {
                foreach (var r in Retrievers)
                {
                    if (!r.Retrieve(content) && !r.IsOptional)
                    {
                        Logger.Info("can not retrieve");
                        Logger.Info(content);
                        return JobResults.Failed;
                    }

                    AddResultData(r.RetrivedData);
                }

                if (!HasMatcher)
                {
                    return JobResults.Succeed;
                }
            }

            if (HasMatcher)
            {
                foreach (var m in Matchers)
                {
                    if (m.Match(content))
                    {
                        return m.JobResult;
                    }
                }
            }

            Logger.Error(content);
            return JobResults.Excepted;
        }

        #region header
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }
        #endregion

        #region response content
        public string ResponseContent { get; private set; }
        public bool HasResponseContent
        {
            get
            {
                return !string.IsNullOrEmpty(ResponseContent);
            }
        }
        #endregion

        #region Retriever
        public IList<WebContentRetriever> Retrievers { get; private set; }
        public bool HasRetrievedData
        {
            get
            {
                return HasResultData;
            }
        }
        public Dictionary<string, string> RetrievedData
        {
            get
            {
                return JobResultData;
            }
        }
        public bool HasRetriever
        {
            get
            {
                return Retrievers != null && Retrievers.Count > 0;
            }
        }
        public void AddRetriever(WebContentRetriever retriever)
        {
            if (Retrievers == null)
            {
                Retrievers = new List<WebContentRetriever>();
            }

            Retrievers.Add(retriever);
        }
        #endregion

        #region Matcher
        public IList<WebContentMatcher> Matchers { get; private set; }
        public bool HasMatcher
        {
            get
            {
                return Matchers != null && Matchers.Count > 0;
            }
        }
        public void AddMatcher(WebContentMatcher matcher)
        {
            if (Matchers == null)
            {
                Matchers = new List<WebContentMatcher>();
            }

            Matchers.Add(matcher);
        }
        #endregion
    }
}
