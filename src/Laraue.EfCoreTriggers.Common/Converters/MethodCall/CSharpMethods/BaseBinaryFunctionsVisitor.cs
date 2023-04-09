using System;
using Laraue.EfCoreTriggers.Common.CSharpMethods;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.CSharpMethods
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