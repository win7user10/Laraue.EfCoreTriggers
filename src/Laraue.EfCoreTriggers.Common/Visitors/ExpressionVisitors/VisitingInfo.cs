using System;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    /// <summary>
    /// Information about members visiting.
    /// </summary>
    public class VisitingInfo
    {
        /// <summary>
        /// Current visiting member.
        /// </summary>
        public MemberInfo? CurrentMember { get; internal set; }
    }

    internal static class VisitingInfoExtensions
    {
        /// <summary>
        /// Change current visiting member info, executes the passed action,
        /// sets the previous visiting member info.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="memberInfo"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T ExecuteWithChangingMember<T>(
            this VisitingInfo info,
            MemberInfo memberInfo,
            Func<T> action)
        {
            var previousMember = info.CurrentMember;
            info.CurrentMember = memberInfo;

            try
            {
                return action();
            }
            finally
            {
                info.CurrentMember = previousMember;
            }
        }
    }
}