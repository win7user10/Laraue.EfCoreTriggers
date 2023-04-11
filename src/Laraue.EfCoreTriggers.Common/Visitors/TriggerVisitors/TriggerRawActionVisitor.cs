using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors
{
    public class TriggerRawActionVisitor : ITriggerActionVisitor<TriggerRawAction>
    {
        private readonly IExpressionVisitorFactory _factory;

        public TriggerRawActionVisitor(IExpressionVisitorFactory factory)
        {
            _factory = factory;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(TriggerRawAction triggerAction, VisitedMembers visitedMembers)
        {
            var sqlBuilder = new SqlBuilder();

            if (triggerAction.ArgumentSelectorExpressions.Length > 0)
            {
                var sqlArgBuilders = new List<SqlBuilder>();
                
                for (var i = 0; i < triggerAction.ArgumentSelectorExpressions.Length; i++)
                {
                    var expression = triggerAction.ArgumentSelectorExpressions[i];
                    // TODO - var prefixes = triggerAction.ArgumentPrefixes[i];

                    sqlArgBuilders.Add(_factory.Visit(expression.Body, visitedMembers));
                }

                sqlBuilder.Append(
                    string.Format(
                        triggerAction.Sql, sqlArgBuilders
                            .Select(x => (object)x.ToString())
                            .ToArray()));
            }
            else
            {
                sqlBuilder.Append(triggerAction.Sql);
            }

            return sqlBuilder;
        }
    }
}