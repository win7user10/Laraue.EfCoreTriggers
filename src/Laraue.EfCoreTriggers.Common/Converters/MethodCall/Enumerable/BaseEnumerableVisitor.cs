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
        private readonly IExpressionVisitorFactory _expressionVisitorFactory;

        /// <inheritdoc />
        protected BaseEnumerableVisitor(
            IExpressionVisitorFactory visitorFactory,
            IDbSchemaRetriever schemaRetriever,
            ISqlGenerator sqlGenerator) 
            : base(visitorFactory)
        {
            _schemaRetriever = schemaRetriever;
            _sqlGenerator = sqlGenerator;
            _expressionVisitorFactory = visitorFactory;
        }
        
        public override SqlBuilder Visit(MethodCallExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
        {
            var whereExpressions = new HashSet<Expression>();
            var exp = GetFlattenExpressions(expression, whereExpressions);
            
            var baseMember = exp as MemberExpression;
            var enumerableMemberType = baseMember.Type;
            
            var originalSetMemberExpression = (ParameterExpression) baseMember.Expression;
            var originalSetType = originalSetMemberExpression?.Type;

            if (!typeof(IEnumerable).IsAssignableFrom(enumerableMemberType) || !enumerableMemberType.IsGenericType)
            {
                throw new NotSupportedException($"Don't know how to translate expression {expression}");
            }
            
            var entityType = baseMember.Type.GetGenericArguments()[0];

            var otherArguments = expression.Arguments
                .Skip(1)
                .ToArray();
            
            var sql = Visit(otherArguments, argumentTypes, visitedMembers);

            if (sql.Item2 is not null)
            {
                whereExpressions.Add(sql.Item2);
            }

            var finalSql = SqlBuilder.FromString("(");
            
            finalSql.WithIdent(x=> x
                .Append("SELECT ")
                .Append(sql.Item1)
                .AppendNewLine($"FROM {_schemaRetriever.GetTableName(entityType)}")
                .AppendNewLine($"INNER JOIN {_schemaRetriever.GetTableName(originalSetType)} ON "));

            var keys = _schemaRetriever.GetForeignKeyMembers(entityType, originalSetType);

            var joinParts = new List<string>();
            foreach (var key in keys)
            {
                var c1 = _sqlGenerator.GetColumnSql(entityType, key.ForeignKey, ArgumentType.Default);
                
                var arg2Type = argumentTypes.Get(originalSetMemberExpression);
                
                var c2AsWhere = _sqlGenerator.GetVariableSql(originalSetType, key.PrincipalKey, arg2Type);
                visitedMembers.AddMember(arg2Type, key.PrincipalKey);
                
                var c2AsJoin = _sqlGenerator.GetColumnSql(originalSetType, key.PrincipalKey, ArgumentType.Default);

                joinParts.Add($"{c1} = {c2AsJoin}");
                joinParts.Add($"{c1} = {c2AsWhere}");
            }

            foreach (var e in whereExpressions)
            {
                joinParts.Add(_expressionVisitorFactory.Visit(e, argumentTypes, visitedMembers));
            }

            finalSql.AppendJoin(" AND ", joinParts);
            
            finalSql.Append(")");

            return finalSql;
        }

        private Expression GetFlattenExpressions(MethodCallExpression methodCallExpression, HashSet<Expression> whereExpressions)
        {
            while (true)
            {
                if (methodCallExpression.Arguments[0] is not MethodCallExpression childCall ||
                    childCall.Method.Name != nameof(System.Linq.Enumerable.Where))
                {
                    return methodCallExpression.Arguments[0];
                }
                
                whereExpressions.Add(childCall.Arguments[1]);

                methodCallExpression = childCall;
            }
        }

        protected abstract (SqlBuilder, Expression) Visit(
            Expression[] arguments,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers);
    }
}
