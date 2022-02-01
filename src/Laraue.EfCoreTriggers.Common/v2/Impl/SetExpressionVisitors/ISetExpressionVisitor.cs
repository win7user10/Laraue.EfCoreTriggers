using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;

public interface ISetExpressionVisitor<in TExpression>
    where TExpression : Expression
{
    Dictionary<MemberInfo, SqlBuilder> Visit(TExpression expression, ArgumentTypes argumentTypes, VisitedMembers visitedMembers);
}