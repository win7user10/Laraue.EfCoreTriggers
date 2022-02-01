using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
{
    public abstract class BaseAtan2Visitor : BaseMathVisitor
    {
        protected override string MethodName => nameof(System.Math.Atan2);
        
        protected BaseAtan2Visitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        protected abstract string SqlFunctionName { get; }

        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argumentsSql = VisitorFactory.VisitArguments(expression, argumentTypes, visitedMembers);
            
            return SqlBuilder.FromString($"{SqlFunctionName}({argumentsSql[0]}, {argumentsSql[1]})");
        }
    }
}