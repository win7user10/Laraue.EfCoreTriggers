using System;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using LinqToDB;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests
{
    [IntegrationTest]
    [Collection("IntegrationTests")]
    public abstract class UpdateTests : BaseTriggerTests
    {
        protected UpdateTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory) : base(contextOptionsFactory)
        {
        }
        
        [Fact]
        public void UpdateTrigger_ShouldInsertDataInNewTable_WhenDataUpdateInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterUpdate(trigger => trigger
                    .Action(action => action
                        .Insert((entityBeforeUpdate, updatedEntity) => new DestinationEntity { StringField = updatedEntity.StringField + entityBeforeUpdate.StringField + "x" })));

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
                        .Delete<DestinationEntity>((entityBeforeUpdate, updatedEntity, destinationEntities)
                            => destinationEntities.StringField == updatedEntity.StringField + entityBeforeUpdate.StringField)));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity() { StringField = "dcab" });
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
                            (entityBeforeUpdate, updatedEntity, destinationEntities) 
                                => destinationEntities.StringField == updatedEntity.StringField + entityBeforeUpdate.StringField,
                            (entityBeforeUpdate, updatedEntity, destinationEntities) 
                                => new DestinationEntity { IntValue = entityBeforeUpdate.IntValue + updatedEntity.IntValue + 10 })));

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
                            (entityBeforeUpdate, updatedEntity) 
                                => new DestinationEntity { UniqueIdentifier = entityBeforeUpdate.IntValue + updatedEntity.IntValue },
                            (entityBeforeUpdate, updatedEntity) 
                                => new DestinationEntity
                                {
                                    DecimalValue = entityBeforeUpdate.DecimalValue + updatedEntity.DecimalValue,
                                    UniqueIdentifier = entityBeforeUpdate.IntValue + updatedEntity.IntValue
                                },
                            (entityBeforeUpdate, updatedEntity, oldEntity)
                                => new DestinationEntity
                                {
                                    DecimalValue = oldEntity.DecimalValue + entityBeforeUpdate.DecimalValue + updatedEntity.DecimalValue
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
                            (entityBeforeUpdate, updatedEntity) 
                                => new DestinationEntity { UniqueIdentifier = entityBeforeUpdate.IntValue + updatedEntity.IntValue },
                            (entityBeforeUpdate, updatedEntity) 
                                => new DestinationEntity
                                {
                                    DecimalValue = entityBeforeUpdate.DecimalValue + updatedEntity.DecimalValue, 
                                    UniqueIdentifier = entityBeforeUpdate.IntValue + updatedEntity.IntValue
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