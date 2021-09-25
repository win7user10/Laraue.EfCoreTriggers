using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public abstract class BaseExpressionProvider : EfCoreMetadataRetriever
    {
        /// <summary>
        /// Prefix for inserted or updated (new value) entity in triggers. E.g to get balance from inserted entity, 
        /// inPostgreSQL should be used syntax NEW.balance.
        /// </summary>
        protected virtual string NewEntityPrefix => "NEW";

        /// <summary>
        /// Prefix for deleted or updated (old value) entity in triggers. E.g to get balance from deleted entity, 
        /// inPostgreSQL should be used syntax OLD.balance.
        /// </summary>
        protected virtual string OldEntityPrefix => "OLD";

        /// <summary>
        /// Quote in the database.
        /// </summary>
        protected virtual char Quote => '\'';

        /// <summary>
        /// Mappings between <see cref="Type">CLR Type</see> and SQL column types.
        /// Do not ask types directly from this property, use <see cref="GetSqlType"/> instead.
        /// </summary>
        protected abstract Dictionary<Type, string> TypeMappings { get; }

        /// <summary>
        /// Get SQL type represents passed <see cref="Type">CLR Type</see>.
        /// Based on <see cref="TypeMappings"/> but uses additional logic and all types should be 
        /// asked through this method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetSqlType(Type type)
        {
            type = GetNotNullableType(type);
            type = type.IsEnum ? typeof(Enum) : type;
            TypeMappings.TryGetValue(type, out var sqlType);
            return sqlType;
		}
        
        protected Type GetNotNullableType(Type type)
        {
            var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
            return nullableUnderlyingType ?? type;
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
        public virtual SqlBuilder GetExpressionSql(Expression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return expression switch
            {
                BinaryExpression binaryExpression => GetBinaryExpressionSql(binaryExpression, argumentTypes),
                MemberExpression memberExpression => GetMemberExpressionSql(memberExpression, argumentTypes),
                UnaryExpression unaryExpression => GetUnaryExpressionSql(unaryExpression, argumentTypes),
                NewExpression newExpression => GetNewExpressionSql(newExpression),
                ConstantExpression constantExpression => GetConstantExpressionSql(constantExpression),
                MethodCallExpression methodCallExpression => GetMethodCallExpressionSql(methodCallExpression, argumentTypes),
                null => throw new ArgumentNullException(nameof(expression)),
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
            return GetColumnSql(memberExpression.Member, argumentType);
        }
        
        protected virtual string GetColumnSql(MemberInfo memberInfo, ArgumentType argumentType)
        {
            return argumentType switch
            {
                ArgumentType.New => $"{NewEntityPrefix}.{GetColumnName(memberInfo)}", 
                ArgumentType.Old => $"{OldEntityPrefix}.{GetColumnName(memberInfo)}", 
                ArgumentType.None => GetColumnName(memberInfo), 
                _ => $"{GetTableName(memberInfo)}.{GetColumnName(memberInfo)}",
            };
        }

        /// <summary>
        /// Get from <see cref="MemberAssignment"/> mapping <see cref="MemberInfo"/> -> assignment SQL.
        /// </summary>
        /// <param name="memberAssignment"></param>
        /// <param name="argumentTypes"></param>
        /// <returns></returns>
        protected virtual (MemberAssignment MemberAssignment, SqlBuilder AssignmentSqlResult) GetMemberAssignmentParts(
            MemberAssignment memberAssignment, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlExtendedResult = GetExpressionSql(memberAssignment.Expression, argumentTypes);
            return (memberAssignment, sqlExtendedResult);
        }

        protected virtual SqlBuilder GetMethodCallExpressionSql(MethodCallExpression methodCallExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            foreach (var converter in Converters.ExpressionCallConverters)
            {
                if (converter.IsApplicable(methodCallExpression))
                {
                    return converter.BuildSql(this, methodCallExpression, argumentTypes);
                }
            }

            throw new NotSupportedException($"Expression {methodCallExpression.Method.Name} is not supported");
        }

        protected virtual SqlBuilder GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var internalExpressionSql = GetExpressionSql(unaryExpression.Operand, argumentTypes);
            var sqlBuilder = new SqlBuilder(internalExpressionSql.AffectedColumns);
            
            if (unaryExpression.NodeType == ExpressionType.Convert)
            {
                if (IsNeedConvertion(unaryExpression))
                {
                    sqlBuilder.Append(GetConvertExpressionSql(unaryExpression, internalExpressionSql));
                }
                else
                {
                    sqlBuilder = internalExpressionSql;
                }

                return sqlBuilder;
            }
            
            var operand = GetExpressionOperandSql(unaryExpression);
            
            var sql = unaryExpression.NodeType == ExpressionType.Negate 
                ? $"{operand}{internalExpressionSql}" 
                : $"{internalExpressionSql} {operand}";

            sqlBuilder.Append(sql);
            
            return sqlBuilder;
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

            if (binaryExpression.Method?.Name == nameof(string.Concat))
            {
                return GetMethodCallExpressionSql(Expression.Call(null, binaryExpression.Method, binaryExpression.Left, binaryExpression.Right), argumentTypes);
            }

            var binaryExpressionParts = GetBinaryExpressionParts();

            // Check, if one argument is null, should be generated expression "value IS NULL"
            if (binaryExpression.NodeType is ExpressionType.Equal || binaryExpression.NodeType is ExpressionType.NotEqual)
            {
                if (binaryExpressionParts.Any(x => x is ConstantExpression constExpr && constExpr.Value == null))
                {
                    var secondArgument = binaryExpressionParts.First(x => x is ConstantExpression constExpr && constExpr.Value == null);
                    var firstArgument = binaryExpressionParts.Except(new[] { secondArgument }).First();
                    var argumentsSql = new[] { firstArgument, secondArgument }.Select(part => GetExpressionSql(part, argumentTypes)).ToArray();
                    return new SqlBuilder(argumentsSql)
                        .Append(argumentsSql[0].StringBuilder)
                        .Append(" IS NULL");
                }
            }

            var binaryParts = binaryExpressionParts.Select(part => GetExpressionSql(part, argumentTypes)).ToArray();

            return new SqlBuilder(binaryParts)
                .AppendJoin($" {GetExpressionOperandSql(binaryExpression)} ", binaryParts.Select(x => x.StringBuilder));
        }

        /// <summary>
        /// Get from <see cref="MemberInitExpression"/> mapping <see cref="MemberInfo"/> -> generated SQL.
        /// </summary>
        /// <param name="memberInitExpression"></param>
        /// <param name="argumentTypes"></param>
        /// <returns></returns>
        protected Dictionary<MemberAssignment, SqlBuilder> GetMemberInitExpressionAssignmentParts(MemberInitExpression memberInitExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return memberInitExpression.Bindings.Select(memberBinding =>
            {
                var memberAssignmentExpression = (MemberAssignment)memberBinding;
                return GetMemberAssignmentParts(memberAssignmentExpression, argumentTypes);
            }).ToDictionary(x => x.MemberAssignment, x => x.AssignmentSqlResult);
        }
        
        protected Dictionary<MemberExpression, SqlBuilder> GetNewExpressionAssignmentParts(NewExpression newExpression, Dictionary<string, ArgumentType> argumentTypes)
        {
            return newExpression.Arguments.ToDictionary(
                argument => (MemberExpression)argument,
                argument => GetExpressionSql(argument, argumentTypes));
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
                case null:
                    return new SqlBuilder("NULL");
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

        public AvailableConverters Converters { get; } = new ();

        /// <inheritdoc />
        protected BaseExpressionProvider(IModel model) : base(model)
        {
        }

        public void AddConverter(IMethodCallConverter converter)
        {
            Converters.ExpressionCallConverters.Push(converter);
        }
    }
}
