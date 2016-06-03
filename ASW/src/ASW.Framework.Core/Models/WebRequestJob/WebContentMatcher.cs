using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public enum ExpressionCombinationTypes { AND, OR }

    [Serializable]
    public class WebContentMatcher
    {
        public JobResults JobResult { get; private set; }
        private ExpressionCombinationTypes _combinationType = ExpressionCombinationTypes.OR;
        private List<string> _expressions = new List<string>();

        public WebContentMatcher(JobResults result, IList<string> expressions, 
                                    ExpressionCombinationTypes type = ExpressionCombinationTypes.AND)
        {
            JobResult = result;
            _expressions.AddRange(expressions);
            _combinationType = type;
        }

        public bool Match(string content)
        {
            bool matched = false;
            foreach (var exp in _expressions)
            {
                var regex = new Regex(exp, RegexOptions.Multiline);
                var m = regex.Match(content);
                matched = m.Success;
                if (_combinationType == ExpressionCombinationTypes.AND && !matched)
                {
                    break;
                }
                else if(_combinationType == ExpressionCombinationTypes.OR && matched)
                {
                    break;
                }
            }

            return matched;
        }
    }
}