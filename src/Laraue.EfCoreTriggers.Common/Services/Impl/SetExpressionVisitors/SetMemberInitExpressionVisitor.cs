using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;

/// <inheritdoc />
public class SetMemberInitExpressionVisitor : IMemberInfoVisitor<MemberInitExpression>
{
    private readonly IExpressionVisitorFactory _factory;
    
    /// <summary>
    /// Initializes a new instance of <see cref="SetMemberInitExpressionVisitor"/>.
    /// </summary>
    /// <param name="factory"></param>
    public SetMemberInitExpressionVisitor(IExpressionVisitorFactory factory)
    {
        _factory = factory;
    }

    /// <inheritdoc />
    public Dictionary<MemberInfo, SqlBuilder> Visit(MemberInitExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return expression.Bindings.Select(memberBinding =>
        {
            var memberAssignmentExpression = (MemberAssignment)memberBinding;
            
            var sqlExtendedResult = _factory.Visit(memberAssignmentExpression.Expression, argumentTypes, visitedMembers);
            
            return (memberAssignmentExpression.Member, sqlExtendedResult);
        }).ToDictionary(x => x.Member, x => x.sqlExtendedResult);
    }
}