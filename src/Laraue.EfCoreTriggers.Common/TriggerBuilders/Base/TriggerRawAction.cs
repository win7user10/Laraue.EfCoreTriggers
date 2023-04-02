using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public sealed class TriggerRawAction<TEntity, TTableRefs> : ITriggerAction
        where TEntity : class
        where TTableRefs : ITableRef<TEntity>
    {
        internal readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        internal readonly string Sql;

        public TriggerRawAction(string sql, params Expression<Func<TTableRefs, object>>[]? argumentSelectors)
        {
            ArgumentSelectorExpressions = argumentSelectors?.Cast<LambdaExpression>().ToArray()
                ?? Array.Empty<LambdaExpression>();
            Sql = sql;
        }
    }
}