using System;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math
{
    public abstract class BaseMathVisitor : BaseMethodCallVisitor
    {
        protected override Type ReflectedType => typeof(System.Math);

        protected BaseMathVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}
