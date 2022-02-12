﻿using System;
using System.Linq.Expressions;
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
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.PostgreSql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers PostgreSQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UsePostgreSqlTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
            where TContext : DbContext
        {
            TriggerExtensions.Services.AddPostgreSqlServices(modifyServices);
            
            return optionsBuilder.ReplaceMigrationsModelDiffer();
        }

        /// <summary>
        /// Add EF Core triggers PostgreSQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UsePostgreSqlTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
        {
            TriggerExtensions.Services.AddPostgreSqlServices(modifyServices);
            
            return optionsBuilder.ReplaceMigrationsModelDiffer();
        }

        /// <summary>
        /// Add EF Core triggers PostgreSQL provider services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modifyServices"></param>
        public static void AddPostgreSqlServices(
            this IServiceCollection services,
            Action<IServiceCollection> modifyServices = null)
        {
            services.AddDefaultServices()
                .AddSingleton<SqlTypeMappings, PostgreSqlTypeMappings>()
                .AddSingleton<ITriggerVisitor, PostgreSqlTriggerVisitor>()
                .AddExpressionVisitor<NewExpression, PostreSqlNewExpressionVisitor>()
                .AddTriggerActionVisitor<TriggerUpsertAction, TriggerUpsertActionVisitor>()
                .AddSingleton<IInsertExpressionVisitor, InsertExpressionVisitor>()
                .AddSingleton<ISqlGenerator, SqlGenerator>()
                .AddMethodCallConverter<ConcatStringViaConcatFuncVisitor>()
                .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
                .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
                .AddMethodCallConverter<StringTrimViaBtrimFuncVisitor>()
                .AddMethodCallConverter<StringContainsViaStrposFuncVisitor>()
                .AddMethodCallConverter<StringEndsWithViaDoubleVerticalLineVisitor>()
                .AddMethodCallConverter<StringIsNullOrEmptyVisitor>()
                .AddMethodCallConverter<MathAbsVisitor>()
                .AddMethodCallConverter<MathAcosVisitor>()
                .AddMethodCallConverter<MathAsinVisitor>()
                .AddMethodCallConverter<MathAtanVisitor>()
                .AddMethodCallConverter<MathAtan2Visitor>()
                .AddMethodCallConverter<MathCeilingVisitor>()
                .AddMethodCallConverter<MathCosVisitor>()
                .AddMethodCallConverter<MathExpVisitor>()
                .AddMethodCallConverter<MathFloorVisitor>();
            
            modifyServices?.Invoke(services);
        }
    }
}