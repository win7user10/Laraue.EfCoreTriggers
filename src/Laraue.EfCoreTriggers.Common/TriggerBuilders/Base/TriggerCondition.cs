using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public class TriggerCondition : ITriggerAction
    {
        /// <summary>
        /// Expression to delete, e.g. Users.Where(x => x.Id == 2)
        /// </summary>
        internal readonly LambdaExpression Condition;
        
        public TriggerCondition(LambdaExpression condition)
        {
            Condition = condition;
        }
    }

    public sealed class TriggerCondition<TEntity, TTableRefs> : TriggerCondition
        where TEntity : class
        where TTableRefs : ITableRef<TEntity>
    {
        public TriggerCondition(Expression<Func<TTableRefs, bool>> condition)
            : base(condition)
        {
        }
    }
}