using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat
{
    public abstract class BaseStringConcatConverter : BaseStringConverter
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.Concat);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argumentsSql = visitor.VisitArguments(expression, argumentTypes, visitedMembers);
            return BuildSql(argumentsSql);
        }

        protected abstract SqlBuilder BuildSql(SqlBuilder[] argumentsSql);
    }
}