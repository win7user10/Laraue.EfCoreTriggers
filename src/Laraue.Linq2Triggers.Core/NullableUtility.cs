using System;

namespace Laraue.Linq2Triggers.Core
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class NullableUtility
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