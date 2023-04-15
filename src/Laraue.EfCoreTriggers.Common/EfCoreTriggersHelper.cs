using System;

namespace Laraue.EfCoreTriggers.Common
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class EfCoreTriggersHelper
    {
        /// <summary>
        /// Get underlying type if it is nullable or return this type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNotNullableType(Type type)
        {
            var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        
            return nullableUnderlyingType ?? type;
        }
    }
}