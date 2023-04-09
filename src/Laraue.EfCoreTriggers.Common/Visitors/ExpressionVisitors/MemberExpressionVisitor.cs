using System;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <inheritdoc />
    public sealed class MemberExpressionVisitor : BaseExpressionVisitor<MemberExpression>
    {
        private readonly ISqlGenerator _generator;
    
        /// <inheritdoc />
        public MemberExpressionVisitor(ISqlGenerator generator)
        {
            _generator = generator;
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(MemberExpression expression, VisitedMembers visitedMembers)
        {
            visitedMembers.AddMember(ArgumentType.Default, expression.Member);
        
            return SqlBuilder.FromString(Visit(expression, ArgumentType.Default, visitedMembers));
        }
    
        /// <summary>
        /// Visit specified member with specified <see cref="ArgumentType"/>.
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="argumentType"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        private string Visit(MemberExpression memberExpression, ArgumentType argumentType, VisitedMembers visitedMembers)
        {
            if (memberExpression.Expression is MemberExpression nestedMemberExpression)
            {
                return GetColumnSql(nestedMemberExpression, memberExpression.Member, visitedMembers);
            }

            return GetTableSql(memberExpression, argumentType);
        }

        private string GetTableSql(MemberExpression memberExpression, ArgumentType argumentType)
        {
            if (memberExpression.Member.TryGetNewTableRef(out _))
            {
                return _generator.NewEntityPrefix;
            }
        
            return memberExpression.Member.TryGetOldTableRef(out _)
                ? _generator.OldEntityPrefix
                : GetColumnSql(memberExpression.Expression.Type, memberExpression.Member, argumentType);
        }

        private string GetColumnSql(
            MemberExpression memberExpression,
            MemberInfo parentMember,
            VisitedMembers visitedMembers)
        {
            var argumentType = ArgumentType.Default;
            var memberType = memberExpression.Expression.Type;
        
            if (memberExpression.Member.TryGetNewTableRef(out var tableRefType))
            {
                memberType = tableRefType;
                argumentType = ArgumentType.New;
            }
            else if (memberExpression.Member.TryGetOldTableRef(out tableRefType))
            {
                memberType = tableRefType;
                argumentType = ArgumentType.Old;
            }
        
            visitedMembers.AddMember(argumentType, parentMember);
        
            return GetColumnSql(memberType, parentMember, argumentType);
        }

        private string GetColumnSql(Type? tableType, MemberInfo columnMember, ArgumentType argumentType)
        {
            if (argumentType is ArgumentType.New or ArgumentType.Old)
            {
                return _generator.GetColumnValueReferenceSql(
                    tableType,
                    columnMember,
                    argumentType);
            }

            return _generator.GetColumnSql(
                tableType!,
                columnMember,
                argumentType);
        }
    }
}