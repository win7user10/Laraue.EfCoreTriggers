using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public class MemberExpressionVisitor : BaseExpressionVisitor<MemberExpression>
{
    private readonly ISqlGenerator _generator;
    
    public MemberExpressionVisitor(ISqlGenerator generator)
    {
        _generator = generator;
    }

    public override SqlBuilder Visit(MemberExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        argumentTypes ??= new ArgumentTypes();
        var parameterExpression = (ParameterExpression)expression.Expression;
        var memberName = parameterExpression.Name;
        if (!argumentTypes.TryGetValue(memberName, out var argumentType))
        {
            argumentType = ArgumentType.Default;
        }
        
        visitedMembers.AddMember(argumentType, expression.Member);
        
        return SqlBuilder.FromString(Visit(expression, argumentType));
    }
    
    protected virtual string Visit(MemberExpression memberExpression, ArgumentType argumentType)
    {
        return _generator.GetColumnSql(memberExpression.Member, argumentType);
    }
}