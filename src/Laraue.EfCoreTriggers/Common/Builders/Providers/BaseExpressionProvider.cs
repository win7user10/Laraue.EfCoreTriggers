﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace Laraue.EfCoreTriggers.Common.Builders.Providers
{
    public abstract class BaseExpressionProvider
    {
        /// <summary>
        /// Cached column names for entity properties.
        /// </summary>
        private readonly Dictionary<MemberInfo, string> _columnNamesCache = new Dictionary<MemberInfo, string>();

        /// <summary>
        /// Cached table names for entities.
        /// </summary>
        private readonly Dictionary<Type, string> _tableNamesCache = new Dictionary<Type, string>();

        /// <summary>
        /// Cached database schema names.
        /// </summary>
        private readonly Dictionary<Type, string> _tableSchemasCache = new Dictionary<Type, string>();

        /// <summary>
        /// Model used for generating SQL. From this model takes column names, table names and other meta information.
        /// </summary>
        protected IModel Model { get; }

        /// <summary>
        /// Prefix for inserted or updated (new value) entity in triggers. E.g to get balance from inserted entity, 
        /// inPostgreSQL should be used syntax NEW.balance.
        /// </summary>
        protected virtual string NewEntityPrefix { get; } = "NEW";

        /// <summary>
        /// Prefix for deleted or updated (old value) entity in triggers. E.g to get balance from deleted entity, 
        /// inPostgreSQL should be used syntax OLD.balance.
        /// </summary>
        protected virtual string OldEntityPrefix { get; } = "OLD";

        /// <summary>
        /// Quote in the database.
        /// </summary>
        protected virtual char Quote { get; } = '\'';

        /// <summary>
        /// Mappings between <see cref="Type">CLR Type</see> and SQL column types.
        /// Do not ask types directly from this property, use <see cref="GetSqlType"/> instead.
        /// </summary>
        protected abstract Dictionary<Type, string> TypeMappings { get; }

        /// <summary>
        /// Initializes new instance of <see cref="BaseExpressionProvider"/>.
        /// </summary>
        /// <param name="model"></param>
		public BaseExpressionProvider(IModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        /// <summary>
        /// Get column name for passed property type.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        protected virtual string GetColumnName(MemberInfo memberInfo)
        {
            if (!_columnNamesCache.ContainsKey(memberInfo))
            {
                var entityType = Model.FindEntityType(memberInfo.DeclaringType);
                var property = entityType.FindProperty(memberInfo.Name);
                var identifier = (StoreObjectIdentifier)StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                _columnNamesCache.Add(memberInfo, property.GetColumnName(identifier));
            }

            if (!_columnNamesCache.TryGetValue(memberInfo, out var columnName))
                throw new InvalidOperationException($"Column name for member {memberInfo.Name} is not defined in model");
            return columnName;
        }

        /// <summary>
        /// Get SQL type represents passed <see cref="Type">CLR Type</see>.
        /// Based on <see cref="TypeMappings"/> but uses additional logic and all types should be 
        /// asked through this method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetSqlType(Type type)
		{
            type = type.IsEnum ? typeof(Enum) : type;
            TypeMappings.TryGetValue(type, out var sqlType);
            return sqlType;
		}

        /// <summary>
        /// Get table name in which situated entity with passed property.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        protected virtual string GetTableName(MemberInfo memberInfo) => GetTableName(memberInfo.DeclaringType);

        /// <summary>
        /// Get table name for passed <see cref="Type">ClrType</see>.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual string GetTableName(Type entity)
        {
            if (!_tableNamesCache.ContainsKey(entity))
            {
                var entityType = Model.FindEntityType(entity);
                _tableNamesCache.Add(entity, entityType.GetTableName());
            }

            if (!_tableNamesCache.TryGetValue(entity, out var tableName))
			{
                throw new InvalidOperationException($"Table name for entity {entity.FullName} is not defined in model.");
            }

            var schemaName = GetTableSchemaName(entity);

            return string.IsNullOrWhiteSpace(schemaName)
                ? tableName
                : $"{schemaName}.{tableName}";
        }

        /// <summary>
        /// Get schema name for passed <see cref="Type">ClrType</see>.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual string GetTableSchemaName(Type entity)
        {
            if (!_tableSchemasCache.ContainsKey(entity))
            {
                var entityType = Model.FindEntityType(entity);
                _tableSchemasCache.Add(entity, entityType.GetSchema());
            }

            if (!_tableSchemasCache.TryGetValue(entity, out var schemaName))
            {
                throw new InvalidOperationException($"Schema for entity {entity.FullName} is not defined in model.");
            }

            return schemaName;
        }

        /// <summary>
        /// Get expression operand based on <see cref="ExpressionType"/> property.
        /// E.g. <see cref="ExpressionType.Add"/> will returns "+", <see cref="ExpressionType.Subtract"/> "-" e.t.c. 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual string GetExpressionOperandSql(Expression expression) => expression.NodeType switch
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
            _ => throw new NotSupportedException($"Unknown sign of {expression.NodeType}"),
        };

        /// <summary>
        /// Analyze, does passed <see cref="UnaryExpression"/> needs to cast into the Database.
        /// For example, casting of <see cref="Enum"/> values to <see cref="int"/> is not neccessary, 
        /// because each <see cref="Enum"></see> is stored as <see cref="int"/> in the Database.
        /// </summary>
        /// <param name="unaryExpression"></param>
        /// <returns></returns>
        protected virtual bool IsNeedConvertion(UnaryExpression unaryExpression)
		{
            var clrType1 = unaryExpression.Operand.Type;
            var clrType2 = unaryExpression.Type;
            var sqlType1 = GetSqlType(clrType1);
            var sqlType2 = GetSqlType(clrType2);
            return sqlType1 != sqlType2;
        }

        /// <summary>
        /// Generate SQL expression to cast passed <paramref name="member"/> to type represents in <paramref name="unaryExpression"/>.
        /// </summary>
        /// <param name="unaryExpression"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual string GetConvertExpressionSql(UnaryExpression unaryExpression, string member)
		{
            IsNeedConvertion(unaryExpression);

            var sqlType = GetSqlType(unaryExpression.Type);
            return sqlType is not null
                ? $"CAST({member} AS {sqlType})"
                : throw new NotSupportedException($"Converting of type {unaryExpression.Type} is not supported");
		}

        /// <summary>
        /// Get sql form passed <see cref="Expression"/> of any type.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="argumentTypes">Prefixes for column name, such as <see cref="OldEntityPrefix"/> or <see cref="NewEntityPrefix"/>.</param>
        /// <returns></returns>
        protected virtual SqlBuilder GetExpressionSql(Expression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return expression switch
            {
                BinaryExpression binaryExpression => GetBinaryExpressionSql(binaryExpression, argumentTypes),
                MemberExpression memberExpression => GetMemberExpressionSql(memberExpression, argumentTypes),
                UnaryExpression unaryExpression => GetUnaryExpressionSql(unaryExpression, argumentTypes),
                NewExpression newExpression => GetNewExpressionSql(newExpression),
                ConstantExpression constantExpression => GetConstantExpressionSql(constantExpression),
                MethodCallExpression methodCallExpression => GetMethodCallExpressionSql(methodCallExpression, argumentTypes),
                _ => throw new NotSupportedException($"Expression of type {expression.GetType()} for {expression} is not supported."),
            };
        }

        /// <summary>
        /// Get sql for new expressions, e.g. new <see cref="Guid"/>() will be translated to SQL provider specified function.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual SqlBuilder GetNewExpressionSql(NewExpression expression)
		{
            var mapping = new Dictionary<Type, Func<string>>
            {
                [typeof(Guid)] = GetNewGuidExpressionSql
            };

            if (mapping.TryGetValue(expression.Type, out var expressionGenerator))
			{
                var builder = new SqlBuilder(expressionGenerator.Invoke());
                return builder;
			}

            throw new NotImplementedException($"Sql creating value new{expression.Type}() is not supported.");
		}

        /// <summary>
        /// Get new <see cref="Guid"/> expression sql.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetNewGuidExpressionSql();

        protected virtual SqlBuilder GetMemberExpressionSql(MemberExpression memberExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            argumentTypes ??= new Dictionary<string, ArgumentType>();
            var parameterExpression = (ParameterExpression)memberExpression.Expression;
            var memberName = parameterExpression.Name;
            if (!argumentTypes.TryGetValue(memberName, out var argumentType))
                argumentType = ArgumentType.Default;
            return new SqlBuilder(memberExpression.Member, argumentType)
                .Append(GetMemberExpressionSql(memberExpression, argumentType));
        }

        protected virtual string GetMemberExpressionSql(MemberExpression memberExpression, ArgumentType argumentType)
        {
            return argumentType switch
            {
                ArgumentType.New => $"{NewEntityPrefix}.{GetColumnName(memberExpression.Member)}", 
                ArgumentType.Old => $"{OldEntityPrefix}.{GetColumnName(memberExpression.Member)}", 
                ArgumentType.None => GetColumnName(memberExpression.Member), 
                _ => $"{GetTableName(memberExpression.Member)}.{GetColumnName(memberExpression.Member)}",
            };
        }

        /// <summary>
        /// Get from <see cref="MemberAssignment"/> mapping <see cref="MemberInfo"/> -> assignment SQL.
        /// </summary>
        /// <param name="memberAssignment"></param>
        /// <param name="argumentTypes"></param>
        /// <returns></returns>
        protected virtual (MemberInfo MemberInfo, SqlBuilder AssignmentSqlResult) GetMemberAssignmentParts(
            MemberAssignment memberAssignment, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlExtendedResult = GetExpressionSql(memberAssignment.Expression, argumentTypes);
            return (memberAssignment.Member, sqlExtendedResult);
        }

        protected virtual SqlBuilder GetMethodCallExpressionSql(MethodCallExpression methodCallExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var parsedArguments = methodCallExpression.Arguments.Select(argumentExpression => GetExpressionSql(argumentExpression, argumentTypes)).ToArray();
            return methodCallExpression.Method.Name switch
            {
                "Concat" => GetMethodConcatCallExpressionSql(parsedArguments),
                "ToLower" => GetMethodToLowerCallExpressionSql(GetExpressionSql(methodCallExpression.Object, argumentTypes)),
                "ToUpper" => GetMethodToUpperCallExpressionSql(GetExpressionSql(methodCallExpression.Object, argumentTypes)),
                _ => throw new NotSupportedException($"Expression {methodCallExpression.Method.Name} is not supported"),
            };
        }

        protected virtual SqlBuilder GetMethodConcatCallExpressionSql(params SqlBuilder[] concatExpressionArgsSql)
            => new SqlBuilder(concatExpressionArgsSql)
                .AppendJoin(" + ", concatExpressionArgsSql.Select(x => x.StringBuilder));

        protected virtual SqlBuilder GetMethodToLowerCallExpressionSql(SqlBuilder lowerSqlExpression)
            => new SqlBuilder(lowerSqlExpression.AffectedColumns, $"LOWER({lowerSqlExpression})");

        protected virtual SqlBuilder GetMethodToUpperCallExpressionSql(SqlBuilder upperSqlExpression)
            => new SqlBuilder(upperSqlExpression.AffectedColumns, $"UPPER({upperSqlExpression})");

        protected virtual SqlBuilder GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlBuilder = new SqlBuilder();

            var internalSql = unaryExpression.Operand switch
            {
                MemberExpression memberExpression => GetMemberExpressionSql(memberExpression, argumentTypes),
                UnaryExpression internalUnaryExpression => GetUnaryExpressionSql(internalUnaryExpression, argumentTypes),
                _ => throw new NotSupportedException($"Operand {unaryExpression.Operand.Type} is not supported for UnaryExpression")
            };

            sqlBuilder.MergeColumnsInfo(internalSql.AffectedColumns);
            sqlBuilder.Append(GetUnaryExpressionSql(unaryExpression, internalSql.Sql));

            return sqlBuilder;
        }


        protected virtual string GetUnaryExpressionSql(Expression expression, string member)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                var unaryExpression = expression as UnaryExpression;
                return IsNeedConvertion(unaryExpression)
                    ? GetConvertExpressionSql(unaryExpression, member)
                    : member;
            }
            var operand = GetExpressionOperandSql(expression);
            if (expression.NodeType == ExpressionType.Negate)
            {
                return $"{operand}{member}";
            }
            else
            {
                return $"{member} {operand}";
            }
        }

        protected virtual SqlBuilder[] GetNewExpressionColumnsSql(NewExpression newExpression, Dictionary<string, ArgumentType> argumentTypes)
            => newExpression.Arguments.Select(argument => GetMemberExpressionSql((MemberExpression)argument, argumentTypes)).ToArray();

        protected virtual SqlBuilder GetBinaryExpressionSql(BinaryExpression binaryExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            Expression[] GetBinaryExpressionParts()
            {
                var parts = new[] { binaryExpression.Left, binaryExpression.Right };
                if (binaryExpression.Method is null)
                {
                    if (binaryExpression.Left is MemberExpression leftMemberExpression && leftMemberExpression.Type == typeof(bool))
                        parts[0] = Expression.IsTrue(binaryExpression.Left);
                    if (binaryExpression.Right is MemberExpression rightMemberExpression && rightMemberExpression.Type == typeof(bool))
                        parts[1] = Expression.IsTrue(binaryExpression.Right);
                }
                return parts;
            };

            if (binaryExpression.Method?.Name == "Concat")
            {
                return GetMethodCallExpressionSql(Expression.Call(null, binaryExpression.Method, binaryExpression.Left, binaryExpression.Right), argumentTypes);
            }
            else
            {
                var binaryParts = GetBinaryExpressionParts().Select(part => GetExpressionSql(part, argumentTypes));
                return new SqlBuilder(binaryParts)
                    .AppendJoin($" {GetExpressionOperandSql(binaryExpression)} ", binaryParts.Select(x => x.StringBuilder));
            }
        }

        /// <summary>
        /// Get from <see cref="MemberInitExpression"/> mapping <see cref="MemberInfo"/> -> generated SQL.
        /// </summary>
        /// <param name="memberInitExpression"></param>
        /// <param name="argumentTypes"></param>
        /// <returns></returns>
        protected Dictionary<MemberInfo, SqlBuilder> GetMemberInitExpressionAssignmentParts(MemberInitExpression memberInitExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return memberInitExpression.Bindings.Select(memberBinding =>
            {
                var memberAssignmentExpression = (MemberAssignment)memberBinding;
                return GetMemberAssignmentParts(memberAssignmentExpression, argumentTypes);
            }).ToDictionary(x => x.MemberInfo, x => x.AssignmentSqlResult);
        }

        /// <summary>
        /// Get sql for passed <see cref="ConstantExpression"/> values e.g. <para>5</para><para>"string"</para><para>Enum.Value</para>
        /// </summary>
        /// <param name="constantExpression"></param>
        /// <returns></returns>
        protected virtual SqlBuilder GetConstantExpressionSql(ConstantExpression constantExpression)
        {
            switch (constantExpression.Value)
			{
                case string strValue:
                    return new SqlBuilder(GetStringSqlValue(strValue));
                case Enum enumValue:
                    return new SqlBuilder(GetEnumSqlValue(enumValue));
                case bool boolValue:
                    return new SqlBuilder(GetBooleanSqlValue(boolValue));
                default:
                    return new SqlBuilder(constantExpression.Value.ToString().ToLower());
            }
        }

        /// <summary>
        /// Get sql for passed <see cref="Enum"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetEnumSqlValue(Enum value) => value.ToString("D");

        /// <summary>
        /// Get sql for passed <see cref="string"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetStringSqlValue(string value) => $"{Quote}{value}{Quote}";

        /// <summary>
        /// Get sql for passed <see cref="bool"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetBooleanSqlValue(bool value) => $"{value.ToString().ToLower()}";
    }
}
