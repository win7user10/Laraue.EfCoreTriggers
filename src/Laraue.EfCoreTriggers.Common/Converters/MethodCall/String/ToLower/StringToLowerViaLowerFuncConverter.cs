using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower
{
    public class StringToLowerViaLowerFuncConverter : BaseStringConverter
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.ToLower);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var sqlBuilder = visitor.Visit(expression.Object, argumentTypes, visitedMembers);
            return new(sqlBuilder.AffectedColumns, $"LOWER({sqlBuilder})");
        }
    }
}