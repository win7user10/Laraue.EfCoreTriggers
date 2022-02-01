using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Acos
{
    public class MathAcosVisitor : BaseMathVisitor
    {
        protected override string MethodName => nameof(System.Math.Acos);

        public MathAcosVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = VisitorFactory.Visit(argument, argumentTypes, visitedMembers);
            return SqlBuilder.FromString($"ACOS({sqlBuilder})");
        }
    }
}
