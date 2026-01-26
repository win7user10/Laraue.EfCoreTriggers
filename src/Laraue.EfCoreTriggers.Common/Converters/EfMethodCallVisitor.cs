using System;
using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MethodCall;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.TableRefs;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Common.Converters;

public class EfMethodCallVisitor : BaseMethodCallVisitor
{
    private readonly ISqlGenerator _sqlGenerator;

    public EfMethodCallVisitor(
        IExpressionVisitorFactory visitorFactory,
        ISqlGenerator sqlGenerator)
        : base(visitorFactory)
    {
        _sqlGenerator = sqlGenerator;
    }

    /// <inheritdoc />
    protected override string MethodName => nameof(EF.Property);
    
    /// <inheritdoc />
    protected override Type ReflectedType => typeof(EF);
    
    /// <inheritdoc />
    public override SqlBuilder Visit(MethodCallExpression expression, VisitedMembers visitedMembers)
    {
        if (expression.Arguments[0] is not MemberExpression dbSetExpression)
        {
            throw new NotSupportedException("MemberExpression as first argument excepted");
        }
        
        if (expression.Arguments[1] is not ConstantExpression constantExpression)
        {
            throw new NotSupportedException("Constant Expression as second argument excepted");
        }

        if (constantExpression.Value is not string propertyName)
        {
            throw new NotSupportedException("String value as second argument excepted");
        }
        
        var dbSetType = dbSetExpression.Type;
        var argumentType = dbSetExpression.Member.GetArgumentType();
        var columnSql = _sqlGenerator.GetColumnValueReferenceSql(dbSetType, propertyName, argumentType);
        
        visitedMembers.AddMember(argumentType, new VisitedMemberInfo(dbSetType, propertyName));

        return new SqlBuilder().Append(columnSql);
    }
}