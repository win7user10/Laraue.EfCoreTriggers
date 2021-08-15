using System.Collections.Generic;
using Laraue.EfCoreTriggers.Converters.MethodCall;

namespace Laraue.EfCoreTriggers.Converters
{
    public class AvailableConverters
    {
        public Stack<IMethodCallConverter> ExpressionCallConverters = new ();
    }
}