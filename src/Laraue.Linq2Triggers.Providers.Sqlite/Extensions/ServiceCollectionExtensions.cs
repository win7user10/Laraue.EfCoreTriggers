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
using Laraue.Linq2Triggers.Providers.Sqlite.Converters.MethodCalls.Guid.NewGuid;
using Laraue.Linq2Triggers.Providers.Sqlite.Converters.NewExpression;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Linq2Triggers.Providers.Sqlite.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add EF Core triggers MySQL provider services.
    /// </summary>
    public static void AddBaseSqliteServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddDefaultServices()
            .AddScoped<SqlTypeMappings, SqliteTypeMappings>()
            .AddScoped<ITriggerVisitor, SqliteTriggerVisitor>()
            .AddTriggerActionVisitor<TriggerUpsertAction, TriggerUpsertActionVisitor>()
            .AddScoped<IInsertExpressionVisitor, SqliteInsertExpressionVisitor>()
            .AddScoped<ISqlGenerator, SqlGenerator>()
            .AddTriggerActionVisitor<TriggerActionsGroup, SqliteTriggerActionsGroupVisitor>()
            .AddMethodCallConverter<ConcatStringViaDoubleVerticalLineVisitor>()
            .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
            .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
            .AddMethodCallConverter<StringTrimViaTrimFuncVisitor>()
            .AddMethodCallConverter<StringContainsViaInstrFuncVisitor>()
            .AddMethodCallConverter<StringEndsWithViaDoubleVerticalLineVisitor>()
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
            .AddMethodCallConverter<NewGuidVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTime.UtcNowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTime.NowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.UtcNowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.NowVisitor>()
            .AddNewExpressionConverter<NewDateTimeSqliteExpressionVisitor>()
            .AddNewExpressionConverter<NewDateTimeOffsetSqliteExpressionVisitor>();
    }
}