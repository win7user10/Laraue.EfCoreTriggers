using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.CSharpMethods;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public class BinaryExpressionVisitor : BaseExpressionVisitor<BinaryExpression>
    {
        private readonly IExpressionVisitorFactory _factory;
        private readonly ISqlGenerator _generator;
        private readonly IDbSchemaRetriever _schemaRetriever;
    
        /// <inheritdoc />
        public BinaryExpressionVisitor(
            IExpressionVisitorFactory factory,
            ISqlGenerator generator,
            IDbSchemaRetriever schemaRetriever)
        {
            _factory = factory;
            _generator = generator;
            _schemaRetriever = schemaRetriever;
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            BinaryExpression expression,
            VisitedMembers visitedMembers)
        {
            if (expression is 
                {
                    Left: UnaryExpression
                    {
                        NodeType: ExpressionType.Convert, 
                        Operand: MemberExpression memberExpression
                    },
                    Right: ConstantExpression constantExpression
                })
            {
                // Convert(enumValue, Int32) == 1 when enum is stores as string -> enumValue == Enum.Value
                var clrType = _schemaRetriever.GetActualClrType(
                    memberExpression.Member.DeclaringType,
                    memberExpression.Member);
            
                if (memberExpression.Type.IsEnum && clrType == typeof(string))
                {
                    var valueName = Enum.GetValues(memberExpression.Type)
                        .Cast<object>()
                        .First(x => (int)x == (int)constantExpression.Value!)
                        .ToString()!;
                
                    var sb = _factory.Visit(memberExpression, visitedMembers);
                    sb.Append($" = {_generator.GetSql(valueName)}");
                    return sb;
                }
            
                // Convert(charValue, Int32) == 122 -> charValue == 'z'
                if (memberExpression.Type == typeof(char))
                {
                    var memberSql = _factory.Visit(memberExpression, visitedMembers);
                    var constantSql = _factory.Visit(Expression.Constant(Convert.ToChar(constantExpression.Value)), visitedMembers);

                    return memberSql
                        .Append(" = ")
                        .Append(constantSql);
                }
            }
        
            if (expression.Method?.Name == nameof(string.Concat))
            {
                return _factory.Visit(
                    Expression.Call(
                        null,
                        expression.Method,
                        expression.Left,
                        expression.Right),
                    visitedMembers);
            }

            var binaryExpressionParts = GetBinaryExpressionParts(expression);
        
            if (expression.NodeType == ExpressionType.Coalesce)
            {
                var methodInfo = typeof(BinaryFunctions).GetMethod(nameof(BinaryFunctions.Coalesce))!
                    .MakeGenericMethod(binaryExpressionParts[0].Type);
            
                var methodCall = Expression.Call(
                    null,
                    methodInfo,
                    binaryExpressionParts[0],
                    Expression.Convert(binaryExpressionParts[1], binaryExpressionParts[0].Type));

                return _factory.Visit(methodCall, visitedMembers);
            }

            // Check, if one argument is null, should be generated expression "value IS NULL"
            if (expression.NodeType is ExpressionType.Equal or ExpressionType.NotEqual)
            {
                if (binaryExpressionParts.Any(x => x is ConstantExpression { Value: null }))
                {
                    var secondArgument = binaryExpressionParts
                        .First(x => 
                            x is ConstantExpression { Value: null });
                
                    var firstArgument = binaryExpressionParts
                        .Except(new[] { secondArgument })
                        .First();
                
                    var argumentsSql = new[] { firstArgument, secondArgument }
                        .Select(part => 
                            _factory.Visit(part, visitedMembers))
                        .ToArray();
                
                    var sqlBuilder = new SqlBuilder()
                        .Append(argumentsSql[0])
                        .Append(" IS ");

                    if (expression.NodeType is ExpressionType.NotEqual)
                    {
                        sqlBuilder.Append("NOT ");
                    }

                    sqlBuilder.Append(argumentsSql[1]);

                    return sqlBuilder;
                }
            }

            var binaryParts = binaryExpressionParts
                .Select(part => 
                    _factory.Visit(
                        part,
                        visitedMembers))
                .ToArray();
        
            var leftArgument = binaryParts[0];
            var rightArgument = binaryParts[1];

            return new SqlBuilder()
                .Append(_generator.GetBinarySql(expression.NodeType, leftArgument, rightArgument));
        }

        private static Expression[] GetBinaryExpressionParts(BinaryExpression expression)
        {
            var parts = new[] { expression.Left, expression.Right };
            if (expression.Method is not null) return parts;
            if (expression.Left is MemberExpression leftMemberExpression && leftMemberExpression.Type == typeof(bool))
                parts[0] = Expression.IsTrue(expression.Left);
            if (expression.Right is MemberExpression rightMemberExpression && rightMemberExpression.Type == typeof(bool))
                parts[1] = Expression.IsTrue(expression.Right);
            return parts;
        }
    }
}