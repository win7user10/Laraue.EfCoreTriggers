using System;
using Laraue.Linq2Triggers.Core.CSharpMethods;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.CSharpMethods
{
    /// <summary>
    /// Base visitor for <see cref="System.String"/> methods.
    /// </summary>
    public abstract class BaseBinaryFunctionsVisitor : BaseMethodCallVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(BinaryFunctions);

        /// <inheritdoc />
        protected BaseBinaryFunctionsVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}