﻿using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.SqlLite;

public class SqliteNewExpressionVisitor : NewExpressionVisitor
{
    protected override SqlBuilder GetNewGuidSql()
    {
        return SqlBuilder.FromString(
            "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || " +
            "substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || " +
            "substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))");
    }
}