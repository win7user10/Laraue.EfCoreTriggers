using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2;

public interface IRootExpressionVisitor
{
    IReadOnlyDictionary<ArgumentType, HashSet<MemberInfo>> VisitedMembers { get; }
    SqlBuilder Visit(Expression expression, ArgumentTypes argumentTypes);
}