using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Abs;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Acos;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Asin;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Atan;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Atan2;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Ceiling;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Cos;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Exp;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.Math.Floor;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.Concat;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.Contains;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.EndsWith;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.IsNullOrEmpty;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.ToLower;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.ToUpper;
using Laraue.Linq2Triggers.Core.Converters.MethodCall.String.Trim;
using Laraue.Linq2Triggers.Core.Extensions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors.Statements;
using Laraue.Linq2Triggers.Providers.Oracle.Converters.MemberAccess.DateTime;
using Laraue.Linq2Triggers.Providers.Oracle.Converters.MemberAccess.DateTimeOffset;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Linq2Triggers.Providers.Oracle.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add EF Core triggers Oracle provider services.
    /// </summary>
    public static void AddBaseOracleServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddDefaultServices()
            .AddExpressionVisitor<UnaryExpression, OracleUnaryExpressionVisitor>()
            .AddScoped<SqlTypeMappings, OracleTypeMappings>()
            .AddScoped<ITriggerVisitor, OracleTriggerVisitor>()
            .AddTriggerActionVisitor<TriggerUpsertAction, OracleTriggerUpsertActionVisitor>()
            .AddScoped<IInsertExpressionVisitor, OracleInsertExpressionVisitor>()
            .AddScoped<ISqlGenerator, OracleSqlGenerator>()
            .AddTriggerActionVisitor<TriggerActionsGroup, OracleTriggerActionsGroupVisitor>()
            .AddMethodCallConverter<ConcatStringViaConcatFuncVisitor>()
            .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
            .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
            .AddMethodCallConverter<StringTrimViaTrimFuncVisitor>()
            .AddMethodCallConverter<StringContainsViaInstrFuncVisitor>()
            .AddMethodCallConverter<StringEndsWithViaConcatFuncVisitor>()
            .AddMethodCallConverter<StringIsNullOrEmptyVisitor>()
            .AddMethodCallConverter<MathAbsVisitor>()
            .AddMethodCallConverter<MathAcosVisitor>()
            .AddMethodCallConverter<MathAsinVisitor>()
            .AddMethodCallConverter<MathAtanVisitor>()
            .AddMethodCallConverter<MathAtan2Visitor>()
            .AddMethodCallConverter<MathCeilVisitor>()
            .AddMethodCallConverter<MathCosVisitor>()
            .AddMethodCallConverter<MathExpVisitor>()
            .AddMethodCallConverter<MathFloorVisitor>()
            .AddMemberAccessConverter<DateTimeNowVisitor>()
            .AddMemberAccessConverter<DateTimeUtcNowVisitor>()
            .AddMemberAccessConverter<DateTimeOffsetNowVisitor>()
            .AddMemberAccessConverter<DateTimeOffsetUtcNowVisitor>();
    }
}