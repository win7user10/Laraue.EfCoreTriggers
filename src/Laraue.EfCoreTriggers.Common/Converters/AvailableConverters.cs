using System.Collections.Generic;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;

namespace Laraue.EfCoreTriggers.Common.Converters
{
    /// <summary>
    /// Available converters for SQL generating.
    /// </summary>
    public class AvailableConverters
    {
        /// <summary>
        /// All applying method call converters when SQL generates. 
        /// </summary>
        public readonly Stack<IMethodCallConverter> ExpressionCallConverters = new ();
    }
}