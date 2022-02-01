﻿using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

namespace Laraue.EfCoreTriggers.MySql;

public class MySqlTriggerInsertActionVisitor : TriggerInsertActionVisitor
{
    public MySqlTriggerInsertActionVisitor(ISetExpressionVisitorFactory factory, IEfCoreMetadataRetriever metadataRetriever, ISqlGenerator sqlGenerator) 
        : base(factory, metadataRetriever, sqlGenerator)
    {
    }

    public override SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression, ArgumentTypes argumentTypes)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.Append("() VALUES ()");
        return sqlBuilder;
    }
}