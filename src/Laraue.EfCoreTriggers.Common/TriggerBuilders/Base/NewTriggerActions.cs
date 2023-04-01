using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

public sealed class NewTriggerActions<TTriggerEntity, TTriggerEntityRefs>
{
    internal IEnumerable<ITriggerAction> ActionConditions => _actionConditions;

    internal IEnumerable<ITriggerAction> ActionExpressions => _actionExpressions;

    private readonly List<TriggerCondition> _actionConditions = new();
        
    private readonly List<ITriggerAction> _actionExpressions = new();

    protected NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> AddAction(ITriggerAction triggerAction)
    {
        _actionExpressions.Add(triggerAction);

        return this;
    }
    
    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> Condition(
        Expression<Func<TTriggerEntityRefs, bool>> conditionalExpression)
    {
        // Throw on expressions like "_ => true"
        if (conditionalExpression.Body is ConstantExpression)
        {
            throw new InvalidOperationException("Condition with constant expression makes no sense");
        }

        throw new NotImplementedException();
    }

    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> Insert<TInsertEntity>(
        Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
    {
        return AddAction(new TriggerInsertAction(insertExpression));
    }
    
    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> Delete<TDeleteEntity>(
        Expression<Func<TTriggerEntityRefs, TDeleteEntity, bool>> deletePredicate)
    {
        return AddAction(new TriggerInsertAction(deletePredicate));
    }
    
    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> Update<TUpdateEntity>(
        Expression<Func<TTriggerEntityRefs, TUpdateEntity, bool>> updatePredicate,
        Expression<Func<TTriggerEntityRefs, TUpdateEntity, TUpdateEntity>> updateExpression)
    {
        throw new NotImplementedException();
    }
    
    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> Upsert<TUpsertEntity>(
        Expression<Func<TTriggerEntityRefs, TUpsertEntity, bool>> upsertPredicate,
        Expression<Func<TTriggerEntityRefs, TUpsertEntity>> insertExpression,
        Expression<Func<TTriggerEntityRefs, TUpsertEntity, TUpsertEntity>> updateExpression)
    {
        throw new NotImplementedException();
    }
    
    public NewTriggerActions<TTriggerEntity, TTriggerEntityRefs> InsertIfNotExists<TInsertEntity>(
        Expression<Func<TTriggerEntityRefs, TInsertEntity, bool>> insertPredicate,
        Expression<Func<TTriggerEntityRefs, TInsertEntity>> insertExpression)
    {
        throw new NotImplementedException();
    }
}