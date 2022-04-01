using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class BinaryExpressionVisitor : BaseExpressionVisitor<BinaryExpression>
{
    private readonly IExpressionVisitorFactory _factory;
    private readonly ISqlGenerator _generator;
    
    /// <inheritdoc />
    public BinaryExpressionVisitor(IExpressionVisitorFactory factory, ISqlGenerator generator)
    {
        _factory = factory;
        _generator = generator;
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(
        BinaryExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers)
    {
        // Convert(charValue, Int32) == 122 -> charValue == 'z'
        if (expression.Left is UnaryExpression
            {
                NodeType: ExpressionType.Convert, 
                Operand: MemberExpression memberExpression
            } 
            && memberExpression.Type == typeof(char) 
            && expression.Right is ConstantExpression constantExpression)
        {
            var memberSql = _factory.Visit(memberExpression, argumentTypes, visitedMembers);
            var constantSql = _factory.Visit(Expression.Constant(Convert.ToChar(constantExpression.Value)), argumentTypes, visitedMembers);

            return memberSql
                .Append(" = ")
                .Append(constantSql);
        }
        
        if (expression.Method?.Name == nameof(string.Concat))
        {
            return _factory.Visit(
                Expression.Call(
                    null,
                    expression.Method,
                    expression.Left,
                    expression.Right),
                argumentTypes,
                visitedMembers);
        }

        var binaryExpressionParts = GetBinaryExpressionParts(expression);

        // Check, if one argument is null, should be generated expression "value IS NULL"
        if (expression.NodeType is ExpressionType.Equal || expression.NodeType is ExpressionType.NotEqual)
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
                        _factory.Visit(part, argumentTypes, visitedMembers))
                    .ToArray();
                
                return new SqlBuilder()
                    .Append(argumentsSql[0])
                    .Append(" IS NULL");
            }
        }

        var binaryParts = binaryExpressionParts
            .Select(part => 
                _factory.Visit(
                    part,
                    argumentTypes,
                    visitedMembers))
            .ToArray();

        return new SqlBuilder()
            .Append(binaryParts[0])
            .Append(" ")
            .Append(_generator.GetOperand(expression))
            .Append(" ")
            .Append(binaryParts[1]);
    }
    
    Expression[] GetBinaryExpressionParts(BinaryExpression expression)
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