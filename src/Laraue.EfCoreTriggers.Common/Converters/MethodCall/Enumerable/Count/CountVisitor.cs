using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Enumerable.Count;

public class CountVisitor : BaseEnumerableVisitor
{
    protected override string MethodName => nameof(System.Linq.Enumerable.Count);
    protected override SqlBuilder Visit(IReadOnlyCollection<Expression> arguments, ArgumentTypes argumentTypes, VisitedMembers visitedMembers)
    {
        return SqlBuilder.FromString("count(*)");
    }

    public CountVisitor(IExpressionVisitorFactory visitorFactory, IDbSchemaRetriever schemaRetriever, ISqlGenerator sqlGenerator)
        : base(visitorFactory, schemaRetriever, sqlGenerator)
    {
    }
}