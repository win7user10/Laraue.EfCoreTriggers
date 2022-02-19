using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Enumerable
{
    /// <summary>
    /// Base visitor for <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public abstract class BaseEnumerableVisitor : BaseMethodCallVisitor
    {
        /// <inheritdoc />
        protected override Type ReflectedType => typeof(System.Linq.Enumerable);

        private readonly IDbSchemaRetriever _schemaRetriever;
        private readonly ISqlGenerator _sqlGenerator;

        /// <inheritdoc />
        protected BaseEnumerableVisitor(IExpressionVisitorFactory visitorFactory, IDbSchemaRetriever schemaRetriever, ISqlGenerator sqlGenerator) 
            : base(visitorFactory)
        {
            _schemaRetriever = schemaRetriever;
            _sqlGenerator = sqlGenerator;
        }

        public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
        {
            var enumerableMemberExpression = (MemberExpression) expression.Arguments[0];
            var enumerableMemberType = enumerableMemberExpression.Type;
            
            var originalSetMemberExpression = (ParameterExpression) enumerableMemberExpression.Expression;
            var originalSetType = originalSetMemberExpression?.Type;

            if (!typeof(IEnumerable).IsAssignableFrom(enumerableMemberType) || !enumerableMemberType.IsGenericType)
            {
                throw new NotSupportedException($"Don't know how to translate expression {expression}");
            }
            
            var entityType = enumerableMemberExpression.Type.GetGenericArguments()[0];

            var countSql = Visit(expression.Arguments, argumentTypes, visitedMembers);

            var finalSql = SqlBuilder.FromString("(");
            
            finalSql.WithIdent(x=> x
                .Append("SELECT ")
                .Append(countSql)
                .AppendNewLine($"FROM {_schemaRetriever.GetTableName(entityType)}")
                .AppendNewLine($"INNER JOIN {_schemaRetriever.GetTableName(originalSetType)} ON "));

            var keys = _schemaRetriever.GetForeignKeyMembers(entityType, originalSetType);

            var joinParts = new List<string>();
            foreach (var key in keys)
            {
                var c1 = _sqlGenerator.GetColumnSql(entityType, key.ForeignKey, ArgumentType.Default);
                
                var arg2Type = argumentTypes.Get(originalSetMemberExpression);
                var c2AsWhere = _sqlGenerator.GetColumnSql(originalSetType, key.PrincipalKey, arg2Type);
                var c2AsJoin = _sqlGenerator.GetColumnSql(originalSetType, key.PrincipalKey, ArgumentType.Default);

                joinParts.Add($"{c1} = {c2AsJoin}");
                joinParts.Add($"{c1} = {c2AsWhere}");
            }

            finalSql.AppendJoin(" AND ", joinParts);
            
            finalSql.Append(")");

            return finalSql;
        }

        protected abstract SqlBuilder Visit(IReadOnlyCollection<Expression> arguments, ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers);
    }
}
