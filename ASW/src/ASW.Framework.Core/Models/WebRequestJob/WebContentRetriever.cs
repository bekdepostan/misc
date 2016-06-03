using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    [Serializable]
    public class WebContentRetriever
    {
        public string Expression { get; private set; }
        public Dictionary<string, string> RetrivedData { get; private set; } = null;
        public bool IsOptional { get; private set; } = false;

        public WebContentRetriever(string expression, bool isOptional = false)
        {
            Expression = expression;
            IsOptional = isOptional;
        }

        public bool Retrieve(string content)
        {
            var regex = new Regex(Expression, RegexOptions.Multiline);
            var m = regex.Match(content);
            if (m.Success)
            {
                RetrivedData = new Dictionary<string, string>();
                foreach (var gName in regex.GetGroupNames())
                {
                    RetrivedData[gName] = m.Groups[gName].Value;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}