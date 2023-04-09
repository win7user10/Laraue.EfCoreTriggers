using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public class UnaryExpressionVisitor : BaseExpressionVisitor<UnaryExpression>
    {
        private readonly IExpressionVisitorFactory _factory;
        private readonly ISqlGenerator _generator;
        private readonly IDbSchemaRetriever _dbSchemaRetriever;
    
        /// <inheritdoc />
        public UnaryExpressionVisitor(
            IExpressionVisitorFactory factory,
            ISqlGenerator generator,
            IDbSchemaRetriever dbSchemaRetriever)
        {
            _factory = factory;
            _generator = generator;
            _dbSchemaRetriever = dbSchemaRetriever;
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(UnaryExpression expression, VisitedMembers visitedMembers)
        {
            var internalExpressionSql = _factory.Visit(expression.Operand, visitedMembers);
            var sqlBuilder = new SqlBuilder();
        
            if (expression.NodeType == ExpressionType.Convert)
            {
                if (IsNeedConversion(expression))
                {
                    sqlBuilder.Append(GetConvertExpressionSql(expression, internalExpressionSql));
                }
                else
                {
                    sqlBuilder = internalExpressionSql;
                }

                return sqlBuilder;
            }
        
            sqlBuilder.Append(_generator.GetUnarySql(expression.NodeType, internalExpressionSql));
            
            return sqlBuilder;
        }
    
        /// <summary>
        /// Analyze, does passed <see cref="UnaryExpression"/> needs to cast into the Database.
        /// For example, casting of <see cref="Enum"/> values to <see cref="int"/> is not necessary, 
        /// because each <see cref="Enum"></see> is stored as <see cref="int"/> in the Database.
        /// </summary>
        /// <param name="unaryExpression"></param>
        /// <returns></returns>
        protected virtual bool IsNeedConversion(UnaryExpression unaryExpression)
        {
            // Do not execute conversion Type? -> Type, it is actual for CLR only
            if (Nullable.GetUnderlyingType(unaryExpression.Type) != null)
            {
                return false;
            }
        
            var clrType1 = GetActualClrType(unaryExpression.Operand);
            var clrType2 = GetActualClrType(unaryExpression);
            if (clrType1 == typeof(object) || clrType2 == typeof(object))
            {
                return false;
            }
        
            var sqlType1 = _generator.GetSqlType(clrType1);
            var sqlType2 = _generator.GetSqlType(clrType2);
            return sqlType1 != sqlType2;
        }
    
        /// <summary>
        /// Generate SQL expression to cast passed <paramref name="member"/>
        /// to type represents in <paramref name="unaryExpression"/>.
        /// </summary>
        /// <param name="unaryExpression"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual string GetConvertExpressionSql(UnaryExpression unaryExpression, string member)
        {
            var sqlType = _generator.GetSqlType(unaryExpression.Type);
            return sqlType is not null
                ? $"CAST({member} AS {sqlType})"
                : throw new NotSupportedException($"Converting of type {unaryExpression.Type} is not supported");
        }

        private Type GetActualClrType(Expression expression)
        {
            if (expression is not MemberExpression {Expression: ParameterExpression parameterExpression} memberExpression)
            {
                return EfCoreTriggersHelper.GetNotNullableType(expression.Type);
            }
        
            return _dbSchemaRetriever.GetActualClrType(
                EfCoreTriggersHelper.GetNotNullableType(parameterExpression.Type),
                memberExpression.Member);
        }
    }
}