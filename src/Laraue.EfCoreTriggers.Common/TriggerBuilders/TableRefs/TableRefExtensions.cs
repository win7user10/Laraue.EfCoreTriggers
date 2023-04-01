using System;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

public static class TableRefExtensions
{
    public static ArgumentTypes GetArgumentTypes<TEntity>(
        this ITableRef<TEntity> tableRef,
        LambdaExpression expression)
        where TEntity : class
    {
        throw new NotImplementedException();
    }
}