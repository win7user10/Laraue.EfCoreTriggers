using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Enumerable.Count
{
    /// <inheritdoc />
    public sealed class CountVisitor : BaseEnumerableVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(System.Linq.Enumerable.Count);

        /// <summary>
        /// Initializes a new instance of <see cref="CountVisitor"/>.
        /// </summary>
        /// <param name="visitorFactory"></param>
        /// <param name="schemaRetriever"></param>
        /// <param name="sqlGenerator"></param>
        public CountVisitor(
            IExpressionVisitorFactory visitorFactory,
            IDbSchemaRetriever schemaRetriever,
            ISqlGenerator sqlGenerator)
            : base(visitorFactory, schemaRetriever, sqlGenerator)
        {
        }
    
        /// <inheritdoc />
        protected override (SqlBuilder, Expression) Visit(IEnumerable<Expression> arguments, VisitedMembers visitedMembers)
        {
            return  (SqlBuilder.FromString("count(*)"), arguments.FirstOrDefault());
        }
    }
}