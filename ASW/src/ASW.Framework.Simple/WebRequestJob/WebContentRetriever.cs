using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Simple
{
    [Serializable]
    public class WebContentRetrievingProfile
    {
        public IList<string> Expressions { get; private set; } = new List<string>();
        public WebContentRetrievingProfile(string expression)
        {
            AddExpression(expression);
        }

        public void AddExpression(string expression)
        {
            Expressions.Add(expression);
        }
    }

    public class WebContentRetriever
    {
        private WebContentRetrievingProfile _profile;
        private NameValueCollection _result;

        public WebContentRetriever(WebContentRetrievingProfile profile)
        {
            _profile = profile;
        }

        public NameValueCollection Result
        {
            get
            {
                if (_result == null)
                {
                    return Retrieve();
                }
                else
                {
                    return _result;
                }
            }
        }

        public NameValueCollection Retrieve()
        {
            _result = new NameValueCollection();
            foreach (var exp in _profile.Expressions)
            {
            }

            return _result;
        }
    }
}