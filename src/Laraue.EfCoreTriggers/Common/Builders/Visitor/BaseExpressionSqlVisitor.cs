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
    public abstract class BaseExpressionSqlVisitor
    {
        private Dictionary<MemberInfo, string> _columnNamesCache = new Dictionary<MemberInfo, string>();

        private Dictionary<Type, string> _tableNamesCache = new Dictionary<Type, string>();

        protected IModel Model { get; }

        protected abstract string NewEntityPrefix { get; }

        protected abstract string OldEntityPrefix { get; }

        protected abstract char Quote { get; }

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
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.IsTrue => "is true",
            ExpressionType.IsFalse => "is false",
            ExpressionType.Negate => "-",
            ExpressionType.Not => "is false",
            _ => throw new NotSupportedException($"Unknown sign of {expressionType}"),
        };

        public virtual string GetExpressionSql(Expression expression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            return expression switch
            {
                BinaryExpression binaryExpression => GetBinaryExpressionSql(binaryExpression, argumentTypes),
                MemberExpression memberExpression => GetMemberExpressionSql(memberExpression, argumentTypes),
                UnaryExpression unaryExpression => GetUnaryExpressionSql(unaryExpression, argumentTypes),
                ConstantExpression constantExpression => GetConstantExpressionSql(constantExpression),
                MethodCallExpression methodCallExpression => GetMethodCallExpressionSql(methodCallExpression, argumentTypes),
                _ => throw new NotSupportedException($"Expression of type {expression.GetHashCode()} for {expression} is not supported."),
            };
        }


        public virtual string GetMemberExpressionSql(MemberExpression memberExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            argumentTypes ??= new Dictionary<string, ArgumentPrefix>();
            var sqlBuilder = new StringBuilder();

            var memberName = ((ParameterExpression)memberExpression.Expression).Name;
            if (argumentTypes.TryGetValue(memberName, out var argumentType))
            {
                if (argumentType != ArgumentPrefix.None)
                {
                    if (argumentType == ArgumentPrefix.New)
                        sqlBuilder.Append(NewEntityPrefix);
                    else if (argumentType == ArgumentPrefix.Old)
                        sqlBuilder.Append(OldEntityPrefix);
                    sqlBuilder.Append('.');
                }
            }
            else
                sqlBuilder.Append($"{GetTableName(memberExpression.Member)}.");

            sqlBuilder.Append(GetColumnName(memberExpression.Member));

            return sqlBuilder.ToString();
        }

        public virtual (string ColumnName, string AssignmentExpressionSql) GetMemberAssignmentParts(MemberAssignment memberAssignment, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var columnName = GetColumnName(memberAssignment.Member);
            var assignmentExpressionSql = GetExpressionSql(memberAssignment.Expression, argumentTypes);
            return (columnName, assignmentExpressionSql);
        }

        public virtual string GetMethodCallExpressionSql(MethodCallExpression methodCallExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var parsedArguments = methodCallExpression.Arguments.Select(argumentExpression => GetExpressionSql(argumentExpression, argumentTypes)).ToArray();
            return methodCallExpression.Method.Name switch
            {
                "Concat" => GetMethodConcatCallExpressionSql(methodCallExpression, parsedArguments),
                "ToLower" => GetMethodToLowerCallExpressionSql(methodCallExpression, GetExpressionSql(methodCallExpression.Object, argumentTypes)),
                "ToUpper" => GetMethodToUpperCallExpressionSql(methodCallExpression, GetExpressionSql(methodCallExpression.Object, argumentTypes)),
                _ => throw new NotSupportedException($"Expression {methodCallExpression.Method.Name} is not supported"),
            };
        }

        public abstract string GetMethodConcatCallExpressionSql(MethodCallExpression methodCallExpression, params string[] concatExpressionArgsSql);

        public abstract string GetMethodToLowerCallExpressionSql(MethodCallExpression methodCallExpression, string lowerSqlExpression);

        public abstract string GetMethodToUpperCallExpressionSql(MethodCallExpression methodCallExpression, string upperSqlExpression);

        public virtual string GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            var leftSideExpressionTypes = new[] { ExpressionType.Negate };

            var sqlBuilder = new StringBuilder();
            var memberExpression = (MemberExpression)unaryExpression.Operand;
            if (leftSideExpressionTypes.Contains(unaryExpression.NodeType))
                sqlBuilder.Append(GetExpressionTypeSql(unaryExpression.NodeType));
            sqlBuilder.Append(GetMemberExpressionSql(memberExpression, argumentTypes));
            if (!leftSideExpressionTypes.Contains(unaryExpression.NodeType))
                sqlBuilder.Append($" {GetExpressionTypeSql(unaryExpression.NodeType)}");

            return sqlBuilder.ToString();
        }

        public virtual string[] GetNewExpressionColumns(NewExpression newExpression)
        {
            return newExpression.Arguments.Select(argument =>
            {
                var memberExpression = (MemberExpression)argument;
                var parameter = (ParameterExpression)memberExpression.Expression;
                return GetMemberExpressionSql(memberExpression, new Dictionary<string, ArgumentPrefix>
                {
                    [parameter.Name] = ArgumentPrefix.None,
                });
            }).ToArray();
        }

        public virtual string GetBinaryExpressionSql(BinaryExpression binaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            Expression[] GetBinaryExpressionParts()
            {
                var parts = new[] { binaryExpression.Left, binaryExpression.Right };
                if (binaryExpression.Method is null)
                {
                    if (binaryExpression.Left is MemberExpression leftMemberExpression)
                        parts[0] = Expression.IsTrue(binaryExpression.Left);
                    if (binaryExpression.Right is MemberExpression rightMemberExpression)
                        parts[1] = Expression.IsTrue(binaryExpression.Right);
                }
                return parts;
            };

            if (binaryExpression.Method?.Name == "Concat")
                return GetMethodCallExpressionSql(Expression.Call(null, binaryExpression.Method, binaryExpression.Left, binaryExpression.Right), argumentTypes);
            else
            {
                var binaryParts = GetBinaryExpressionParts().Select(part => GetExpressionSql(part, argumentTypes));
                return string.Join($" {GetExpressionTypeSql(binaryExpression.NodeType)} ", binaryParts);
            }
        }

        protected Dictionary<string, string> GetMemberInitExpressionAssignmentParts(MemberInitExpression memberInitExpression, Dictionary<string, ArgumentPrefix> argumentTypes)
        {
            return memberInitExpression.Bindings.Select(memberBinding =>
            {
                var memberAssignmentExpression = (MemberAssignment)memberBinding;
                return GetMemberAssignmentParts(memberAssignmentExpression, argumentTypes);
            }).ToDictionary(x => x.ColumnName, x => x.AssignmentExpressionSql);
        }

        public virtual string GetConstantExpressionSql(ConstantExpression constantExpression)
        {
            if (constantExpression.Value is string strValue)
                return $"{Quote}{strValue}{Quote}";
            else if (constantExpression.Value is Enum enumValue)
                return enumValue.ToString("D");
            return constantExpression.Value.ToString().ToLower();
        }
    }
}
