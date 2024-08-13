using System;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Abs;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Acos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Asin;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Cos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Exp;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Floor;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.MySql.Converters.MethodCalls.Guid.NewGuid;
using Laraue.EfCoreTriggers.MySql.Converters.NewExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.MySql.Extensions
{
    /// <summary>
    /// Extensions to add MySql Triggers to the container.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers MySQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseMySqlTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddMySqlServices,  modifyServices);
        }
        
        /// <summary>
        /// Add EF Core triggers MySQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>A
        public static DbContextOptionsBuilder<TContext> UseMySqlTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddMySqlServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers MySQL provider services.
        /// </summary>
        public static void AddMySqlServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddDefaultServices()
                .AddScoped<SqlTypeMappings, MySqlTypeMappings>()
                .AddScoped<ITriggerVisitor, MySqlTriggerVisitor>()
                .AddTriggerActionVisitor<TriggerUpsertAction, MySqlTriggerUpsertActionVisitor>()
                .AddScoped<IInsertExpressionVisitor, MySqlInsertExpressionVisitor>()
                .AddScoped<ISqlGenerator, MySqlSqlGenerator>()
                .AddTriggerActionVisitor<TriggerActionsGroup, MySqlTriggerActionsGroupVisitor>()
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
}