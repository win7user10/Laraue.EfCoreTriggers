using System;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String
{
    public abstract class BaseStringVisitor : BaseMethodCallVisitor
    {
        protected override Type ReflectedType => typeof(string);

        protected BaseStringVisitor(IExpressionVisitorFactory visitorFactory) 
            : base(visitorFactory)
        {
        }
    }
}