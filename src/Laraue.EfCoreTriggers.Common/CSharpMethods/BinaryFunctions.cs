using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.CSharpMethods
{
    /// <summary>
    /// Helper for binary functions translation.
    /// </summary>
    public static class BinaryFunctions
    {
        /// <summary>
        /// Translation of the <see cref="ExpressionType.Coalesce"/> binary.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueIfNull"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Coalesce<T>(T value, T valueIfNull)
        {
            return value ?? valueIfNull;
        }
    }
}