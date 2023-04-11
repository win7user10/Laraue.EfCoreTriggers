using System;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors
{
    public sealed class TriggerActionVisitorFactory : ITriggerActionVisitorFactory
    {
        private readonly IServiceProvider _provider;

        public TriggerActionVisitorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(ITriggerAction triggerAction, VisitedMembers visitedMembers)
        {
            return triggerAction switch
            {
                TriggerRawAction rawAction => Visit(rawAction, visitedMembers),
                TriggerCondition condition => Visit(condition, visitedMembers),
                TriggerUpdateAction updateAction => Visit(updateAction, visitedMembers),
                TriggerUpsertAction upsertAction => Visit(upsertAction, visitedMembers),
                TriggerDeleteAction deleteAction => Visit(deleteAction, visitedMembers),
                TriggerInsertAction insertAction => Visit(insertAction, visitedMembers),
                TriggerActionsGroup newTriggerAction => Visit(newTriggerAction, visitedMembers),
                _ => throw new NotSupportedException($"Trigger action {triggerAction.GetType()} not supported")
            };
        }

        private SqlBuilder Visit<T>(T triggerAction, VisitedMembers visitedMembers)
            where T : ITriggerAction
        {
            return _provider.GetRequiredService<ITriggerActionVisitor<T>>()
                .Visit(triggerAction, visitedMembers);
        }
    }
}