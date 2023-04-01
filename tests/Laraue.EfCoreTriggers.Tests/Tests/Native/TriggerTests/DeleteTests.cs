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
    public abstract class DeleteTests : BaseTriggerTests
    {
        protected DeleteTests(IContextOptionsFactory<DynamicDbContext> contextOptionsFactory)
            : base(contextOptionsFactory)
        {
        }
        
        [Fact]
        public void DeleteTrigger_ShouldInsertDataInNewTable_WhenDataDeletedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterDelete(trigger => trigger
                    .Action(action => action
                        .Insert(deleted => new DestinationEntity
                        {
                            StringField = deleted.Old.StringField
                        })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { StringField = "12" });
            dbContext.Delete(x => x.SourceEntities);

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal("12",saved.StringField);
        }
        
        [Fact]
        public void DeleteTrigger_ShouldDeleteDataInNewTable_WhenDataDeletedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterDelete(trigger => trigger
                    .Action(action => action
                        .Delete<DestinationEntity>((tableRefs, destinationEntities)
                            => tableRefs.Old.StringField == destinationEntities.StringField)));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "def"});
            dbContext.Save(new DestinationEntity { StringField = "abc"});
            dbContext.Save(new SourceEntity { StringField = "abc" });
            dbContext.Delete(x => x.SourceEntities);

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal("def",saved.StringField);
        }
        
        [Fact]
        public void DeleteTrigger_ShouldUpdateDataInNewTable_WhenDataDeletedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterDelete(trigger => trigger
                    .Action(action => action
                        .Update<DestinationEntity>(
                            (tableRefs, destinationEntities)
                                => tableRefs.Old.StringField == destinationEntities.StringField,
                            (deleted, destinationEntities) 
                                => new DestinationEntity { IntValue = destinationEntities.IntValue + 10 })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new DestinationEntity { StringField = "def", IntValue = 1});
            dbContext.Save(new DestinationEntity { StringField = "abc", IntValue = 1});
            dbContext.Save(new SourceEntity { StringField = "abc" });
            dbContext.Delete(x => x.SourceEntities);

            var saved = dbContext.DestinationEntities.OrderBy(x => x.Id).ToArray();
            Assert.Equal(2, saved.Length);
            Assert.Equal(1, saved[0].IntValue);
            Assert.Equal(11, saved[1].IntValue);
        }
        
        [Fact]
        public void DeleteTrigger_ShouldUpsertDataInNewTable_WhenDataDeletedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterDelete(trigger => trigger
                    .Action(action => action
                        .Upsert(
                            (tableRefs, entities)
                                => entities.UniqueIdentifier == tableRefs.Old.IntValue,
                            tableRefs
                                => new DestinationEntity
                                {
                                    DecimalValue = tableRefs.Old.DecimalValue,
                                    UniqueIdentifier = tableRefs.Old.IntValue
                                },
                            (tableRefs, destinationEntities)
                                => new DestinationEntity
                                {
                                    DecimalValue = destinationEntities.DecimalValue + tableRefs.Old.DecimalValue
                                })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 20 });
            dbContext.Delete(x => x.SourceEntities);

            var saved = Assert.Single(dbContext.DestinationEntities.ToArray());
            Assert.Equal(35, saved.DecimalValue);
        }
        
        [Fact]
        public void DeleteTrigger_ShouldInsertIfNotExistsDataInNewTable_WhenDataDeletedInOldTable()
        {
            Action<EntityTypeBuilder<SourceEntity>> builder = x =>
                x.AfterDelete(trigger => trigger
                    .Action(action => action
                        .InsertIfNotExists(
                            (tableRefs, entities)
                                => entities.UniqueIdentifier == tableRefs.Old.IntValue,
                            tableRefs
                                => new DestinationEntity
                                {
                                    DecimalValue = tableRefs.Old.DecimalValue,
                                    UniqueIdentifier = tableRefs.Old.IntValue
                                })));

            using var dbContext = CreateDbContext(builder);
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 15 });
            dbContext.Save(new SourceEntity { IntValue = 1, DecimalValue = 20 });
            dbContext.Delete(x => x.SourceEntities);

            var saved = Assert.Single(dbContext.DestinationEntities);
            Assert.Equal(15, saved.DecimalValue);
        }
    }
}