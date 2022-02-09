﻿using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerMemberExpressionVisitor : MemberExpressionVisitor
{
    public SqlServerMemberExpressionVisitor(ISqlGenerator generator) 
        : base(generator)
    {
    }

    protected override string Visit(MemberExpression memberExpression, ArgumentType argumentType)
    {
        var memberInfo = memberExpression.Member;
        
        return argumentType switch
        {
            ArgumentType.New => SqlServerTriggerVisitor.GetVariableNameSql(argumentType, memberInfo),
            ArgumentType.Old => SqlServerTriggerVisitor.GetVariableNameSql(argumentType, memberInfo),
            _ => base.Visit(memberExpression, argumentType)
        };
    }
}