using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

public abstract class NewTriggerAction : ITriggerAction
{
    public IReadOnlyList<ITriggerAction> ActionConditions => _actionConditions;
    public IReadOnlyList<ITriggerAction> ActionExpressions => _actionExpressions;

    protected readonly List<TriggerCondition> _actionConditions = new();
        
    protected readonly List<ITriggerAction> _actionExpressions = new();
}

public sealed class NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> : NewTriggerAction
    where TTriggerEntity : class
    where TTriggerEntityRefs : ITableRef<TTriggerEntity>
{
    protected NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> AddAction(ITriggerAction triggerAction)
    {
        _actionExpressions.Add(triggerAction);

        return this;
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> Condition(
        Expression<Func<TTriggerEntityRefs, bool>> conditionalExpression)
    {
        // Throw on expressions like "_ => true"
        if (conditionalExpression.Body is ConstantExpression)
        {
            throw new InvalidOperationException("Condition with constant expression makes no sense");
        }

        return AddAction(new TriggerCondition(conditionalExpression));
    }

    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> Insert<TInsertEntity>(
        Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
    {
        return AddAction(new TriggerInsertAction(insertExpression));
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> Delete<TDeleteEntity>(
        Expression<Func<TTriggerEntityRefs, TDeleteEntity, bool>> deletePredicate)
    {
        return AddAction(new TriggerDeleteAction(deletePredicate));
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> Update<TUpdateEntity>(
        Expression<Func<TTriggerEntityRefs, TUpdateEntity, bool>> updatePredicate,
        Expression<Func<TTriggerEntityRefs, TUpdateEntity, TUpdateEntity>> updateExpression)
    {
        return AddAction(new TriggerUpdateAction(updatePredicate, updateExpression));
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> Upsert<TUpsertEntity>(
        Expression<Func<TTriggerEntityRefs, TUpsertEntity, bool>> upsertPredicate,
        Expression<Func<TTriggerEntityRefs, TUpsertEntity>> insertExpression,
        Expression<Func<TTriggerEntityRefs, TUpsertEntity, TUpsertEntity>> updateExpression)
    {
        return AddAction(
            new TriggerUpsertAction(
                upsertPredicate,
                insertExpression,
                updateExpression));
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> InsertIfNotExists<TInsertEntity>(
        Expression<Func<TTriggerEntityRefs, TInsertEntity, bool>> insertPredicate,
        Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
    {
        return AddAction(
            new TriggerUpsertAction(
                insertPredicate,
                insertExpression,
                null));
    }
    
    public NewTriggerAction<TTriggerEntity, TTriggerEntityRefs> ExecuteRawSql(
        string sql,
        params Expression<Func<TTriggerEntityRefs, object>>[] getSqlVariable)
    {
        return AddAction(new TriggerRawAction<TTriggerEntity, TTriggerEntityRefs>(sql, getSqlVariable));
    }
}