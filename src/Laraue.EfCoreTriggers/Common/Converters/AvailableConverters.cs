using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;

namespace Laraue.EfCoreTriggers.Common.Converters
{
    public class AvailableConverters
    {
        public Stack<IMethodCallConverter> ExpressionCallConverters = new ();
    }
}