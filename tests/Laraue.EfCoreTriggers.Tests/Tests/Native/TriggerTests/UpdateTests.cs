using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests
{
    [IntegrationTest]
    [Collection("IntegrationTests")]
    public abstract class UpdateTests : BaseTriggerTests
    {
        protected UpdateTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
            : base(contextOptionsFactory)
        {
        }
        
        [Fact]
        public void UpdateTrigger_ShouldInsertDataInNewTable_WhenDataUpdateInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Insert(tableRefs
                            => new DestinationEntity
                            {
                                StringField = tableRefs.New.StringField + tableRefs.Old.StringField + "x"
                            })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { StringField = "ab" });
            dbContext.Update(x => x.SourceEntities, x => x.StringField = "dc");

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal("dcabx",saved.StringField);
        }
        
        [Fact]
        public void UpdateTrigger_ShouldDeleteDataInNewTable_WhenDataUpdatedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Delete<DestinationEntity>((tableRefs, entities)
                            => entities.StringField == tableRefs.New.StringField + tableRefs.Old.StringField)));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "dcab" });
            dbContext.Save(new SourceEntity { StringField = "ab" });
            dbContext.Update(x => x.SourceEntities, x => x.StringField = "dc");

            Assert.Empty(dbContext.DestinationEntities);
        }
        
        [Fact]
        public void UpdateTrigger_ShouldUpdateDataInNewTable_WhenDataUpdatedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Update<DestinationEntity>(
                            (tableRefs, destinationEntities) 
                                => destinationEntities.StringField == tableRefs.New.StringField + tableRefs.Old.StringField,
                            (tableRefs, destinationEntities) 
                                => new DestinationEntity
                                {
                                    IntValue = tableRefs.Old.IntValue + tableRefs.New.IntValue + 10
                                })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "abcdef", IntValue = 1});
            dbContext.Save(new SourceEntity { StringField = "def", IntValue = 2});

            dbContext.Update(x => x.SourceEntities, x =>
            {
                x.StringField = "abc";
                x.IntValue = 3;
            });

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(15, saved.IntValue);
        }
        
        [Fact]
        public void UpdateTrigger_ShouldUpsertDataInNewTable_WhenDataUpdatedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Upsert(
                            (tableRefs, entities)
                                => entities.UniqueIdentifier == tableRefs.Old.IntValue + tableRefs.New.IntValue,
                            tableRefs 
                                => new DestinationEntity
                                {
                                    DecimalValue = tableRefs.Old.DecimalValue + tableRefs.New.DecimalValue,
                                    UniqueIdentifier = tableRefs.Old.IntValue + tableRefs.New.IntValue
                                },
                            (tableRefs, oldEntity)
                                => new DestinationEntity
                                {
                                    DecimalValue = oldEntity.DecimalValue + tableRefs.Old.DecimalValue + tableRefs.New.DecimalValue
                                })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });
            
            dbContext.Update(x => x.SourceEntities, x =>
            {
                x.DecimalValue = 10;
                x.IntValue = 2;
            });

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(25, saved.DecimalValue);
            Assert.Equal(3, saved.UniqueIdentifier);
            
            dbContext.Update(x => x.SourceEntities, x =>
            {
                x.DecimalValue = 10;
                x.IntValue = 1;
            });
            
            saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(45, saved.DecimalValue);
        }
        
        [Fact]
        public void UpdateTrigger_ShouldInsertIfNotExistsDataInNewTable_WhenDataUpdatedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .InsertIfNotExists(
                            (tableRefs, entities) 
                                => entities.UniqueIdentifier == tableRefs.Old.IntValue + tableRefs.New.IntValue,
                            tableRefs
                                => new DestinationEntity
                                {
                                    DecimalValue = tableRefs.Old.DecimalValue + tableRefs.New.DecimalValue, 
                                    UniqueIdentifier = tableRefs.Old.IntValue + tableRefs.New.IntValue
                                })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });

            for (var i = 0; i < 2; i++)
            {
                dbContext.Update(x => x.SourceEntities, x =>
                {
                    x.DecimalValue = 10;
                    x.IntValue = 1;
                });
            }

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(2, saved.UniqueIdentifier);
            Assert.Equal(25, saved.DecimalValue);
        }
    }
}