using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Converters.ExpressionCall;

namespace Laraue.EfCoreTriggers.Common.Converters
{
    public class AvailableConverters
    {
        public Stack<IExpressionCallConverter> ExpressionCallConverters = new ();
    }
}