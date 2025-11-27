using System.Linq.Expressions;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Abs;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Acos;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Asin;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Atan;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Atan2;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Ceiling;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Cos;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Exp;
using Laraue.Triggers.Core.Converters.MethodCall.Math.Floor;
using Laraue.Triggers.Core.Converters.MethodCall.String.Concat;
using Laraue.Triggers.Core.Converters.MethodCall.String.Contains;
using Laraue.Triggers.Core.Converters.MethodCall.String.EndsWith;
using Laraue.Triggers.Core.Converters.MethodCall.String.IsNullOrEmpty;
using Laraue.Triggers.Core.Converters.MethodCall.String.ToLower;
using Laraue.Triggers.Core.Converters.MethodCall.String.ToUpper;
using Laraue.Triggers.Core.Converters.MethodCall.String.Trim;
using Laraue.Triggers.Core.Extensions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.TriggerBuilders.Actions;
using Laraue.Triggers.Core.Visitors.TriggerVisitors;
using Laraue.Triggers.Core.Visitors.TriggerVisitors.Statements;
using Laraue.Triggers.SqlServer.Converters.MethodCalls.Guid.NewGuid;
using Laraue.Triggers.SqlServer.Converters.NewExpression;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Triggers.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add EF Core triggers MySQL provider services.
    /// </summary>
    public static void AddBaseSqlServerServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddDefaultServices()
            .AddScoped<SqlTypeMappings, SqlServerTypeMappings>()
            .AddExpressionVisitor<UnaryExpression, SqlServerUnaryExpressionVisitor>()
            .AddScoped<IInsertExpressionVisitor, InsertExpressionVisitor>()
            .AddTriggerActionVisitor<TriggerUpsertAction, SqlServerTriggerUpsertActionVisitor>()
            .AddScoped<ISqlGenerator, SqlServerSqlGenerator>()
            .AddScoped<ITriggerVisitor, SqlServerTriggerVisitor>()
            .AddTriggerActionVisitor<TriggerActionsGroup, SqlServerTriggerActionsGroupVisitor>()
            .AddMethodCallConverter<ConcatStringViaPlusVisitor>()
            .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
            .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
            .AddMethodCallConverter<StringTrimViaLtrimRtrimFuncVisitor>()
            .AddMethodCallConverter<StringContainsViaCharindexFuncVisitor>()
            .AddMethodCallConverter<StringEndsWithViaPlusVisitor>()
            .AddMethodCallConverter<StringIsNullOrEmptyVisitor>()
            .AddMethodCallConverter<MathAbsVisitor>()
            .AddMethodCallConverter<MathAcosVisitor>()
            .AddMethodCallConverter<MathAsinVisitor>()
            .AddMethodCallConverter<MathAtanVisitor>()
            .AddMethodCallConverter<MathAtn2Visitor>()
            .AddMethodCallConverter<MathCeilingVisitor>()
            .AddMethodCallConverter<MathCosVisitor>()
            .AddMethodCallConverter<MathExpVisitor>()
            .AddMethodCallConverter<MathFloorVisitor>()
            .AddMethodCallConverter<NewGuidVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTime.UtcNowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTime.NowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.UtcNowVisitor>()
            .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.NowVisitor>()
            .AddNewExpressionConverter<NewDateTimeExpressionVisitor>()
            .AddNewExpressionConverter<NewDateTimeOffsetExpressionVisitor>();
    }
}