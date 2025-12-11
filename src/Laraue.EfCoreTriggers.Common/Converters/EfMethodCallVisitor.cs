using System;
using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MethodCall;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Common.Converters;

public class EfMethodCallVisitor : BaseMethodCallVisitor
{
    public EfMethodCallVisitor(IExpressionVisitorFactory visitorFactory)
        : base(visitorFactory)
    {
    }

    /// <inheritdoc />
    protected override string MethodName => nameof(EF.Property); // TODO - applicable to all methods
    
    /// <inheritdoc />
    protected override Type ReflectedType => typeof(EF);
    public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
    {
        throw new NotImplementedException();
    }
}