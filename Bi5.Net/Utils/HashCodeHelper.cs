using System;
using System.Collections.Generic;

namespace Bi5.Net.Utils
{
    /// <summary>
    /// Provides HashCode functionality for .NET Standard 2.0, which doesn't have the HashCode.Combine method
    /// </summary>
    public static class HashCodeHelper
    {
        /// <summary>
        /// Combines hash codes for multiple objects
        /// </summary>
        public static int Combine<T>(T obj)
        {
            return obj?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Combines hash codes for two objects
        /// </summary>
        public static int Combine<T1, T2>(T1 obj1, T2 obj2)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj1?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj2?.GetHashCode() ?? 0);
                return hash;
            }
        }

        /// <summary>
        /// Combines hash codes for three objects
        /// </summary>
        public static int Combine<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj1?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj2?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj3?.GetHashCode() ?? 0);
                return hash;
            }
        }

        /// <summary>
        /// Combines hash codes for four objects
        /// </summary>
        public static int Combine<T1, T2, T3, T4>(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj1?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj2?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj3?.GetHashCode() ?? 0);
                hash = hash * 23 + (obj4?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}