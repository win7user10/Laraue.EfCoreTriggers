using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

public class SetMemberInitExpressionVisitor : ISetExpressionVisitor<MemberInitExpression>
{
    private readonly IExpressionVisitorFactory _factory;

    public SetMemberInitExpressionVisitor(IExpressionVisitorFactory factory)
    {
        _factory = factory;
    }

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