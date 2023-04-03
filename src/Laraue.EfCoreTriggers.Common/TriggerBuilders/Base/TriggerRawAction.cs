using System;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public abstract class TriggerRawAction : ITriggerAction
    {
        internal readonly LambdaExpression[] ArgumentSelectorExpressions;
        
        internal readonly string Sql;

        protected TriggerRawAction(string sql, LambdaExpression[]? argumentSelectors)
        {
            ArgumentSelectorExpressions = argumentSelectors;
            Sql = sql;
        }
    }
    
    public sealed class TriggerRawAction<TEntity, TTableRefs> : TriggerRawAction
        where TEntity : class
        where TTableRefs : ITableRef<TEntity>
    {
        public TriggerRawAction(string sql, params Expression<Func<TTableRefs, object>>[] argumentSelectors)
            :base(sql, argumentSelectors.Cast<LambdaExpression>().ToArray())
        {
        }
    }
}