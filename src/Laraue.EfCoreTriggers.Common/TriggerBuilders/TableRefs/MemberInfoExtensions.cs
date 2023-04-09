using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs
{
    /// <summary>
    /// Helper methods allows to return information
    /// about <see cref="ITableRef{TEntity}"/> if <see cref="MemberInfo"/>
    /// represents the table reference.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Get argument type for the passed member.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static ArgumentType GetArgumentType(this MemberInfo memberInfo)
        {
            if (memberInfo.TryGetNewTableRef(out _))
            {
                return ArgumentType.New;
            }

            return memberInfo.TryGetOldTableRef(out _)
                ? ArgumentType.Old
                : ArgumentType.Default;
        }
    
        /// <summary>
        /// Returns the type of table reference if the passed member
        /// represents a <see cref="IOldTableRef{TEntity}"/>.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        public static bool TryGetOldTableRef(this MemberInfo memberInfo, [NotNullWhen(true)] out Type? refType)
        {
            return TryGetTableRef(memberInfo, typeof(IOldTableRef<>), out refType);
        }

        /// <summary>
        /// Returns the type of table reference if the passed member
        /// represents a <see cref="INewTableRef{TEntity}"/>.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        public static bool TryGetNewTableRef(this MemberInfo memberInfo, [NotNullWhen(true)] out Type? refType)
        {
            return TryGetTableRef(memberInfo, typeof(INewTableRef<>), out refType);
        }
    
        private static bool TryGetTableRef(this MemberInfo memberInfo, Type tableRefType, [NotNullWhen(true)] out Type? refType)
        {
            refType = null;

            var reflectedType = memberInfo.ReflectedType
                                ?? throw new InvalidOperationException("Passed member does not contain reflected type");
        
            if (reflectedType.GenericTypeArguments.Length != 1)
            {
                return false;
            }

            var genericArgType = reflectedType.GenericTypeArguments[0];
            if (!genericArgType.IsClass)
            {
                return false;
            }

            if (!typeof(ITableRef).IsAssignableFrom(reflectedType))
            {
                return false;
            }

            var tableRefProperty = tableRefType.GetProperties().Single();
            if (memberInfo.Name != tableRefProperty.Name)
            {
                return false;
            }
        
            refType = genericArgType;
            return true;
        }
    }
}