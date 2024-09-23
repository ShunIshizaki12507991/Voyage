namespace Voyage.Common
{
    using System.Collections.Generic;

    public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public static ReferenceEqualityComparer<T> Instance { get; } = new ReferenceEqualityComparer<T>();

        public bool Equals(T x, T y)
            => ReferenceEquals(x, y);

        public int GetHashCode(T obj)
        {
            if (obj != null)
                return obj.GetHashCode();
            return 0;
        }
    }
}