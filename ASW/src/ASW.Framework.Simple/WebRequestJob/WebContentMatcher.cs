using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Simple
{
    public enum ExpressionCombinationTypes { AND, OR }

    [Serializable]
    public class WebContentMatchingProfile
    {
        public ExpressionCombinationTypes CombinationType { get; set; }
        public IList<string> Expressions { get; private set; } = new List<string>();
        public WebContentMatchingProfile(string expression, ExpressionCombinationTypes type = ExpressionCombinationTypes.OR)
        {
            AddExpression(expression);
        }
        public void AddExpression(string expression)
        {
            Expressions.Add(expression);
        }
    }
}