using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2
{
    public abstract class BaseAtan2Converter : BaseMathConverter
    {
        protected override string MethodName => nameof(System.Math.Atan2);

        protected abstract string SqlFunctionName { get; }

        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argumentsSql = visitor.VisitArguments(expression, argumentTypes, visitedMembers);
            
            return new SqlBuilder($"{SqlFunctionName}({argumentsSql[0]}, {argumentsSql[1]})")
                .MergeColumnsInfo(argumentsSql);
        }
    }
}