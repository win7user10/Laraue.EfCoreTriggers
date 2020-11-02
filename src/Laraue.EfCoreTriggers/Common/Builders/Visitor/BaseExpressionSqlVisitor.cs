using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public abstract class BaseExpressionSqlVisitor : IExpressionSqlVisitor
    {
        private Dictionary<MemberInfo, string> _columnNamesCache = new Dictionary<MemberInfo, string>();

        private Dictionary<Type, string> _tableNamesCache = new Dictionary<Type, string>();

        protected IModel Model { get; }

        protected abstract string NewEntityPrefix { get; }

        protected abstract string OldEntityPrefix { get; }

        public BaseExpressionSqlVisitor(IModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public string GetColumnName(MemberInfo memberInfo)
        {
            if (!_columnNamesCache.ContainsKey(memberInfo))
            {
                var entityType = Model.FindEntityType(memberInfo.DeclaringType);
                _columnNamesCache.Add(memberInfo, entityType.GetProperty(memberInfo.Name).GetColumnName());
            }
            if (!_columnNamesCache.TryGetValue(memberInfo, out var columnName))
                throw new InvalidOperationException($"Column name for member {memberInfo.Name} is not defined in model");
            return columnName;
        }

        public string GetTableName(MemberInfo memberInfo) => GetTableName(memberInfo.DeclaringType);

        public string GetTableName(Type entity)
        {
            if (!_tableNamesCache.ContainsKey(entity))
            {
                var entityType = Model.FindEntityType(entity);
                _tableNamesCache.Add(entity, entityType.GetTableName());
            }
            if (!_tableNamesCache.TryGetValue(entity, out var columnName))
                throw new InvalidOperationException($"Table name for entity {entity.FullName} is not defined in model.");
            return columnName;
        }

        public virtual string GetExpressionTypeSql(ExpressionType expressionType) => expressionType switch
        {
            ExpressionType.Add => "+",
            ExpressionType.Subtract => "-",
            ExpressionType.Multiply => "*",
            ExpressionType.Divide => "/",
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.AndAlso => "&&",
            ExpressionType.OrElse => "||",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.IsTrue => "is true",
            ExpressionType.IsFalse => "is false",
            _ => throw new NotSupportedException($"Unknown sign of {expressionType}"),
        };

        public virtual string GetMemberExpressionSql(MemberExpression memberExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var sqlBuilder = new StringBuilder();

            var memberName = ((ParameterExpression)memberExpression.Expression).Name;
            if (argumentTypes.TryGetValue(memberName, out var argumentType))
            {
                if (argumentType == ArgumentPrefix.New)
                    sqlBuilder.Append(NewEntityPrefix);
                else if (argumentType == ArgumentPrefix.Old)
                    sqlBuilder.Append(OldEntityPrefix);
            }
            else
                sqlBuilder.Append(GetTableName(memberExpression.Member));

            sqlBuilder.Append('.')
                .Append(GetColumnName(memberExpression.Member));

            return sqlBuilder.ToString();
        }

        public virtual string GetMemberAssignmentSql(MemberAssignment memberAssignment, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var sqlBuilder = new StringBuilder();

            sqlBuilder.Append(GetColumnName(memberAssignment.Member))
                .Append(" = ");

            var assignmentExpression = (BinaryExpression)memberAssignment.Expression;
            var assignmentExpressionSql = GetBinaryExpressionSql(assignmentExpression, argumentTypes);
            sqlBuilder.Append(assignmentExpressionSql);

            return sqlBuilder.ToString();
        }

        public virtual string GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var sqlBuilder = new StringBuilder();
            var memberExpression = (MemberExpression)unaryExpression.Operand;
            sqlBuilder.Append(GetMemberExpressionSql(memberExpression, argumentTypes));
            sqlBuilder.Append($" {GetExpressionTypeSql(unaryExpression.NodeType)}");
            return sqlBuilder.ToString();
        }

        public virtual string GetBinaryExpressionSql(BinaryExpression binaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var sqlBuilder = new StringBuilder();

            void AddBinarySeparator()
            {
                sqlBuilder.Append($" {GetExpressionTypeSql(binaryExpression.NodeType)} ");
            }

            var parts = new[] { binaryExpression.Left, binaryExpression.Right };

            // Correct transform expressions like x => x.IsGood && x.IsSmth to boolean x.IsGood = true && x.IsSmth = true 
            if (parts[0] is MemberExpression leftMemberExpression && parts[1] is MemberExpression rightMemberExpression
                && ((PropertyInfo)leftMemberExpression.Member).PropertyType == typeof(bool)
                && ((PropertyInfo)rightMemberExpression.Member).PropertyType == typeof(bool))
            {
                sqlBuilder.Append(GetUnaryExpressionSql(Expression.IsTrue(leftMemberExpression), argumentTypes));
                AddBinarySeparator();
                sqlBuilder.Append(GetUnaryExpressionSql(Expression.IsTrue(rightMemberExpression), argumentTypes));
            }
            else
            {
                foreach (var part in parts)
                {
                    if (part is MemberExpression memberExpression)
                        sqlBuilder.Append(GetMemberExpressionSql(memberExpression, argumentTypes));
                    else if (part is ConstantExpression constantExpression)
                        sqlBuilder.Append(GetConstantExpressionSql(constantExpression));
                    else if (part is BinaryExpression binaryExp)
                        sqlBuilder.Append(GetBinaryExpressionSql(binaryExp, argumentTypes));
                    else
                        throw new InvalidOperationException($"{part.GetType()} expression does not supports in set statement.");

                    if (part != binaryExpression.Right) AddBinarySeparator();
                }
            }

            
            return sqlBuilder.ToString();
        }

        public virtual string GetMemberInitSql(MemberInitExpression memberInitExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("set ");
            var setExpressionBindings = memberInitExpression.Bindings;
            foreach (var memberBinding in setExpressionBindings)
            {
                var memberAssignmentExpression = (MemberAssignment)memberBinding;
                var sql = GetMemberAssignmentSql(memberAssignmentExpression, argumentTypes);
                sqlBuilder.Append(sql);

                if (memberBinding != setExpressionBindings.Last())
                    sqlBuilder.Append(", ");
            }

            return sqlBuilder.ToString();
        }

        public virtual string GetConstantExpressionSql(ConstantExpression constantExpression) => constantExpression.Value.ToString().ToLower();
    }
}
