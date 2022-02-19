using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

/// <inheritdoc />
public class MemberExpressionVisitor : BaseExpressionVisitor<MemberExpression>
{
    private readonly ISqlGenerator _generator;
    
    /// <inheritdoc />
    public MemberExpressionVisitor(ISqlGenerator generator)
    {
        _generator = generator;
    }

    /// <inheritdoc />
    public override SqlBuilder Visit(MemberExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        argumentTypes ??= new ArgumentTypes();
        
        var argumentType = argumentTypes.Get((ParameterExpression) expression.Expression);
        
        visitedMembers.AddMember(argumentType, expression.Member);
        
        return SqlBuilder.FromString(Visit(expression, argumentType));
    }
    
    /// <summary>
    /// Visit specified member with specified <see cref="ArgumentType"/>.
    /// </summary>
    /// <param name="memberExpression"></param>
    /// <param name="argumentType"></param>
    /// <returns></returns>
    protected virtual string Visit(MemberExpression memberExpression, ArgumentType argumentType)
    {
        return _generator.GetColumnSql(memberExpression.Expression.Type, memberExpression.Member, argumentType);
    }
}