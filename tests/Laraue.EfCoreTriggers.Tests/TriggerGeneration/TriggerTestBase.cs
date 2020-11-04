using Laraue.EfCoreTriggers.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.Tests.TriggerGeneration
{
    public class TriggerTestBase
    {
        protected readonly TestDbContext DbContext;

        public TriggerTestBase()
        {
            DbContext = new ContextFactory().CreatePgDbContext();
        }

        protected string GetAnnotationName<T>(TriggerTime triggerTime, TriggerType triggerType)
            => $"{Constants.AnnotationKey}_{triggerTime.ToString().ToUpper()}_{triggerType.ToString().ToUpper()}_{typeof(T).Name.ToUpper()}";

        protected string GetAnnotationSqlFromDbContext<T>(TriggerTime triggerTime, TriggerType triggerType)
        {
            var entity = DbContext.Model.FindEntityType(typeof(T).FullName);
            var annotationName = GetAnnotationName<T>(triggerTime, triggerType);
            var annotation = entity.GetAnnotation(annotationName);
            return (string)annotation.Value;
        }
    }
}
