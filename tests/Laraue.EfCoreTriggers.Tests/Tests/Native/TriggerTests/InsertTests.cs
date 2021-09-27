using System;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests
{
    [IntegrationTest]
    [Collection("IntegrationTests")]
    public abstract class InsertTests : BaseTriggerTests
    {
        protected InsertTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory) : base(contextOptionsFactory)
        {
        }
        
        [Fact]
        public void InsertTrigger_ShouldInsertDataInNewTable_WhenDataInsertedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Insert(inserted => new DestinationEntity {StringField = "12"})));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity());

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal("12",saved.StringField);
        }
        
        [Fact]
        public void InsertTrigger_ShouldDeleteDataInNewTable_WhenDataInsertedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Delete<DestinationEntity>((inserted, destinationEntities) => inserted.StringField == destinationEntities.StringField)));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "abc"}, new DestinationEntity { StringField = "def"});
            dbContext.Save(new SourceEntity { StringField = "abc" });

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal("def",saved.StringField);
        }
        
        [Fact]
        public void InsertTrigger_ShouldUpdateDataInNewTable_WhenDataInsertedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Update<DestinationEntity>(
                            (inserted, destinationEntities) => inserted.StringField == destinationEntities.StringField,
                            (inserted, oldEntities) => new DestinationEntity { IntValue = oldEntities.IntValue + 10 })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "abc", IntValue = 1}, new DestinationEntity { StringField = "def", IntValue = 2});
            dbContext.Save(new SourceEntity { StringField = "abc" });

            var saved = dbContext.DestinationEntities.OrderBy(x => x.Id).ToArray();
            Assert.Equal(2, saved.Length);
            Assert.Equal(11, saved[0].IntValue);
            Assert.Equal(2, saved[1].IntValue);
        }
        
        [Fact]
        public void InsertTrigger_ShouldUpsertDataInNewTable_WhenDataInsertedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Upsert(
                            inserted => new DestinationEntity { UniqueIdentifier = inserted.IntValue },
                            inserted => new DestinationEntity { DecimalValue = inserted.DecimalValue, UniqueIdentifier = inserted.IntValue },
                            (inserted, oldEntity) => new DestinationEntity { DecimalValue = oldEntity.DecimalValue + inserted.DecimalValue })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 20 });

            var saved = Assert.Single(dbContext.DestinationEntities.ToArray());
            Assert.Equal(35, saved.DecimalValue);
        }
        
        [Fact]
        public void InsertTrigger_ShouldInsertIfNotExistsDataInNewTable_WhenDataInsertedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .InsertIfNotExists(
                            inserted => new DestinationEntity { UniqueIdentifier = inserted.IntValue },
                            inserted => new DestinationEntity { DecimalValue = inserted.DecimalValue, UniqueIdentifier = inserted.IntValue })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 20 });

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(15, saved.DecimalValue);
        }
        
        [Fact]
        public void InsertTrigger_ShouldExecuteAction_OnlyWhenConditionIsPassed()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Condition(inserted => inserted.IntValue > 10)
                        .Condition(inserted => inserted.IntValue < 20)
                        .Insert(inserted => new DestinationEntity())));

            using var dbContext = CreateDbContext(builder);
            
            dbContext.Save(new SourceEntity { IntValue = 9 });
            Assert.Empty(dbContext.DestinationEntities);
            
            dbContext.Save(new SourceEntity { IntValue = 21 });
            Assert.Empty(dbContext.DestinationEntities);
            
            dbContext.Save(new SourceEntity { IntValue = 12 });
            Assert.Single(dbContext.DestinationEntities);
        }
        
        [Fact]
        public void InsertTrigger_ShouldExecuteActionSequence()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterInsert(trigger => trigger
                    .Action(action => action
                        .Insert(inserted => new DestinationEntity { StringField = inserted.StringField + "Bob" }))
                    .Action(action => action
                        .Insert(inserted => new DestinationEntity { StringField = inserted.StringField + "John" })));

            using var dbContext = CreateDbContext(builder);
            
            dbContext.Save(new SourceEntity { StringField = "Hi, " });
            var result = dbContext.DestinationEntities.OrderBy(x => x.Id).ToArray();
            Assert.Equal(2, result.Length);
            Assert.Equal("Hi, Bob", result[0].StringField);
            Assert.Equal("Hi, John", result[1].StringField);
        }
    }
}