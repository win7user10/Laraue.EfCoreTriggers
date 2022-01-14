using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling
{
    public class MathCeilingConverter : BaseMathConverter
    {
        protected override string MethodName => nameof(System.Math.Ceiling);

        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var sqlBuilder = visitor.Visit(argument, argumentTypes, visitedMembers);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"CEILING({sqlBuilder})");
        }
    }
}
