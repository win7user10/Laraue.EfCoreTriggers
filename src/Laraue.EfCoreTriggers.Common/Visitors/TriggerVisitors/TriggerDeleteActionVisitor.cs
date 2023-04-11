using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors
{
    public sealed class TriggerDeleteActionVisitor : ITriggerActionVisitor<TriggerDeleteAction>
    {
        private readonly ISqlGenerator _sqlGenerator;
        private readonly ITriggerActionVisitorFactory _factory;

        public TriggerDeleteActionVisitor(ISqlGenerator sqlGenerator, ITriggerActionVisitorFactory factory)
        {
            _sqlGenerator = sqlGenerator;
            _factory = factory;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(TriggerDeleteAction triggerAction, VisitedMembers visitedMembers)
        {
            var tableType = triggerAction.Predicate.Parameters.Last().Type;

            var triggerCondition = new TriggerCondition(triggerAction.Predicate);
            var conditionStatement = _factory.Visit(triggerCondition, visitedMembers);
        
            return new SqlBuilder()
                .Append($"DELETE FROM {_sqlGenerator.GetTableSql(tableType)}")
                .AppendNewLine("WHERE ")
                .Append(conditionStatement)
                .Append(";");
        }
    }
}