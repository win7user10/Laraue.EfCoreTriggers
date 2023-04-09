using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

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
        
        /// <inheritdoc />
        public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
        {
            var whereExpressions = new HashSet<Expression>();
            var exp = GetFlattenExpressions(expression, whereExpressions);

            if (exp is not MemberExpression baseMember)
            {
                throw new InvalidOperationException("Member expression was excepted");
            }
            
            var enumerableMemberType = baseMember.Type;
            
            var originalSetType = baseMember.Expression?.Type
                ?? throw new InvalidOperationException("Not null expression in the passed member excepted.");

            if (!typeof(IEnumerable).IsAssignableFrom(enumerableMemberType) || !enumerableMemberType.IsGenericType)
            {
                throw new NotSupportedException($"Don't know how to translate expression {expression}");
            }
            
            var entityType = baseMember.Type.GetGenericArguments()[0];

            var otherArguments = expression.Arguments
                .Skip(1)
                .ToArray();
            
            var selectSql = Visit(otherArguments, visitedMembers);

            if (selectSql.Item2 is not null)
            {
                whereExpressions.Add(selectSql.Item2);
            }

            var finalSql = SqlBuilder.FromString("(");
            
            finalSql.WithIdent(x=> x
                .Append("SELECT ")
                .Append(selectSql.Item1)
                .AppendNewLine($"FROM {_sqlGenerator.GetTableSql(entityType)}")
                .AppendNewLine($"INNER JOIN {_sqlGenerator.GetTableSql(originalSetType)} ON "));

            var keys = _schemaRetriever.GetForeignKeyMembers(entityType, originalSetType);

            var joinParts = new List<string>();
            foreach (var key in keys)
            {
                var column1Sql = _sqlGenerator.GetColumnSql(entityType, key.ForeignKey, ArgumentType.Default);

                if (baseMember.Expression is not MemberExpression memberExpression)
                {
                    throw new InvalidOperationException("Member expression was excepted");
                }
                
                var argument2Type = memberExpression.Member.GetArgumentType();
                
                var column2WhereSql = _sqlGenerator.GetColumnValueReferenceSql(originalSetType, key.PrincipalKey, argument2Type);
                visitedMembers.AddMember(argument2Type, key.PrincipalKey);
                
                var column2JoinSql = _sqlGenerator.GetColumnSql(originalSetType, key.PrincipalKey, ArgumentType.Default);

                joinParts.Add($"{column1Sql} = {column2JoinSql}");
                joinParts.Add($"{column1Sql} = {column2WhereSql}");
            }

            foreach (var e in whereExpressions)
            {
                joinParts.Add(_expressionVisitorFactory.Visit(e, visitedMembers));
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

        /// <summary>
        /// Generate pairs SqlBuilder -> Expression for all passed expressions
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        protected abstract (SqlBuilder, Expression) Visit(
            IEnumerable<Expression> arguments,
            VisitedMembers visitedMembers);
    }
}
