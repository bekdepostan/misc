using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;

namespace ASW.Framework.Simple
{
    [Serializable]
    public class WebRequestJob : ASWJob
    {
        public enum Methods { POST, GET }
        public enum WebRequestJobType { Retrieving, Attampt }
        public WebRequestJob(string groupId, string code) : base(groupId, code)
        {
            AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
        }

        public WebRequestJobType JobType { get; set; } = WebRequestJobType.Attampt;
        public string Url { get; set; }
        public Methods Method { get; set; } = Methods.POST;
        public NameValueCollection Headers { get; private set; } = new NameValueCollection();
        public NameValueCollection Data { get; private set; } = new NameValueCollection();
        public WebContentRetrievingProfile RetrievingProfile { get; set; }
        public WebContentMatchingProfile SuccessProfile { get; set; }
        public WebContentMatchingProfile FailProfile { get; set; }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }

        public void AddData(string name, string value)
        {
            Data.Add(name, value);
        }
    }
}
